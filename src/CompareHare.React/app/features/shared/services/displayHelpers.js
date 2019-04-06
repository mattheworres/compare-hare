export function translateBooleanForDisplay(boolean) {
  return boolean === true ? 'Yes' : 'No';
}

export function retrieveAttributeValue(domEvent, valueName) {
  return domEvent && domEvent.currentTarget
    ? domEvent.currentTarget.getAttribute(valueName)
    : null;
}

export default {translateBooleanForDisplay, retrieveAttributeValue};
