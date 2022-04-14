import {get} from 'truefit-react-utils';

export const CHECK_MANUAL_DATE = 'CHECK_MANUAL_DATE';
export const CHECK_MANUAL_DATE_PENDING = `${CHECK_MANUAL_DATE}_PENDING`;
export const CHECK_MANUAL_DATE_FULFILLED = `${CHECK_MANUAL_DATE}_FULFILLED`;
export const CHECK_MANUAL_DATE_REJECTED = `${CHECK_MANUAL_DATE}_REJECTED`;

export function checkManualDate(manualModel) {
  return {
    type: CHECK_MANUAL_DATE,
    payload: get(`prices/manual/${manualModel.trackedProductRetailerId}/check?date=${manualModel.priceDate}T00:00:00.000Z`)
  };
}
