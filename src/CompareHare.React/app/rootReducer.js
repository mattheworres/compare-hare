/* eslint-disable sort-imports */
/* eslint-disable camelcase */
/* eslint-disable object-shorthand */
import {combineReducers} from 'redux';
import {connectRouter} from 'connected-react-router';
import authentication from './features/authentication/reducers';
import featureGroups from './features/featureGroups/reducers';
import games from './features/games/reducers';

const rootReducer = history =>
  combineReducers({
    features: combineReducers({
      authentication: authentication,
      featureGroups: featureGroups,
      games: games,
    }),
    router: connectRouter(history),
  });

export default rootReducer;
