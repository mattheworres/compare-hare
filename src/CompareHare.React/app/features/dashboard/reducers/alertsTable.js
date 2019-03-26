import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  LOAD_ALERTS_PENDING,
  LOAD_ALERTS_REJECTED,
  LOAD_ALERTS_FULFILLED,
} from '../actions/alertsTable';

const initialState = new Map({
  loading: false,
  alerts: [],
  hasError: false,
});

export default stateReducer(initialState, {
  [LOAD_ALERTS_PENDING]: () => initialState.set('loading', true),

  [LOAD_ALERTS_REJECTED]: state =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('hasError', true);
    }),

  [LOAD_ALERTS_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('alerts', payload.data);
    }),
});
