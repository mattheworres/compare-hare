import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  LOAD_PRODUCT_CURRENT_PENDING,
  LOAD_PRODUCT_CURRENT_REJECTED,
  LOAD_PRODUCT_CURRENT_FULFILLED,
  TOGGLE_PRODUCT_RETAILER_PENDING,
  TOGGLE_PRODUCT_RETAILER_REJECTED,
  TOGGLE_PRODUCT_RETAILER_FULFILLED
} from '../actions/productDisplay'

const initialState = new Map({
  loading: false,
  deleting: false,
  product: {},
  productRetailers: [],
  hasError: false,
  toggling: false,
  toggleError: null
});

export default stateReducer(initialState, {
  [LOAD_PRODUCT_CURRENT_PENDING]: () => initialState.set('loading', true),

  [LOAD_PRODUCT_CURRENT_REJECTED]: state =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('hasError', true);
    }),

  [LOAD_PRODUCT_CURRENT_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('product', payload.data);
      // if (payload.data.productRetailers) {
      //   map.set('productRetailers')
      // }
    }),

  [TOGGLE_PRODUCT_RETAILER_PENDING]: state => 
    state.withMutations(map => {
      map.set('toggling', true);
      map.set('toggleError', null);
    }),

  [TOGGLE_PRODUCT_RETAILER_REJECTED]: (state, payload) =>
    state.withMutations(map => {
      map.set('toggling', false);
      map.set('toggleError', payload);
    }),

  [TOGGLE_PRODUCT_RETAILER_FULFILLED]: state => state.set('toggling', false),
});
