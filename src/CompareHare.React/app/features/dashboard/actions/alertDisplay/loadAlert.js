import {get} from 'truefit-react-utils';

export const LOAD_ALERT = 'LOAD_ALERT';
export const LOAD_ALERT_PENDING = `${LOAD_ALERT}_PENDING`;
export const LOAD_ALERT_FULFILLED = `${LOAD_ALERT}_FULFILLED`;
export const LOAD_ALERT_REJECTED = `${LOAD_ALERT}_REJECTED`;

export function loadAlert(alertId) {
  return {
    type: LOAD_ALERT,
    payload: get(`alerts/${alertId}`),
  };
}
