import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  INITIALIZE_PA_POWER,
  OPEN_ADD_PA_POWER,
  OPEN_ADD_PA_POWER_2,
  OPEN_ADD_PA_POWER_3,
  CLOSE_PA_POWER,
  OPEN_EDIT_PA_POWER,
  OPEN_EDIT_PA_POWER_2,
  OPEN_EDIT_PA_POWER_3,
} from '../actions/paPower';
import {AlertModel} from '../models';

const initialState = new Map({
  addOpen: false,
  editOpen: false,
  addStage: 1,
  editStage: 1,
  alertModel: new AlertModel(),
});

export default stateReducer(initialState, {
  [INITIALIZE_PA_POWER]: (state, payload) =>
    state.withMutations(map => {
      map.set('alertModel', new AlertModel(payload));
    }),

  [OPEN_ADD_PA_POWER]: state =>
    state.withMutations(map => {
      map.set('addOpen', true);
      map.set('addStage', 1);
    }),

  [OPEN_EDIT_PA_POWER]: state =>
    state.withMutations(map => {
      map.set('editOpen', true);
      map.set('editStage', 1);
    }),

  [OPEN_EDIT_PA_POWER_2]: state => state.set('editStage', 2),
  [OPEN_EDIT_PA_POWER_3]: state => state.set('editStage', 3),

  [OPEN_ADD_PA_POWER_2]: (state, payload) =>
    state.withMutations(map => {
      const {
        name,
        hasMinimumPrice,
        minimumPrice,
        hasMaximumPrice,
        maximumPrice,
        hasMinimumMonthLength,
        minimumMonthLength,
        hasMaximumMonthLength,
        maximumMonthLength,
      } = payload;
      map.setIn(['alertModel', 'name'], name);
      map.setIn(['alertModel', 'hasMinimumPrice'], hasMinimumPrice);
      map.setIn(['alertModel', 'minimumPrice'], hasMinimumPrice ? minimumPrice : 0);
      map.setIn(['alertModel', 'hasMaximumPrice'], hasMaximumPrice);
      map.setIn(['alertModel', 'maximumPrice'], hasMaximumPrice ? maximumPrice : 0);
      map.setIn(['alertModel', 'hasMinimumMonthLength'], hasMinimumMonthLength);
      map.setIn(['alertModel', 'minimumMonthLength'], hasMinimumMonthLength ? minimumMonthLength : 0);
      map.setIn(['alertModel', 'hasMaximumMonthLength'], hasMaximumMonthLength);
      map.setIn(['alertModel', 'maximumMonthLength'], hasMaximumMonthLength ? maximumMonthLength : 0);
      map.set('addStage', 2);
    }),

  [OPEN_ADD_PA_POWER_3]: (state, payload) =>
    state.withMutations(map => {
      const {
        filterRenewable,
        hasRenewable,
        minimumRenewablePercent,
        maximumRenewablePercent,
        filterCancellationFee,
        hasCancellationFee,
        filterMonthlyFee,
        hasMonthlyFee,
        filterEnrollmentFee,
        hasEnrollmentFee,
        filterRequiresDeposit,
        requiresDeposit,
        filterBulkDiscounts,
        hasBulkDiscounts,
      } = payload;

      map.setIn(['alertModel', 'filterRenewable'], filterRenewable);
      map.setIn(['alertModel', 'hasRenewable'], hasRenewable);
      map.setIn(['alertModel', 'minimumRenewablePercent'], minimumRenewablePercent);
      map.setIn(['alertModel', 'maximumRenewablePercent'], maximumRenewablePercent);
      map.setIn(['alertModel', 'filterCancellationFee'], filterCancellationFee);
      map.setIn(['alertModel', 'hasCancellationFee'], hasCancellationFee);
      map.setIn(['alertModel', 'filterMonthlyFee'], filterMonthlyFee);
      map.setIn(['alertModel', 'hasMonthlyFee'], hasMonthlyFee);
      map.setIn(['alertModel', 'filterEnrollmentFee'], filterEnrollmentFee);
      map.setIn(['alertModel', 'hasEnrollmentFee'], hasEnrollmentFee);
      map.setIn(['alertModel', 'filterRequiresDeposit'], filterRequiresDeposit);
      map.setIn(['alertModel', 'requiresDeposit'], requiresDeposit);
      map.setIn(['alertModel', 'filterBulkDiscounts'], filterBulkDiscounts);
      map.setIn(['alertModel', 'hasBulkDiscounts'], hasBulkDiscounts);
      map.set('addStage', 3)
    }),

  [CLOSE_PA_POWER]: () => initialState,
});
