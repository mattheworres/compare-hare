/* eslint-disable sort-imports */
import {combineReducers} from 'redux';
import addProduct from './addProduct.js';
import productDisplay from './productDisplay.js';
import productsTable from './productsTable.js';

export default combineReducers({
  addProduct,
  productDisplay,
  productsTable,
});
