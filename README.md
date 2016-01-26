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

#### Database tables/objects

**user_alert_criteria** - tied to a user (is private to that user), stores the distributor/ratetype key for that user as well as any other specific criteria that should trigger an alert. Timestamp last time local prices were checked for a given alert.

**user_alerts** - Specific offer information that is generated from the alert job described below. Only contains "active" or matching offers. Each time the alert job is run for a user this table should be emptied out. In addition to getting email notifications, the user will have a "dashboard" of current alerts.

**utility_prices** - a place to store the data received back from data sources. One row per "offer id", timestamped.

At first I thought using a document-based DB like **Mongo** would be helpful here, but as I envision 60+ implementations of a utility price scraper interface all slinging their own format of data into the same table I'm terrified.

Instead, I should work to extract a common interface for the information, since they all describe *roughly the same thing*. So in addition to a DB-generated primary key, a string for the unique "offer id" from the store, the energy generator (so "North American Power" is the company name), the type of offer (so fixed, dynamic, flat fee, etc.), the price and the units (that'll get fun I'm sure)

**historical_utility_prices** - every time an insert is performed on *utility_prices*, an additional insert is done on this table. For an MVP I do not plan to do anything with this data, but thought it might be helpful information to track historical information about a given offer. Timestamped inserts.

#### Scheduled Runs/Jobs

There will be two processes that need to run, and each will have state/utility-specific in order to perform:

**utility_price** data gathering - a CRON job will need to run on a weekly basis to gather all pricing data for all distributors listed in the alerts table. For PA-specific jobs, a `SELECT DISTINCT` on the distributor/ratetype key will provide a list of requests to be made on the PUC site, for example. Consider running this job as a one-off (for a single distributor/ratetype) each time a user alert criteria is created (but skip if data already exists in the utility_prices table).

Still not sure how to handle the fact that a single data source will get hammered for 60 unique offer types of data. Rather than shotgunning 60 requests at it (which might look like a DDOS), maybe make a request, then wait 10 seconds? Would like input here.

**email alert generation** - a CRON job will need to run on a less-than-weekly basis to compare all utility_prices that are newer than the last time the user's alert criteria was checked. Maybe run it every other day? Find a way to run this once after a new alert criteria is created, after the pricing data has been obtained.
