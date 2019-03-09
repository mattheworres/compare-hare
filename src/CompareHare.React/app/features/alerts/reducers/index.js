/* eslint-disable sort-imports */
import {combineReducers} from 'redux';
import addAlert from './addAlert.js';
import paPower from './paPower.js';

export default combineReducers({
  addAlert,
  paPower,
});
