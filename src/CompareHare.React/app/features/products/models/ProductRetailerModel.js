import {Record} from 'immutable';

export default Record({
  id: null,
  productRetailer: null, // Corresponds to enum value
  productRetailerDisplayName: null, // For FE only, backend cares not
  isOtherRetailer: false,           // For FE only, backend cares not
  otherRetailerDisplayName: null,
  scrapeUrl: null,
  priceSelector: null
});
