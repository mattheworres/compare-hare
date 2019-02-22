import {stateReducer} from 'truefit-react-utils';
import {Map, fromJS} from 'immutable';
import {
  LOG_IN_FULFILLED,
  LOG_IN_PENDING,
  LOG_IN_REJECTED,
  RESET_LOGIN_FORM,
  SET_LOGIN_FORM_FIELD,
} from '../actions/login';
import {LoginModel} from '../models';

const initialState = new Map({
  submitting: false,
  validationErrors: new Map(),
  model: new LoginModel(),
});

export default stateReducer(initialState, {
  [LOG_IN_PENDING]: state => state.set('submitting', true),

  [LOG_IN_FULFILLED]: () => initialState,

  [LOG_IN_REJECTED]: (state, payload) =>
    state.withMutations(map => {
      map.set('submitting', false);

      const {status, data} = payload.response || {};
      map.set('validationErrors', status === 400 ? fromJS(data) : Map());
    }),

  [RESET_LOGIN_FORM]: state => state.set('model', new LoginModel()),

  [SET_LOGIN_FORM_FIELD]: (state, payload) => {
    const {name, value} = payload;
    return state.setIn(['model', name], value);
  },
});
