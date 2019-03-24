import {post} from 'truefit-react-utils';

export const SAVE_ALERT = 'SAVE_ALERT';
export const SAVE_ALERT_PENDING = `${SAVE_ALERT}_PENDING`;
export const SAVE_ALERT_FULFILLED = `${SAVE_ALERT}_FULFILLED`;
export const SAVE_ALERT_REJECTED = `${SAVE_ALERT}_REJECTED`;

export const POPULATE_OFFERS = 'POPULATE_OFFERS';
export const POPULATE_OFFERS_PENDING = `${POPULATE_OFFERS}_PENDING`;
export const POPULATE_OFFERS_FULFILLED = `${POPULATE_OFFERS}_FULFILLED`;
export const POPULATE_OFFERS_REJECTED = `${POPULATE_OFFERS}_REJECTED`;

export function saveAlert(alertModel) {
  return dispatch => {
    const newAlert = post('alerts', alertModel);

    dispatch({type: SAVE_ALERT, payload: newAlert});

    newAlert.then(response => {
      const {indexWasCreatedOrUpdated, alertId} = response.data;

      if (indexWasCreatedOrUpdated) {
        dispatch({type: POPULATE_OFFERS, payload: post(`offers/populate/${alertId}`)});
      }
    });
  }
}
