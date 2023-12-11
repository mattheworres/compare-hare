import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  CLOSE_ADD_MANUAL,
  OPEN_ADD_MANUAL,
  CHECK_MANUAL_DATE_FULFILLED,
  CHECK_MANUAL_DATE_PENDING,
  // CHECK_MANUAL_DATE_REJECTED,
  SAVE_MANUAL_FULFILLED,
  SAVE_MANUAL_PENDING,
  SAVE_MANUAL_REJECTED,
} from '../actions/addManual'
import {AddManualPriceModel} from '../models';

const initialState = new Map({
    addManualOpen: false,
    trackedProductId: null,
    trackedProductRetailerId: null,
    loading: false,
    dateChecking: false,
    dateCheck: null,
    manualModel: new AddManualPriceModel(),
    saving: false,
    saveError: null,
});

export default stateReducer(initialState, {
  [OPEN_ADD_MANUAL]: (state, payload) =>
    state.withMutations(map => {
      const {trackedProductId, trackedProductRetailerId} = payload;
      map.set('trackedProductId', trackedProductId);
      map.set('trackedProductRetailerId', trackedProductRetailerId);
      map.set('addManualOpen', true);
    }),

  [CHECK_MANUAL_DATE_PENDING]: state => state.set('dateChecking', true),
  [CHECK_MANUAL_DATE_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      const dateIsValid = Boolean(payload.data);
      map.set('dateChecking', false);
      map.set('dateCheck', dateIsValid);
    }),

  [SAVE_MANUAL_PENDING]: state => state.set('saving', true),

  [SAVE_MANUAL_REJECTED]: (state, payload) =>
    state.withMutations(map => {
      const { response } = payload;
      console.log('Error:', response.data);
      map.set('saveError', response.data);
      map.set('saving', false);
    }),

  [SAVE_MANUAL_FULFILLED]: state => state.set('saving', false),

  [CLOSE_ADD_MANUAL]: () => initialState
});