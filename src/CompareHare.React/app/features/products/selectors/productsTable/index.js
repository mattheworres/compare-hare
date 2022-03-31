import {makeStandardSelector} from '../../../../util/selectors';

export const productsTableSelector = state => state.features.products.productsTable;

export const loadingSelector = makeStandardSelector(productsTableSelector, 'loading');
export const productsSelector = makeStandardSelector(productsTableSelector, 'products');
export const deletingSelector = makeStandardSelector(productsTableSelector, 'deleting');
export const hasErrorSelector = makeStandardSelector(productsTableSelector, 'hasError');
