import {createSelector} from 'reselect';
import editAlertSelector from './editAlertSelector';

export default createSelector(
  editAlertSelector,
  editAlert => editAlert.get('updateError'),
);
