import {put} from 'truefit-react-utils';

export const TOGGLE_PRODUCT = 'TOGGLE_PRODUCT';
export const TOGGLE_PRODUCT_PENDING = `${TOGGLE_PRODUCT}_PENDING`;
export const TOGGLE_PRODUCT_FULFILLED = `${TOGGLE_PRODUCT}_FULFILLED`;
export const TOGGLE_PRODUCT_REJECTED = `${TOGGLE_PRODUCT}_REJECTED`;

export function toggleProduct(trackedProductId, enabled) {
  return {
    type: TOGGLE_PRODUCT,
    payload: put(`products/${trackedProductId}/toggle`, {trackedProductId, enabled})
  };
}
