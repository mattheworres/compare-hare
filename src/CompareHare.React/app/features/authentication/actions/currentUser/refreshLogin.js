import {post} from 'truefit-react-utils';
import {authenticate} from './authenticate';

export const REFRESH_LOGIN = 'REFRESH_LOGIN';

export function refreshLogin() {
  return dispatch => {
    const refresh = post('authentication/refresh-login');

    dispatch({type: REFRESH_LOGIN, payload: refresh});

    refresh.then(response => {
      dispatch(authenticate(response.data));
    });
  };
}
