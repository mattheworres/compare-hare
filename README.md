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
