# Product Price Architecture (March 2022)
Given that I want to track appliance prices from multiple retailers for each single product, I thought the infra I had already built out for CompareHare would serve well for this purpose. Primary among the needs, I want to have a graph per each product to show prices over time, and I also want a way to notify when something interesting happens (both need persistent data)

#### Database Tables/objects
**ProductRetailer** - not a table, but a constant enum. The list for this can be pretty long, but needs to be codified so the user is required to track this in some way. In the future, difficult-to-scrape retailers will have their own frontend/backend code tied to the enum value, but for now it provides uniqueness to the frontend.

**ProductRetailerPriceHistory** - The data gleaned from a single URL scrape. We are given an URL and a selector, so we grab the price (for now, later, other fields?). Is timestamped on create. These records correspond to UtilityPriceHistory, except there's only 1 table for products - most of the interesting bits of graphing or notification happen on a diff from the latest price to the last price (per retailer), so makes no sense to have 2 tables.

**TrackedProduct** - corresponds to Alert/AlertCriteria, tied to a user (is private), has a display name for the user to know what it is

**TrackedProductRetailer** - is 1 of many children of a TrackedProduct, contains a URL, a specific Retailer and a selector (for now, just price, but in future, other fields? image?) these are more or less what the product scraper(s) will pay attention to

**TrackedProductCriteria** - FUTURE, but in order to customize how one wants to be alerted, lets the user specify under what conditions should this tracked product scrapes generate notifications. Optional key to TrackedProductRetailer?