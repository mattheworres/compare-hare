import {stateReducer} from 'truefit-react-utils';
import {fromJS, Map, List} from 'immutable';
import {
  OPEN_ADD_PRODUCT_PENDING,
  OPEN_ADD_PRODUCT_REJECTED,
  OPEN_ADD_PRODUCT_FULFILLED,
  OPEN_ADD_PRODUCT_2,
  OPEN_ADD_PRODUCT_2_NEW_RETAILER,
  OPEN_ADD_PRODUCT_3,
  OPEN_ADD_PRODUCT_4,
  CLOSE_ADD_PRODUCT,
  SAVE_PRODUCT_PENDING,
  SAVE_PRODUCT_REJECTED,
  SAVE_PRODUCT_FULFILLED
} from '../actions/addProduct'
import ProductRetailerModel from '../models/ProductRetailerModel';

const initialState = new Map({
    addOpen: false,
    addStage: 1,
    loading: false,
    loadError: null,
    productRetailerOptions: new List(),
    productName: null, // For now, its just a name, so lets KISS 💋
    productRetailers: new List(),
    newProductRetailer: new ProductRetailerModel(),
    saving: false,
    saveError: null,
    newProductId: null,
});

const newProductRetailerModelName = 'newProductRetailer';

export default stateReducer(initialState, {
  [OPEN_ADD_PRODUCT_PENDING]: state =>
    state.withMutations(map => {
      map.set('loading', true);
      map.set('loadError', null);
    }),

  [OPEN_ADD_PRODUCT_REJECTED]: state =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('loadError', 'Loading create product failed... sorry?');
    }),

  [OPEN_ADD_PRODUCT_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      const {retailers} = payload.data;
      
      map.set('loading', false);
      map.set('addOpen', true);
      map.set('addStage', 1);
      map.set('productName', null);
      map.set('productRetailerOptions', fromJS(retailers));
    }),

  // Go to form 2 from 1 or 3
  [OPEN_ADD_PRODUCT_2]: (state, payload) =>
    state.withMutations(map => {
      const {name} = payload || {}; //payload is form values

      if (name) {
        map.set('productName', name);
      }

      map.set('addStage', 2)
    }),

  // Want to add additional retailer to new product before saving
  [OPEN_ADD_PRODUCT_2_NEW_RETAILER]: state =>
    state.withMutations(map => {
      map.set('addStage', 2);
      map.set('newProductRetailer', new ProductRetailerModel());
    }),

  // Go to form 3 from 2 or 4 (enter new retailer details)
  [OPEN_ADD_PRODUCT_3]: (state, payload) =>
    state.withMutations(map => {
      const {productRetailer, otherRetailerDisplayName} = payload; //payload is form values
      const list = map.get('productRetailerOptions').toJS();
      const retailerIsOther = productRetailer === 1001; //TODO: magic number
      const productRetailerDisplayName = retailerIsOther
        ? otherRetailerDisplayName
        : list.filter(r => parseInt(r.value) === productRetailer)[0].label;
      
      map.setIn([newProductRetailerModelName, 'productRetailer'], productRetailer);
      map.setIn([newProductRetailerModelName, 'isOtherRetailer'], retailerIsOther);
      map.setIn([newProductRetailerModelName, 'otherRetailerDisplayName'], retailerIsOther ? otherRetailerDisplayName : null);
      map.setIn([newProductRetailerModelName, 'productRetailerDisplayName'], productRetailerDisplayName);
      map.set('addStage', 3);
    }),

  // Go to form 4 from 3 (review)
  [OPEN_ADD_PRODUCT_4]: (state, payload) =>
    state.withMutations(map => {
      const {scrapeUrl, priceSelector} = payload; //payload is form values
      const priceSelectorValue = map.get(newProductRetailerModelName).get('isOtherRetailer') ? priceSelector : null;
      map.setIn([newProductRetailerModelName, 'scrapeUrl'], scrapeUrl);
      map.setIn([newProductRetailerModelName, 'priceSelector'], priceSelectorValue)
      // const productRetailer = map.get('newProductRetailer');

      // productRetailer.scrapeUrl = scrapeUrl;
      // productRetailer.set('scrapeUrl', scrapeUrl);
      // productRetailer.priceSelector = productRetailer.isOtherRetailer ? priceSelector : null;
      // productRetailer.set('priceSelector', productRetailer.get('isOtherRetailer') ? priceSelector : null);

      map.set('productRetailers', map.get('productRetailers').push(map.get(newProductRetailerModelName)));
      map.set('newProductRetailer', new ProductRetailerModel());
      map.set('addStage', 4)
    }),

  [CLOSE_ADD_PRODUCT]: state => state.set('addOpen', false),

  [SAVE_PRODUCT_PENDING]: state =>
    state.withMutations(map => {
      map.set('saving', true);
      map.set('saveError', null);
    }),

  [SAVE_PRODUCT_REJECTED]: state =>
    state.withMutations(map => {
      map.set('saving', false);
      map.set('saveError', 'Saving product failed... sorry?');
    }),

  [SAVE_PRODUCT_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      const {id} = payload.data;
      map.set('saving', false);
      map.set('newProductId', id);
    })
});