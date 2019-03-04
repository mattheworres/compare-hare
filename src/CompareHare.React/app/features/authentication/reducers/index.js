/* eslint-disable sort-imports */
import {combineReducers} from 'redux';
import currentUser from './currentUser.js';
import signin from './signin.js';

export default combineReducers({
  currentUser,
  signin,
});
