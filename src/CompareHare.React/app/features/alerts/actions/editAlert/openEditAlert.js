import {get} from 'truefit-react-utils';
import {initializePaPower, openEditPaPower} from '../paPower';

export const OPEN_EDIT_ALERT = 'OPEN_EDIT_ALERT';
export const OPEN_EDIT_ALERT_PENDING = `${OPEN_EDIT_ALERT}_PENDING`;
export const OPEN_EDIT_ALERT_FULFILLED = `${OPEN_EDIT_ALERT}_FULFILLED`;
export const OPEN_EDIT_ALERT_REJECTED = `${OPEN_EDIT_ALERT}_REJECTED`;

export function openEditAlert(alertId) {
  return dispatch => {
    const editAlert = get(`alerts/${alertId}/edit`);

    dispatch({type: OPEN_EDIT_ALERT, payload: editAlert});

    editAlert.then(response => {
      const {utilityState} = response.data;
      dispatch(initializePaPower(response.data));

      switch(utilityState) {
        case 1:
          dispatch(openEditPaPower());
          break;
      }
    });
  }
}
