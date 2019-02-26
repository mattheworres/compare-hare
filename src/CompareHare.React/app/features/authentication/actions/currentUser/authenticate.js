export const AUTHENTICATE = 'AUTHENTICATE';

export function authenticate(payload) {
  return {
    type: AUTHENTICATE,
    payload,
  };
}
