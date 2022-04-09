import {makeStandardSelector} from '../../../../util/selectors';

export const addProductSelector = state => state.features.products.addProduct;

export const addOpen = makeStandardSelector(addProductSelector, 'addOpen');
export const addStage = makeStandardSelector(addProductSelector, 'addStage');
export const loadingSelector = makeStandardSelector(addProductSelector, 'loading');
export const loadErrorSelector = makeStandardSelector(addProductSelector, 'loadError');
export const productRetailerOptionsSelector = makeStandardSelector(addProductSelector, 'productRetailerOptions');
export const productNameSelector = makeStandardSelector(addProductSelector, 'productName');
export const productRetailersSelector = makeStandardSelector(addProductSelector, 'productRetailers');
export const savingSelector = makeStandardSelector(addProductSelector, 'saving');
export const saveErrorSelector = makeStandardSelector(addProductSelector, 'saveError');
export const newProductIdSelector = makeStandardSelector(addProductSelector, 'newProductId');

