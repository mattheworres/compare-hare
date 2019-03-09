export const INITIALIZE_ADD_PA_POWER = 'INITIALIZE_ADD_PA_POWER';

export function initializeAddPaPower(model) {
  return {
    type: INITIALIZE_ADD_PA_POWER,
    payload: model,
  };
}
