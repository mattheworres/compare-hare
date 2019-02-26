import {post} from 'truefit-react-utils';

export const LOG_IN = 'LOGIN';
export const LOG_IN_PENDING = `${LOG_IN}_PENDING`;
export const LOG_IN_FULFILLED = `${LOG_IN}_FULFILLED`;
export const LOG_IN_REJECTED = `${LOG_IN}_REJECTED`;

export function login(model) {
  return {
    type: LOG_IN,
    payload: post('authentication/log-in', model),
  };
}
