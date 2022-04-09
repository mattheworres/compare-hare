/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import authentication from './features/authentication/reducers';
import dashboard from './features/dashboard/reducers';
import alerts from './features/alerts/reducers';
import products from './features/products/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    authentication: authentication,
    dashboard: dashboard,
    alerts: alerts,
    products: products,
  }),
  router: connectRouter(history),
});

export default rootReducer;
