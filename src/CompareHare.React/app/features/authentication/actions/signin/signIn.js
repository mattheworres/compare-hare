import {post} from 'truefit-react-utils';

export const SIGN_IN = 'SIGNIN';
export const SIGN_IN_PENDING = `${SIGN_IN}_PENDING`;
export const SIGN_IN_FULFILLED = `${SIGN_IN}_FULFILLED`;
export const SIGN_IN_REJECTED = `${SIGN_IN}_REJECTED`;

export function signIn(model) {
  return {
    type: SIGN_IN,
    payload: post('authentication/sign-in', model),
  };
}
