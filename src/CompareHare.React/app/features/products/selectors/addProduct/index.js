import {makeStandardSelector} from '../../../../util/selectors';
import {createSelector} from 'reselect';

export const addProductSelector = state => state.features.products.addProduct;

export const addOpen = makeStandardSelector(addProductSelector, 'addOpen');
// export const addStage = makeStandardSelector(addProductSelector, 'addStage');
export const addOpen1stSelector = createSelector(
  addProductSelector,
  addProduct => (addProduct.get('addOpen') === true && addProduct.get('addStage') === 1)
);
export const addOpen2ndSelector = createSelector(
  addProductSelector,
  addProduct => (addProduct.get('addOpen') === true && addProduct.get('addStage') === 2)
);
export const addOpen3rdSelector = createSelector(
  addProductSelector,
  addProduct => (addProduct.get('addOpen') === true && addProduct.get('addStage') === 3)
);
export const addOpen4thSelector = createSelector(
  addProductSelector,
  addProduct => (addProduct.get('addOpen') === true && addProduct.get('addStage') === 4)
);
export const loadingSelector = makeStandardSelector(addProductSelector, 'loading');
export const loadErrorSelector = makeStandardSelector(addProductSelector, 'loadError');
export const productRetailerOptionsSelector = makeStandardSelector(addProductSelector, 'productRetailerOptions');
export const productNameSelector = makeStandardSelector(addProductSelector, 'productName');
export const productRetailersSelector = makeStandardSelector(addProductSelector, 'productRetailers');
export const savingSelector = makeStandardSelector(addProductSelector, 'saving');
export const saveErrorSelector = makeStandardSelector(addProductSelector, 'saveError');
export const newProductIdSelector = makeStandardSelector(addProductSelector, 'newProductId');
export const newProductRetailerSelector = makeStandardSelector(addProductSelector, 'newProductRetailer');

