export const OPEN_ADD_PA_POWER_2 = 'OPEN_ADD_PA_POWER_2';

export function openAddPaPower2(formValues) {
  return {
    type: OPEN_ADD_PA_POWER_2,
    payload: formValues,
  };
}
