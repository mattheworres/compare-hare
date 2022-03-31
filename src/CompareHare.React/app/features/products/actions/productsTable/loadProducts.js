import {get} from 'truefit-react-utils';

export const LOAD_PRODUCTS = 'LOAD_PRODUCTS';
export const LOAD_PRODUCTS_PENDING = `${LOAD_PRODUCTS}_PENDING`;
export const LOAD_PRODUCTS_FULFILLED = `${LOAD_PRODUCTS}_FULFILLED`;
export const LOAD_PRODUCTS_REJECTED = `${LOAD_PRODUCTS}_REJECTED`;

export function loadProducts() {
  console.log('uh, so here we are');

  return {
    type: LOAD_PRODUCTS,
    payload: get('products/list'),
  };
}
