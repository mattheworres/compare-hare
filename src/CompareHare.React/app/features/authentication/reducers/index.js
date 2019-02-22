/* eslint-disable sort-imports */
import {combineReducers} from 'redux';
import currentUser from './currentUser.js';
import login from './login.js';

export default combineReducers({
  currentUser,
  login,
});
