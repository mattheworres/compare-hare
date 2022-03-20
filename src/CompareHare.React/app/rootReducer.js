/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import authentication from './features/authentication/reducers';
import alerts from './features/alerts/reducers';
import dashboard from './features/dashboard/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    authentication: authentication,
    alerts: alerts,
    dashboard: dashboard,
  }),
  router: connectRouter(history),
});

export default rootReducer;
