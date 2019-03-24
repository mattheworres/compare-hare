import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  INITIALIZE_ADD_PA_POWER,
  OPEN_ADD_PA_POWER,
  OPEN_ADD_PA_POWER_2,
  OPEN_ADD_PA_POWER_3,
  CLOSE_ADD_PA_POWER,
} from '../actions/paPower';
import {AlertModel} from '../models';

const initialState = new Map({
  addOpen: false,
  addStage: 1,
  alertModel: new AlertModel(),
});

export default stateReducer(initialState, {
  [INITIALIZE_ADD_PA_POWER]: (state, payload) =>
    state.withMutations(map => {
      const {zip, utilityType, utilityState} = payload;
      map.setIn(['alertModel', 'zip'], zip);
      map.setIn(['alertModel', 'utilityType'], utilityType);
      map.setIn(['alertModel', 'utilityState'], utilityState);
    }),

  [OPEN_ADD_PA_POWER]: state =>
    state.withMutations(map => {
      map.set('addOpen', true);
      map.set('addStage', 1);
    }),

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

  //SUBMIT TO SERVER HERE

  [CLOSE_ADD_PA_POWER]: () => initialState,
});
