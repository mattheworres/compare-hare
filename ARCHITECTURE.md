# Compare Hare

An app to create &amp; manage power and gas price notifications. (March 2022: also, to add product price tracking history/alerting)

Some US states have [deregulated power and/or gas utilities](http://www.deregulationofenergy.org/) in order to allow consumers to have a choice in how much they pay for their energy. Depending on how a particular consumer prefers to pay for their utilities, it'd be nice to have a service that notifies you of when a lower priced plan becomes available. Enter the Compare Hare.

For electricity consumers in Pennsylvania, the PA Public Utility Commission (PUC)'s [PA Power Switch](http://www.papowerswitch.com/) website provides a powerful search tool in order to search for applicable plans and filter accordingly. This search and filter is powered by a JSON backend service that would be easily interfaced with in order to feed pricing data in.

## So, here's how it would work

1. User signs up for an account, verifies email for account.
2. User starts a new "alert" by selecting a state (from the ones we've implemented), and a utility type (gas/electric, from what we've implemented), and then enter their zipcode.
3. From here we'll need specific client implementations for each StateUtility implemented. For PA Electric (first MVP implementation) the app `POST`'s to <http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-zipcode> with `zipcode` provided, and `ajax` set to `true`
4. The PUC server will respond with the applicable _Distributors_, such as `duquesne-light` or `penn-power`
5. App `POST`'s to <http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-distributor> with the `distributor` from above, which will hand back information for that particular distributor. Using the above examples, Penn Power only has one **rate type**, so the default can be used, but Duquesne Light has 4 separate rate types, so the user must choose the applicable rate type.
6. App then `POST`'s to <http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-distributor/>

- If the distributor only has one rate type (Penn Power), then only `distributor` is passed
- If there are multiple rate types, then the `ratetype` property is populated with the **slug** of the rate type appended to the distributor value (so for Duquesne Light, `duquesne-light/rs` would be passed for residential service)
  6a. At this point the app will also update the user's default ratetype and default state to autofill the alert criteria creation form next time

7. At this point, we will get back JSON data of all the applicable offers for the user. This is useful once, but in reality the PUC's website has already provided a pretty useful interface to get this pricing data. **So, what's the point of Compare Hare?**
8. We can store the specific string passed to the `by-distributor` call (either `penn-power` or `duquesne-light/rs`) along with other specific information for the user. For instance, the user can indicate they only want to be notified if **fixed price** offers of **renewable energy** that have **no cancellation fee** for a price of at or below **$0.0550 per kWh**. With a table of stored alert criteria, we can periodically query the PUC's website for updated pricing information and then send out user alerts accordingly.

## Architecture Ideas

When imagining how this would work, I envision some things needing to exist:

### General Arch

For an "MVP", the first integration will be for Pennsylvania power through the PUC website described above. In addition to PA natural gas also being regulated there are [29 other states](http://www.alliedpowerservices.com/deregulated-states.shtml) that provide either partial or full deregulation for electricity and natural gas.

With this in mind, I'd like to keep state and utility-specific logic and integrations separated as makes sense. Some states might also make this a lot harder (before investigating the PUC site, I was gearing up to create a web scraper...)

#### Assumptions

- When getting pricing data, after scraping/cleaning data into our format, we need to have a way to hash the entire data set and compare it against the last time we scraped data in order to determine if anything has changed. If the hash is the same, we stop right here and do not wipe/insert rows into the UtilityPrice table for this particular StateUtilityIndex.

- Use [object-hash](https://github.com/puleos/object-hash) for SHA1 hashing

- Alerts will only send notifications when the hash of the matching offers has changed.

Given example: PA Power sends the same 5 offers for 3 months. Two of the offers match the very first time, a notification is sent. Every subsequent scrape stops with the OfferLoader because the offer hash never changes there.

Then, they add 1 new offer. The OfferLoader offer hash will have changed, but when the AlertAssessor views the hash of all matching offers (still 2 offers), it is determined that no change has occurred, therefore no new notification is sent.

- Existence of a row in utilityPrices means that price is "current". No rows found matching a specific state, energy type and rate type means that any alerts with those three are no longer valid (and must be removed). utilityPriceHistories remain so that even though utilityPrices is constantly emptied and refilled with the "latest" data, alerts can maintain strong referential integrity to the information for display purposes.

#### Database tables/objects

**AlertCriteria** - tied to a user (is private to that user), stores the distributor/ratetype key for that user as well as any other specific criteria that should trigger an alert. Includes the StateUtilityIndex hash from the last time the alert criteria was checked. Keys: distributor_id, user_id

**AlertMatch** - Specific offer information that is generated from the alert job described below. Only contains "active" or matching offers. In addition to getting email notifications, the user will have a "dashboard" of current alerts. Keys: alert_criteria_id (parent), user_id, utility_price_history_id (utility prices are wiped often, so the history entry should be used for all displays). Also contains the AlertOffer hash from the matching offer data.

**PendingAlertNotifications** - a message table where each row denotes a notification that needs sent to a specific user. Only generated when a "new" alert is found (part 2 of the alert comp service below). Will also queue one or more "new" price hits into a single notification. Will contain some sort of information pertaining to new prices found - either just a "We found 3 new prices that match your alert "Bob's Alert", click here for info" or a "We found 3 new offers from these companies: [company list] click here for more info" message in the email. Once email has been sent, the row is deleted.

**UtilityPrices** - a place to store the data received back from data sources. One row per "offer id", timestamped create only ("update" is meaningless - only inserts and deletes for now). Keys: utility_price_history_id (has many-to-many association with distributors through a linker table). May need to find out if database can automatically NULL out the utility_price_id field on alerts when we delete rows from this table

**UtilityPriceHistories** - every time an insert is performed on _utility_prices_, an additional insert is done on this table. For an MVP I do not plan to do anything with this data, but thought it might be helpful information to track historical information about a given offer. Timestamped inserts. Keys: none (has many-to-many assoc with distributors thru linker table)

**StateUtilityIndices** - LoaderDataIdentifier, State, UtilityType, Active, LastUpdated, LastUpdateHash
A table of indices unique by State and UtilityType, this table provides logic for both of the backend services that run all Offer Loaders and Alert Assessors. LastUpdateHash = reliable hash of all offer data that allows us to cheaply determine if anything has changed since the last time we got data.

#### State/Utility Specific Logic

This is where the architecture gets interesting. Each state AND utility type will likely have its own flavor on how to do things, so we need to split out all of this logic in a few interesting ways:

**AlertCreators** - responsible for providing the frontend user with specific prompts and data input, these also have backend components that take this data input and create AlertCriteria, and also Inserts/Updates to StateUtilityIndices.

**AlertAssessors** - responsible for periodically checking a user's alert criteria with the up to date offer data. First checks the StateUtilityIndexHash of the AlertCriteria and the StateUtilityIndex. If the same, stop here. Then assesses the alert criteria against the newest offer data, and compares the hash of matching offers to the AlertOffer hash. If the hash differs: If there are no matching offers, remove alert. Otherwise, the alert has changed, so send a notification.

**OfferLoaders** - unique per State and UtilityType, offer loaders are responsible for loading new offer data for a given LoaderDataIdentifier from all matching rows in StateUtilityIndices. Can be run both as a one-off (a new row added to StateUtilityIndices when a new AlertCriteria is also created) and is also run periodically in the offer loader update service below. Leverages hash checks against LastUpdatedHash to short-circuit before spurious delete/inserts is performed on UtilityPrices.

#### Scheduled Runs/Jobs

**offer loader update service** - a regularly scheduled job to run all offer loaders on all of their matching identifiers from the StateUtilityIndices table. Must use rate limiting with whatever library we go with. Suggestion: [Bull](https://www.npmjs.com/package/bull)

**alert assessor service** - Cycle thru all alert_criteria and find criteria that have updated pricing data (comparison of StateUtilityIndexHash on alert_criteria against the value in the matching StateUtilityIndices row). Once a criteria needs updated (the hash is different), load pricing data for that criteria's matching state utility index.

Then, calculate the hash for matching offers from the newest pricing data and compare against the hash in the specific alert.
_If there is no alert AND there are > 0 matching offers, create a new alert + notification
_ If there is an alert but the hash is the same, do nothing.
_If there is an alert and the hash differs, send an alert
_ If there is an alert and there are 0 new matching offers, remove the alert

**notification service** - Cycle through all pending_alert_notifications and send emails as necessary. The pending_alert_notification table contains all necessary info in order to generate said email. Once notification has been sent, the pending_alert_notification row can be deleted.
