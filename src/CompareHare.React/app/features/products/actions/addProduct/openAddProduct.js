import {get} from 'truefit-react-utils';

export const OPEN_ADD_PRODUCT = 'OPEN_ADD_PRODUCT';
export const OPEN_ADD_PRODUCT_PENDING = `${OPEN_ADD_PRODUCT}_PENDING`;
export const OPEN_ADD_PRODUCT_FULFILLED = `${OPEN_ADD_PRODUCT}_FULFILLED`;
export const OPEN_ADD_PRODUCT_REJECTED = `${OPEN_ADD_PRODUCT}_REJECTED`;
export const OPEN_ADD_PRODUCT_2 = 'OPEN_ADD_PRODUCT_2';
export const OPEN_ADD_PRODUCT_2_NEW_RETAILER = 'OPEN_ADD_PRODUCT_2_NEW_RETAILER';
export const OPEN_ADD_PRODUCT_3 = 'OPEN_ADD_PRODUCT_3';
export const OPEN_ADD_PRODUCT_4 = 'OPEN_ADD_PRODUCT_4';

export function openAddProduct() {
  return {
    type: OPEN_ADD_PRODUCT,
    payload: get('products/create'),
  };
}

export function openAddProduct2(formValues) {
  return {
    type: OPEN_ADD_PRODUCT_2,
    payload: formValues,
  };
}

export function openAddProduct2NewRetailer() {
  return {
    type: OPEN_ADD_PRODUCT_2_NEW_RETAILER
  };
}

export function openAddProduct3(formValues) {
  return {
    type: OPEN_ADD_PRODUCT_3,
    payload: formValues,
  };
}

export function openAddProduct4(formValues) {
  return {
    type: OPEN_ADD_PRODUCT_4,
    payload: formValues,
  };
}