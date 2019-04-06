import {httpDelete} from 'truefit-react-utils';

export const DELETE_ALERT = 'DELETE_ALERT';
export const DELETE_ALERT_PENDING = `${DELETE_ALERT}_PENDING`;
export const DELETE_ALERT_FULFILLED = `${DELETE_ALERT}_FULFILLED`;
export const DELETE_ALERT_REJECTED = `${DELETE_ALERT}_REJECTED`;

export function deleteAlert(alertId) {
  return {
    type: DELETE_ALERT,
    payload: httpDelete(`alerts/${alertId}`),
  };
}
