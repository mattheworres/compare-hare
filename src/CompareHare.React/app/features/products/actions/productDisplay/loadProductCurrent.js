import {get} from 'truefit-react-utils';

export const LOAD_PRODUCT_CURRENT = 'LOAD_PRODUCT_CURRENT';
export const LOAD_PRODUCT_CURRENT_PENDING = `${LOAD_PRODUCT_CURRENT}_PENDING`;
export const LOAD_PRODUCT_CURRENT_FULFILLED = `${LOAD_PRODUCT_CURRENT}_FULFILLED`;
export const LOAD_PRODUCT_CURRENT_REJECTED = `${LOAD_PRODUCT_CURRENT}_REJECTED`;

export function loadProductCurrent(trackedProductId) {
  console.log('getting it for ', trackedProductId);
  return {
    type: LOAD_PRODUCT_CURRENT,
    payload: get(`products/current/${trackedProductId}`),
  };
}
