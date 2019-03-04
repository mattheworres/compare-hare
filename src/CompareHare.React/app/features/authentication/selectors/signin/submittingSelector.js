import {createSelector} from 'reselect';
import signinSelector from './signinSelector';

export default createSelector(
  signinSelector,
  signin => signin.get('submitting'),
);
