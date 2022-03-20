export const INITIALIZE_PA_POWER = 'INITIALIZE_PA_POWER';

export function initializePaPower(model) {
  return {
    type: INITIALIZE_PA_POWER,
    payload: model,
  };
}
