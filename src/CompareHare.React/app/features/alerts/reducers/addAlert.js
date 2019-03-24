import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  OPEN_ADD_ALERT,
  CLOSE_ADD_ALERT,
  SAVE_ALERT_PENDING,
  SAVE_ALERT_REJECTED,
  SAVE_ALERT_FULFILLED,
  //POPULATE_OFFERS_PENDING,
  POPULATE_OFFERS_REJECTED,
  POPULATE_OFFERS_FULFILLED,
} from '../actions/addAlert';

const initialState = new Map({
  addOpen: false,
  saveAlertOpen: false,
  creatingAlert: false,
  loadingOffers: false,
  offersCompared: false,
  matchingOffersCount: 0,
  saveError: false,
  alertId: null,
});

export default stateReducer(initialState, {
  [OPEN_ADD_ALERT]: () => initialState.set('addOpen', true),
  [CLOSE_ADD_ALERT]: () => initialState,

  [SAVE_ALERT_PENDING]: state => state.set('saveAlertOpen', true),
  //[SAVE_ALERT_REJECTED]: state => state.set('????'),
  [SAVE_ALERT_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      const {indexWasCreatedOrUpdated, matchesCount, alertId} = payload.data;

      map.set('creatingAlert', true);
      map.set('alertId', alertId);

      if (!indexWasCreatedOrUpdated) {
        map.set('loadingOffers', true);
        map.set('offersCompared', true);
        map.set('matchingOffersCount', matchesCount);
      }
    }),

  //[POPULATE_OFFERS_PENDING]: state => state.set('????'),
  [POPULATE_OFFERS_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      const {matchesCount} = payload.data;

      map.set('loadingOffers', true);
      map.set('offersCompared', true);
      map.set('matchingOffersCount', matchesCount);
    }),

  [POPULATE_OFFERS_REJECTED]: state => state.set('saveError', true),
  [SAVE_ALERT_REJECTED]: state => state.set('saveError', true),
});
