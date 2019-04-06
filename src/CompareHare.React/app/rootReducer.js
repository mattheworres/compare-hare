/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import alerts from './features/alerts/reducers';
import authentication from './features/authentication/reducers';
import dashboard from './features/dashboard/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    alerts: alerts,
    authentication: authentication,
    dashboard: dashboard,
  }),
  router: connectRouter(history),
});

export default rootReducer;
