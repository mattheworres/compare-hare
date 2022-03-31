import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  LOAD_PRODUCTS_PENDING,
  LOAD_PRODUCTS_REJECTED,
  LOAD_PRODUCTS_FULFILLED,
} from '../actions/productsTable';

const initialState = new Map({
  loading: false,
  deleting: false,
  products: [],
  hasError: false,
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
});
