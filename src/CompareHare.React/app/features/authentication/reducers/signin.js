import {stateReducer} from 'truefit-react-utils';
import {Map, fromJS} from 'immutable';
import {
  SIGN_IN_FULFILLED,
  SIGN_IN_PENDING,
  SIGN_IN_REJECTED,
} from '../actions/signin';

const initialState = new Map({
  submitting: false,
  validationErrors: new Map(),
});

export default stateReducer(initialState, {
  [SIGN_IN_PENDING]: state => state.set('submitting', true),

  [SIGN_IN_FULFILLED]: () => initialState,

  [SIGN_IN_REJECTED]: (state, payload) =>
    state.withMutations(map => {
      map.set('submitting', false);

      const {status, data} = payload.response || {};
      map.set('validationErrors', status === 400 ? fromJS(data) : Map());
    }),
});
