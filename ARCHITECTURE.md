# Compare Hare
An app to create &amp; manage power and gas price notifications.

Some US states have [deregulated power and/or gas utilities](http://www.deregulationofenergy.org/) in order to allow consumers to have a choice in how much they pay for their energy. Depending on how a particular consumer prefers to pay for their utilities, it'd be nice to have a service that notifies you of when a lower priced plan becomes available. Enter the Compare Hare.

For electricity consumers in Pennsylvania, the PA Public Utility Commission (PUC)'s [PA Power Switch](http://www.papowerswitch.com/) website provides a powerful search tool in order to search for applicable plans and filter accordingly. This search and filter is powered by a JSON backend service that would be easily interfaced with in order to feed pricing data in.

#### So, here's how it would work:

1. User signs up for an account, verifies email for account.
2. User starts a new "alert" by entering their zipcode.
3. App `POST`'s to http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-zipcode with `zipcode` provided, and `ajax` set to `true`
4. The PUC server will respond with the applicable *Distributors*, such as `duquesne-light` or `penn-power`
5. App `POST`'s to http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-distributor with the `distributor` from above, which will hand back information for that particular distributor. Using the above examples, Penn Power only has one **rate type**, so the default can be used, but Duquesne Light has 4 separate rate types, so the user must choose the applicable rate type.
6. App then `POST`'s to http://www.papowerswitch.com/shop-for-electricity/shop-for-your-home/by-distributor/ 
  - If the distributor only has one rate type (Penn Power), then only `distributor` is passed
  - If there are multiple rate types, then the `ratetype` property is populated with the **slug** of the rate type appended to the distributor value (so for Duquesne Light, `duquesne-light/rs` would be passed for residential service)
7. At this point, we will get back JSON data of all the applicable offers for the user. This is useful once, but in reality the PUC's website has already provided a pretty useful interface to get this pricing data. **So, what's the point of Compare Hare?**
8. We can store the specific string passed to the `by-distributor` call (either `penn-power` or `duquesne-light/rs`) along with other specific information for the user. For instance, the user can indicate they only want to be notified if **fixed price** offers of **renewable energy** that have **no cancellation fee** for a price of at or below **$0.0550 per kWh**. With a table of stored alert criteria, we can periodically query the PUC's website for updated pricing information and then send out user alerts accordingly.

## Architecture Ideas
When imagining how this would work, I envision some things needing to exist:

#### General Arch

For an "MVP", the first integration will be for Pennsylvania power through the PUC website described above. In addition to PA natural gas also being regulated there are [29 other states](http://www.alliedpowerservices.com/deregulated-states.shtml) that provide either partial or full deregulation for electricity and natural gas.

With this in mind, I'd like to keep state and utility-specific logic and integrations separated as makes sense. Some states might also make this a lot harder (before investigating the PUC site, I was gearing up to create a web scraper...)

#### Assumptions

- When getting pricing data, the "offer_id" will represent up to date information, and updates within that offer_id will not exist (pricing changes or other major pieces of info that would affect alerts). For this, we will ignore new data from the price scrapes if we already have that offer_id in utility_prices

- Alerts will only send email once - when the alert is created. We cannot wipe utility_prices OR alerts on every price scrape, or we will be constantly emailing alerts.

#### Database tables/objects

**alert_criteria** - tied to a user (is private to that user), stores the distributor/ratetype key for that user as well as any other specific criteria that should trigger an alert. Timestamp last time utility_prices were checked for a given alert. Keys: distributor_id, user_id

**alerts** - Specific offer information that is generated from the alert job described below. Only contains "active" or matching offers. In addition to getting email notifications, the user will have a "dashboard" of current alerts. Keys: alert_criteria_id (parent), user_id, utility_price_id, utility_price_history_id (utility prices are wiped often, so the history entry should be used for all displays)

**pending_alert_notifications** - a message table where each row denotes a notification that needs sent to a specific user. Only generated when a "new" alert is found (part 2 of the alert comp service below). Will also queue one or more "new" price hits into a single notification. Will contain some sort of information pertaining to new prices found - either just a "We found 3 new prices that match your alert "Bob's Alert", click here for info" or a "We found 3 new offers from these companies: [company list] click here for more info" message in the email. Once email has been sent, the row is deleted.

**utility_prices** - a place to store the data received back from data sources. One row per "offer id", timestamped create only ("update" is meaningless - only inserts and deletes for now). Keys: utility_price_history_id (has many-to-many association with distributors through a linker table). May need to find out if database can automatically NULL out the utility_price_id field on alerts when we delete rows from this table

**historical_utility_prices** - every time an insert is performed on *utility_prices*, an additional insert is done on this table. For an MVP I do not plan to do anything with this data, but thought it might be helpful information to track historical information about a given offer. Timestamped inserts. Keys: none (has many-to-many assoc with distributors thru linker table)

**distributors** - in the PUC data (see [SAMPLE_PA_PUC_DATA.json](SAMPLE_PA_PUC_DATA.json)) this is the distributor through which the user already receives service. New Castle for example uses Penn Power, most Pittsburgh areas use Duquesne Light. Contains info about their distributor, their price-to-compare, and also will contain the unique string that the price scraper will use to gather up to date pricing. Keys: none. Also has a many-to-many assoc with utility_prices AND utility_price_histories

**distributor_updates** - a table to track when a distributor has last been updated to notify upstream processes such as the alert comparison service as to when there's newer data available. Should also contain some sort of hash that allows us to quickly compare the latest data to the last time we got data - so we're not thrashing more on the utility_prices table than we need to. May not be possible.

**users** - standard login thru OAuth methods provided by Angular Fullstack. Keys: distributor_id (is the "default" distributor, can be updated periodically. When adding alert_criteria this is provided as the default) Is the foreign key in 2 other tables: alert_criteria, alerts.



#### Scheduled Runs/Jobs

**utility_price** data gathering - a CRON job will need to run on a weekly basis to gather all pricing data for all distributors listed in the alerts table. For PA-specific jobs, a `SELECT DISTINCT` on the distributor/ratetype key will provide a list of requests to be made on the PUC site, for example. Consider running this job as a one-off (for a single distributor/ratetype) each time a user alert criteria is created (but skip if data already exists in the utility_prices table). When the update runs, if the hash on distributor_updates indicates there's newer data, we first empty that distributor/ratetype's utility_prices, and then proceed to insert the new prices and then finally update distributor_updates with both a timestamp and the hash value.

Still not sure how to handle the fact that a single data source will get hammered for 60 unique offer types of data. Rather than shotgunning 60 requests at it (which might look like a DDOS), maybe make a request, then wait 10 seconds? Would like input here.

**alert comparison service** - Cycle thru all alert_criteria and find criteria that have updated pricing data (comparison of the timestamp on alert_criteria and distributor_updates). Once a criteria needs updated, load all pricing data for that criteria's distributor. Then, it's a two step process:
    - One, cycle through all existing alerts tied to the criteria. If the alert points to a utility_price that has been updated, update the alert with the newest utility_price_id. If it does not have an updated utility_price row - this means the alert is no longer valid. Remove the alert, ensure there are no outstanding alert_notification links to the same utility_price external identifier. This step will **absolutely** rely on the assumption that there is a utility price-specific ID that is unique to that specific offer, otherwise every time we get new prices we will generate noisy notifications.
    - Two, cycle thru the rest of the utility prices for the distributor/ratetype (do an exclusion of IDs based on the ones that currently exist). Add new alert objects when a utility_price matches. Create or update an pending_alert_notification row as necessary

**notification service** - Cycle through all pending_alert_notifications and send emails as necessary. The pending_alert_notification table contains all necessary info in order to generate said email. Once notification has been sent, the pending_alert_notification row can be deleted.