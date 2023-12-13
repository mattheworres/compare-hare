export const OPEN_ADD_MANUAL = 'OPEN_ADD_MANUAL';

export function openAddManual(trackedProductId, trackedProductRetailerId) {
  return {
    type: OPEN_ADD_MANUAL,
    payload: {trackedProductId, trackedProductRetailerId}
  };
}
