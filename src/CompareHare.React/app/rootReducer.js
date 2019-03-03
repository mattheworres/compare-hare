/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router'
import authentication from './features/authentication/reducers';

const rootReducer = history => combineReducers({
  features: combineReducers({
    authentication: authentication,
  }),
  router: connectRouter(history),
});

export default rootReducer;
