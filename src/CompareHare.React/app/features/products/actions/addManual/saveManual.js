import {post} from 'truefit-react-utils';

export const SAVE_MANUAL = 'SAVE_MANUAL';
export const SAVE_MANUAL_PENDING = `${SAVE_MANUAL}_PENDING`;
export const SAVE_MANUAL_FULFILLED = `${SAVE_MANUAL}_FULFILLED`;
export const SAVE_MANUAL_REJECTED = `${SAVE_MANUAL}_REJECTED`;

export function saveManual(trackedProductRetailerId, manualModel, callback) {
  return dispatch => {
    const saveManual = post(`prices/manual/${trackedProductRetailerId}`, {trackedProductRetailerId, ...manualModel});

    dispatch({type: SAVE_MANUAL, payload: saveManual});

    saveManual.then(() => {
      callback && typeof(callback) === "function" && callback();
    })
  }
}
