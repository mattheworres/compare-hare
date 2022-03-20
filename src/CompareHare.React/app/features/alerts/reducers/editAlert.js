import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  OPEN_EDIT_ALERT_PENDING,
  OPEN_EDIT_ALERT_REJECTED,
  OPEN_EDIT_ALERT_FULFILLED,
  UPDATE_ALERT_PENDING,
  UPDATE_ALERT_REJECTED,
  UPDATE_ALERT_FULFILLED,
  CLOSE_EDIT_ALERT,
} from '../actions/editAlert';

const initialState = new Map({
  loading: false,
  updateProgressOpen: false,
  updating: false,
  matchingOffersCount: 0,
  updateError: false,
  alertId: null,
});

export default stateReducer(initialState, {
  [OPEN_EDIT_ALERT_PENDING]: state => state.set('loading', true),
  [OPEN_EDIT_ALERT_REJECTED]: state => state.set('loading', false),
  [OPEN_EDIT_ALERT_FULFILLED]: state => state.set('loading', false),

  [UPDATE_ALERT_PENDING]: state =>
    state.withMutations(map => {
      map.set('updateProgressOpen', true);
      map.set('updating', true);
    }),

  [UPDATE_ALERT_REJECTED]: state =>
    state.withMutations(map => {
      map.set('updating', false);
      map.set('updateError', true);
    }),

  [UPDATE_ALERT_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      const {matchesCount, alertId} = payload.data;

      map.set('updating', false);
      map.set('alertId', alertId);
      map.set('matchingOffersCount', matchesCount);
    }),

  [CLOSE_EDIT_ALERT]: () => initialState,
});
