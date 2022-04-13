export const OPEN_ADD_MANUAL = 'OPEN_ADD_MANUAL';

export function openAddManual(trackedProductRetailerId) {
  return {
    type: OPEN_ADD_MANUAL,
    payload: trackedProductRetailerId
  };
}
