import {makeStandardSelector} from '../../../../util/selectors';

export const productDisplaySelector = state => state.features.products.productDisplay;

export const loadingSelector = makeStandardSelector(productDisplaySelector, 'loading');
export const productSelector = makeStandardSelector(productDisplaySelector, 'product');
export const deletingSelector = makeStandardSelector(productDisplaySelector, 'deleting');
export const hasErrorSelector = makeStandardSelector(productDisplaySelector, 'hasError');
export const togglingSelector = makeStandardSelector(productDisplaySelector, 'toggling');
export const toggleErrorSelector = makeStandardSelector(productDisplaySelector, 'toggleError');
