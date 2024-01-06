/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import dashboard from './features/dashboard/reducers';
import alerts from './features/alerts/reducers';
import products from './features/products/reducers';
import authentication from './features/authentication/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    dashboard: dashboard,
    alerts: alerts,
    products: products,
    authentication: authentication,
  }),
  router: connectRouter(history),
});

export default rootReducer;
