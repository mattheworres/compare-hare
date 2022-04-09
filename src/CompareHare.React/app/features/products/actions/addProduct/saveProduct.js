import {post} from 'truefit-react-utils';

export const SAVE_PRODUCT = 'SAVE_PRODUCT';
export const SAVE_PRODUCT_PENDING = `${SAVE_PRODUCT}_PENDING`;
export const SAVE_PRODUCT_FULFILLED = `${SAVE_PRODUCT}_FULFILLED`;
export const SAVE_PRODUCT_REJECTED = `${SAVE_PRODUCT}_REJECTED`;

export function saveProduct(productModel) {
  return {
    type: SAVE_PRODUCT,
    payload: post('products', productModel),
  };
}
