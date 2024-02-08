export const PA_POWER_DISTRIBUTOR_CHANGED = 'PA_POWER_DISTRIBUTOR_CHANGED';

export function paPowerDistributorChanged(distributorId) {
  return {
    type: PA_POWER_DISTRIBUTOR_CHANGED,
    payload: distributorId,
  };
}
