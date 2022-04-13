import {makeStandardSelector} from '../../../../util/selectors';

export const addManualSelector = state => state.features.products.addManual;

export const addManualOpen = makeStandardSelector(addManualSelector, 'addManualOpen');
export const loadingSelector = makeStandardSelector(addManualSelector, 'loading');
export const saveErrorSelector = makeStandardSelector(addManualSelector, 'saveError');
export const dateCheckingSelector = makeStandardSelector(addManualSelector, 'dateChecking');
export const dateCheckSelector = makeStandardSelector(addManualSelector, 'dateCheck');
export const savingSelector = makeStandardSelector(addManualSelector, 'saving');
export const manualModelSelector = makeStandardSelector(addManualSelector, 'manualModel');

