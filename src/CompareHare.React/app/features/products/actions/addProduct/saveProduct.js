import {post} from 'truefit-react-utils';
import {loadProducts} from '../productsTable';

export const SAVE_PRODUCT = 'SAVE_PRODUCT';
export const SAVE_PRODUCT_PENDING = `${SAVE_PRODUCT}_PENDING`;
export const SAVE_PRODUCT_FULFILLED = `${SAVE_PRODUCT}_FULFILLED`;
export const SAVE_PRODUCT_REJECTED = `${SAVE_PRODUCT}_REJECTED`;


export function saveProduct(productModel, callback) {
  return dispatch => {
    const newProduct = post('products/create', productModel);

    dispatch({type: SAVE_PRODUCT, payload: newProduct});

    newProduct.then(() => {// can have response obj here to get ID
      // Dispatch actions we want to act upon the new product ID here
      dispatch(loadProducts());

      callback && typeof(callback) === "function" && callback();
    });
  };
}
