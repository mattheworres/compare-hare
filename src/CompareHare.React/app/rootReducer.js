/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import dashboard from './features/dashboard/reducers';
import authentication from './features/authentication/reducers';
import alerts from './features/alerts/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    dashboard: dashboard,
    authentication: authentication,
    alerts: alerts,
  }),
  router: connectRouter(history),
});

export default rootReducer;
