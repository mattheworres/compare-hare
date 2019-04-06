import {createSelector} from 'reselect';
import alertsTableSelector from './alertsTableSelector';

export default createSelector(
  alertsTableSelector,
  alertsTable => alertsTable.get('deleting'),
);
