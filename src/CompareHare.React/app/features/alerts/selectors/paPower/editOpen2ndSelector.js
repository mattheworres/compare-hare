import {createSelector} from 'reselect';
import paPowerSelector from './paPowerSelector';

export default createSelector(
  paPowerSelector,
  paPower => (paPower.get('editOpen') === true && paPower.get('editStage') === 2),
);
