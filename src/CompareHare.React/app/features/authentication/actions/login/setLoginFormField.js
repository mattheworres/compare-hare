export const SET_LOGIN_FORM_FIELD = 'SET_LOGIN_FORM_FIELD';

export function setLoginFormField(name, value) {
  return {
    type: SET_LOGIN_FORM_FIELD,
    payload: {name, value},
  };
}
