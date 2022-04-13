import {post} from 'truefit-react-utils';

export const SAVE_MANUAL = 'SAVE_MANUAL';
export const SAVE_MANUAL_PENDING = `${SAVE_MANUAL}_PENDING`;
export const SAVE_MANUAL_FULFILLED = `${SAVE_MANUAL}_FULFILLED`;
export const SAVE_MANUAL_REJECTED = `${SAVE_MANUAL}_REJECTED`;

export function saveManual(manualModel, callback) {
  return dispatch => {
    const saveManual = post(`prices/manual/${manualModel.trackedProductRetailerPrice}`);

    dispatch({type: SAVE_MANUAL, payload: saveManual});

    saveManual.then(() => {
      callback && typeof(callback) === "function" && callback();
    })
  }
}
