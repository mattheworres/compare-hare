import {makeStandardSelector} from '../../../../util/selectors';
// import { createSelector } from "reselect";

export const productsTableSelector = state => state.features.products.productsTable;

// export const loadingSelector = createSelector(
//   productsTableSelector,
//   productsTable => productsTable.get('loading'),
// );

// export const productsSelector = createSelector(
//   productsTableSelector,
//   productsTable => productsTable.get('products'),
// );

// export const deletingSelector = createSelector(
//   productsTableSelector,
//   productsTable => productsTable.get('deleting'),
// );

export const loadingSelector = makeStandardSelector(() => productsTableSelector, 'loading');
export const productsSelector = makeStandardSelector(() => productsTableSelector, 'products');
export const deletingSelector = makeStandardSelector(() => productsTableSelector, 'deleting');
