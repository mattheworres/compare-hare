import {stateReducer} from 'truefit-react-utils';
import {LOG_OUT_PENDING, AUTHENTICATE} from '../actions/currentUser';
import {LOAD_IDENTITY_FULFILLED} from '../actions/currentUser/loadIdentity';
import {CurrentUserModel} from '../models';
import createCurrentUserModel from '../services/createCurrentUserModel';

const initialState = new CurrentUserModel();

export default stateReducer(initialState, {
  [AUTHENTICATE]: (state, payload) => createCurrentUserModel(payload),

  [LOAD_IDENTITY_FULFILLED]: (state, payload) =>
    createCurrentUserModel(payload),

  [LOG_OUT_PENDING]: () => initialState,
});
