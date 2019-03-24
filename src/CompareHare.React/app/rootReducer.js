/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import alerts from './features/alerts/reducers';
import authentication from './features/authentication/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    alerts: alerts,
    authentication: authentication,
  }),
  router: connectRouter(history),
});

export default rootReducer;
