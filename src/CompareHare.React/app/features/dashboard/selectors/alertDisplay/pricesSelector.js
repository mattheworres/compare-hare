import {createSelector} from 'reselect';
import alertDisplaySelector from './alertDisplaySelector';

export default createSelector(
  alertDisplaySelector,
  alertDisplay => alertDisplay.get('prices').toJS(),
);
