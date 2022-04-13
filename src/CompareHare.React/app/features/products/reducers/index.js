/* eslint-disable sort-imports */
import {combineReducers} from 'redux';
import addManual from './addManual.js';
import addProduct from './addProduct.js';
import productDisplay from './productDisplay.js';
import productsTable from './productsTable.js';

export default combineReducers({
  addManual,
  addProduct,
  productDisplay,
  productsTable,
});
