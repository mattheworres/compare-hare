export function translateBooleanForDisplay(boolean) {
  return boolean === true ? 'Yes' : 'No';
}

export function retrieveAttributeValue(domEvent, valueName) {
  return domEvent && domEvent.target
    ? domEvent.target.getAttribute(valueName)
    : null;
}

export default {translateBooleanForDisplay, retrieveAttributeValue};
