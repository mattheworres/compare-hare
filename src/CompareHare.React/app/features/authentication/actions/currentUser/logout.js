import {httpDelete} from 'truefit-react-utils';

export const LOG_OUT = 'LOG_OUT';
export const LOG_OUT_PENDING = 'LOG_OUT_PENDING';
export const LOG_OUT_FULFILLED = 'LOG_OUT_FULFILLED';

export function logOut(model) {
  return {
    type: LOG_OUT,
    payload: httpDelete('authentication/log-out', model),
  };
}
