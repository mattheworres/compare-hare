import { get } from 'truefit-react-utils';
export const INITIALIZE_PA_POWER = 'INITIALIZE_PA_POWER';
// const PA_POWER_DISTRIBUTOR_URL = 'https://www.papowerswitch.com/umbraco/Api/ShopApi/ZipSearch?zipcode={zip}&serviceType=residential';
// const PA_POWER_DISTRIBUTOR_URL = 'https://localhost:8000/public/PA_Distributor_Response.json';

export function initializePaPower(initializeModel) {
  // return {
  //   type: INITIALIZE_PA_POWER,
  //   payload: initializeModel,
  // };
  const { zip } = initializeModel;
  return dispatch => {
    // const url = PA_POWER_DISTRIBUTOR_URL.replace('{zip}', zip);
    // const distributorResponse = get(url, {headers: {'Access-Control-Allow-Origin': '*'}});
    const distributorResponse = get(`paPower/distributors/list/${zip}`);

    distributorResponse.then(response => {
      dispatch({type: INITIALIZE_PA_POWER, payload: {alertModel: initializeModel, distributors: response.data}});
    });
  };
}
