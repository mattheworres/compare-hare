import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  LOAD_PRODUCTS_PENDING,
  LOAD_PRODUCTS_REJECTED,
  LOAD_PRODUCTS_FULFILLED,
  TOGGLE_PRODUCT_PENDING,
  TOGGLE_PRODUCT_REJECTED,
  TOGGLE_PRODUCT_FULFILLED
} from '../actions/productsTable';

const initialState = new Map({
  loading: false,
  deleting: false,
  products: [],
  hasError: false,
  toggling: false,
  toggleError: null
});

export default stateReducer(initialState, {
  [LOAD_PRODUCTS_PENDING]: () => initialState.set('loading', true),

  [LOAD_PRODUCTS_REJECTED]: state =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('hasError', true);
    }),

  [LOAD_PRODUCTS_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('products', payload.data);
    }),

  [TOGGLE_PRODUCT_PENDING]: state => 
    state.withMutations(map => {
      map.set('toggling', true);
      map.set('toggleError', null);
    }),

  [TOGGLE_PRODUCT_REJECTED]: (state, payload) =>
    state.withMutations(map => {
      map.set('toggling', false);
      map.set('toggleError', payload.data);
    }),

  [TOGGLE_PRODUCT_FULFILLED]: state => state.set('toggling', false),
});
