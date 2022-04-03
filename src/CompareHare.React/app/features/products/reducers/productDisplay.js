import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  LOAD_PRODUCT_CURRENT_PENDING,
  LOAD_PRODUCT_CURRENT_REJECTED,
  LOAD_PRODUCT_CURRENT_FULFILLED
} from '../actions/productDisplay'

const initialState = new Map({
  loading: false,
  deleting: false,
  product: {},
  productRetailers: [],
  hasError: false,
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
});
