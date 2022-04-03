/* eslint-disable sort-imports */
import {combineReducers} from 'redux';
import productDisplay from './productDisplay.js';
import productsTable from './productsTable.js';

export default combineReducers({
  productDisplay,
  productsTable,
});
