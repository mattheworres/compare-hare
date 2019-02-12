# Compare Hare

An app to create &amp; manage power and gas price notifications.

Some US states have [deregulated power and/or gas utilities](http://www.deregulationofenergy.org/) in order to allow consumers to have a choice in how much they pay for their energy. Depending on how a particular consumer prefers to pay for their utilities, it'd be nice to have a service that notifies you of when a lower priced plan becomes available. Enter the Compare Hare.

For electricity consumers in Pennsylvania, the PA Public Utility Commission (PUC)'s [PA Power Switch](http://www.papowerswitch.com/) website provides a powerful search tool in order to search for applicable plans and filter accordingly. This search and filter is powered by a JSON backend service that would be easily interfaced with in order to feed pricing data in.

#### So, here's how it would work:

1.  User signs up for an account, verifies email for account.
2.  User starts a new "alert" by selecting a state (from the ones we've implemented), and a utility type (gas/electric, from what we've implemented), and then enter their zipcode.
3.  From here we'll need specific client implementations for each StateUtility implemented. For PA Electric (first MVP implementation) the app `POST`'s to http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-zipcode with `zipcode` provided, and `ajax` set to `true`
4.  The PUC server will respond with the applicable _Distributors_, such as `duquesne-light` or `penn-power`
5.  App `POST`'s to http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-distributor with the `distributor` from above, which will hand back information for that particular distributor. Using the above examples, Penn Power only has one **rate type**, so the default can be used, but Duquesne Light has 4 separate rate types, so the user must choose the applicable rate type.
6.  App then `POST`'s to http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-distributor/

- If the distributor only has one rate type (Penn Power), then only `distributor` is passed
- If there are multiple rate types, then the `ratetype` property is populated with the **slug** of the rate type appended to the distributor value (so for Duquesne Light, `duquesne-light/rs` would be passed for residential service)
  6a. At this point the app will also update the user's default ratetype and default state to autofill the alert criteria creation form next time

7.  At this point, we will get back JSON data of all the applicable offers for the user. This is useful once, but in reality the PUC's website has already provided a pretty useful interface to get this pricing data. **So, what's the point of Compare Hare?**
8.  We can store the specific string passed to the `by-distributor` call (either `penn-power` or `duquesne-light/rs`) along with other specific information for the user. For instance, the user can indicate they only want to be notified if **fixed price** offers of **renewable energy** that have **no cancellation fee** for a price of at or below **$0.0550 per kWh**. With a table of stored alert criteria, we can periodically query the PUC's website for updated pricing information and then send out user alerts accordingly.

## Architecture Ideas

When imagining how this would work, I envision some things needing to exist:

#### General Arch

For an "MVP", the first integration will be for Pennsylvania power through the PUC website described above. In addition to PA natural gas also being regulated there are [29 other states](http://www.alliedpowerservices.com/deregulated-states.shtml) that provide either partial or full deregulation for electricity and natural gas.

With this in mind, I'd like to keep state and utility-specific logic and integrations separated as makes sense. Some states might also make this a lot harder (before investigating the PUC site, I was gearing up to create a web scraper...)

#### Assumptions

- When getting pricing data, the "offer_id" will represent up to date information, and updates within that offer_id will not exist (pricing changes or other major pieces of info that would affect alerts). For this, we will ignore new data from the price scrapes if we already have that offer_id in utility_prices

- Alerts will only send email once - when the alert is created. We cannot wipe utility_prices OR alerts on every price scrape, or we will be constantly emailing alerts.

- Existence of a row in utilityPrices means that price is "current". No rows found matching a specific state, energy type and rate type means that any alerts with those three are no longer valid (and must be removed). utilityPriceHistories remain so that even though utilityPrices is constantly emptied and refilled with the "latest" data, alerts can maintain strong referential integrity to the information for display purposes.

#### Database tables/objects

**alertCriteria** - tied to a user (is private to that user), stores the distributor/ratetype key for that user as well as any other specific criteria that should trigger an alert. Timestamp last time utility_prices were checked for a given alert. Keys: distributor_id, user_id

**alerts** - Specific offer information that is generated from the alert job described below. Only contains "active" or matching offers. In addition to getting email notifications, the user will have a "dashboard" of current alerts. Keys: alert_criteria_id (parent), user_id, utility_price_id, utility_price_history_id (utility prices are wiped often, so the history entry should be used for all displays)

**pendingAlertNotifications** - a message table where each row denotes a notification that needs sent to a specific user. Only generated when a "new" alert is found (part 2 of the alert comp service below). Will also queue one or more "new" price hits into a single notification. Will contain some sort of information pertaining to new prices found - either just a "We found 3 new prices that match your alert "Bob's Alert", click here for info" or a "We found 3 new offers from these companies: [company list] click here for more info" message in the email. Once email has been sent, the row is deleted.

**utilityPrices** - a place to store the data received back from data sources. One row per "offer id", timestamped create only ("update" is meaningless - only inserts and deletes for now). Keys: utility_price_history_id (has many-to-many association with distributors through a linker table). May need to find out if database can automatically NULL out the utility_price_id field on alerts when we delete rows from this table

**utilityPriceHistories** - every time an insert is performed on _utility_prices_, an additional insert is done on this table. For an MVP I do not plan to do anything with this data, but thought it might be helpful information to track historical information about a given offer. Timestamped inserts. Keys: none (has many-to-many assoc with distributors thru linker table)

**distributorRates** - in the PUC data (see [SAMPLE_PA_PUC_DATA.json](SAMPLE_PA_PUC_DATA.json)) this is the distributor through which the user already receives service. New Castle for example uses Penn Power, most Pittsburgh areas use Duquesne Light. Contains info about their distributor, their price-to-compare, and also will contain the unique string that the price scraper will use to gather up to date pricing. A distributorRate is analogous to the price-to-compare given for a user from their distributor (Penn Power or Duquesne Light) for their selected rate type (so Duquesne Light - Residential). Keys: none. Also has a many-to-many assoc with utility_prices AND utility_price_histories

**distributorRateUpdates** - a table to track when a distributor has last been updated to notify upstream processes such as the alert comparison service as to when there's newer data available. Should also contain some sort of hash (of the entire data returned from the PUC API) that allows us to quickly determine if there are any updates to the data, or if its the same with what we got last time - so we're not thrashing more on the utility_prices table than we need to. May not be possible.

**users** - standard login thru OAuth methods provided by Angular Fullstack. Keys: none Is the foreign key in 2 other tables: alert_criteria, alerts.

#### Scheduled Runs/Jobs

**utility price update service** - a CRON job will need to run on a weekly basis to gather all pricing data for all distributors listed in the alerts table. For PA-specific jobs, a `SELECT DISTINCT` on the state/ratetype key from alert_criteria will provide a list of ratetypes to check. Each rate type will then be cycled through existing distributorRates - if no distributorRate exists, the PUC has to be hit to get both the distributorRate and all competing prices. If the distributorRate does exist, check the distributorRateUpdates to see if the PUC site needs hit again. If the hash on distributorRateUpdates indicates there's newer data, we update the distributor rate with the most up to date information, empty that distributor/ratetype's utility_prices, and then proceed to insert the new prices into both utilityPrices and utilityPriceHistories and then finally update distributorRateUpdates with both a timestamp and the hash of the price data from the external API.

Still not sure how to handle the fact that a single data source will get hammered for 60 unique offer types of data. Rather than shotgunning 60 requests at it (which might look like a DDOS), maybe make a request, then wait 10 seconds? Would like input here.

**alert comparison service** - Cycle thru all alert_criteria and find criteria that have updated pricing data (comparison of the timestamp on alertCriteria and distributorRateUpdates). Once a criteria needs updated, load all pricing data for that criteria's distributorRate. Then, it's a two step process: - One, cycle through all existing alerts tied to the criteria. If there are no rows in utilityPrices that match the "priceOfferId" and "distributorState" then this means the alert is no longer valid (the offer it represented is no longer being returned to us by the external API). Remove the alert, ensure there are no outstanding alert_notification links to the same utility_price external identifier. This step will **absolutely** rely on the assumption that there is a utility price-specific ID that is unique to that specific offer, otherwise every time we get new prices we will generate noisy notifications. - Two, cycle thru the rest of the utility prices for the distributor/ratetype (do an exclusion of IDs based on the ones that currently exist). Add new alert objects when a utility_price matches. Create or update an pending_alert_notification row as necessary

**notification service** - Cycle through all pending_alert_notifications and send emails as necessary. The pending_alert_notification table contains all necessary info in order to generate said email. Once notification has been sent, the pending_alert_notification row can be deleted.
