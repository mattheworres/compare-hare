import {put} from 'truefit-react-utils';

export const TOGGLE_PRODUCT_RETAILER = 'TOGGLE_PRODUCT_RETAILER';
export const TOGGLE_PRODUCT_RETAILER_PENDING = `${TOGGLE_PRODUCT_RETAILER}_PENDING`;
export const TOGGLE_PRODUCT_RETAILER_FULFILLED = `${TOGGLE_PRODUCT_RETAILER}_FULFILLED`;
export const TOGGLE_PRODUCT_RETAILER_REJECTED = `${TOGGLE_PRODUCT_RETAILER}_REJECTED`;

export function toggleProductRetailer(trackedProductRetailerId, enabled) {
  return {
    type: TOGGLE_PRODUCT_RETAILER,
    payload: put(`products/retailers/${trackedProductRetailerId}/toggle`, {trackedProductRetailerId, enabled})
  };
}
