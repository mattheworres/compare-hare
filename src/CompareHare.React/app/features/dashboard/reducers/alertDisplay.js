import {stateReducer} from 'truefit-react-utils';
import {Map, List} from 'immutable';
import {
  LOAD_ALERT_PENDING,
  LOAD_ALERT_REJECTED,
  LOAD_ALERT_FULFILLED,
} from '../actions/alertDisplay';
import {AlertDisplayModel, ParameterModel, PriceModel} from '../models';
import {modelsArrayToRecordList} from '../../shared/services';

const initialState = new Map({
  loading: false,
  alert: new AlertDisplayModel(),
  parameters: List(),
  prices: List(),
  hasError: false,
});

export default stateReducer(initialState, {
  [LOAD_ALERT_PENDING]: () => initialState.set('loading', true),

  [LOAD_ALERT_REJECTED]: state =>
    state.withMutations(map => {
      map.set('loading', false);
      map.set('hasError', true);
    }),

  [LOAD_ALERT_FULFILLED]: (state, payload) =>
    state.withMutations(map => {
      const {data} = payload;
      map.set('loading', false);
      map.set('alert', new AlertDisplayModel(data));
      map.set('parameters', modelsArrayToRecordList(data.parameters, ParameterModel));
      map.set('prices', modelsArrayToRecordList(data.prices, PriceModel));
    }),
});
