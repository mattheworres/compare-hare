import { createSelector } from "reselect";

export const makeStandardSelector = (firstSelector, propertyName) => createSelector(
    firstSelector,
    secondSelector => secondSelector.get(propertyName)
  );
