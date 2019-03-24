import {createSelector} from 'reselect';
import addAlertSelector from './addAlertSelector';

export default createSelector(
  addAlertSelector,
  addAlert => addAlert.get('creatingAlert'),
);
