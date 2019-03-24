import {createSelector} from 'reselect';
import paPowerSelector from './paPowerSelector';

export default createSelector(
  paPowerSelector,
  paPower => (paPower.get('addOpen') === true && paPower.get('addStage') === 1),
);
