import {stateReducer} from 'truefit-react-utils';
import {Map} from 'immutable';
import {
  OPEN_ADD_ALERT,
  CLOSE_ADD_ALERT,
} from '../actions/addAlert';

const initialState = new Map({
  open: false,
});

export default stateReducer(initialState, {
  [OPEN_ADD_ALERT]: state => state.set('open', true),
  [CLOSE_ADD_ALERT]: () => initialState,
});
