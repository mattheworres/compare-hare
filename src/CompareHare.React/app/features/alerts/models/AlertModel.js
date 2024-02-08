import {Record} from 'immutable';

// TODO: make specialized to PA Power, or make this more flexible?

export default Record({
  id: null,
  stateUtilityIndexId: null,

  name: '',
  utilityState: null,
  utilityType: null,
  zip: '',            //Loader ID 1
  distributorId: '',  //Loader ID 2
  distributorRate: '',//Loader ID 3

  hasMinimumPrice: false,
  minimumPrice: 0,
  hasMaximumPrice: false,
  maximumPrice: 0,

  hasMinimumMonthLength: false,
  minimumMonthLength: 0,
  hasMaximumMonthLength: false,
  maximumMonthLength: 0,

  filterRenewable: false,
  hasRenewable: false,
  minimumRenewablePercent: 0,
  maximumRenewablePercent: 100,

  filterCancellationFee: false,
  hasCancellationFee: false,

  filterMonthlyFee: false,
  hasMonthlyFee: false,

  filterEnrollmentFee: false,
  hasEnrollmentFee: false,

  filterRequiresDeposit: false,
  requiresDeposit: false,

  filterBulkDiscounts: false,
  hasBulkDiscounts: false,

  comments: '',
});
