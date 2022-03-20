import {put} from 'truefit-react-utils';

export const UPDATE_ALERT = 'UPDATE_ALERT';
export const UPDATE_ALERT_PENDING = `${UPDATE_ALERT}_PENDING`;
export const UPDATE_ALERT_FULFILLED = `${UPDATE_ALERT}_FULFILLED`;
export const UPDATE_ALERT_REJECTED = `${UPDATE_ALERT}_REJECTED`;

export function updateAlert(alertModel) {
  return {
    type: UPDATE_ALERT,
    payload: put(`alert/${alertModel.id}`, alertModel),
  };
}
