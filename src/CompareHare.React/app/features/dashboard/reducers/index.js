/* eslint-disable sort-imports */
import {combineReducers} from 'redux';
import alertDisplay from './alertDisplay.js';
import alertsTable from './alertsTable.js';

export default combineReducers({
  alertDisplay,
  alertsTable,
});
