/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import alerts from './features/alerts/reducers';
import products from './features/products/reducers';
import dashboard from './features/dashboard/reducers';
import authentication from './features/authentication/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    alerts: alerts,
    products: products,
    dashboard: dashboard,
    authentication: authentication,
  }),
  router: connectRouter(history),
});

export default rootReducer;
