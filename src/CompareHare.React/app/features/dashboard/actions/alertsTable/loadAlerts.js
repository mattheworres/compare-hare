import {get} from 'truefit-react-utils';

export const LOAD_ALERTS = 'LOAD_ALERTS';
export const LOAD_ALERTS_PENDING = `${LOAD_ALERTS}_PENDING`;
export const LOAD_ALERTS_FULFILLED = `${LOAD_ALERTS}_FULFILLED`;
export const LOAD_ALERTS_REJECTED = `${LOAD_ALERTS}_REJECTED`;

export function loadAlerts() {
  return {
    type: LOAD_ALERTS,
    payload: get('alerts/list'),
  };
}
