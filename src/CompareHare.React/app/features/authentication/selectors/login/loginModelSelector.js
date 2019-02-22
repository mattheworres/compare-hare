import {createSelector} from 'reselect';
import loginSelector from './loginSelector';

export default createSelector(
  loginSelector,
  login => login.get('model'),
);
