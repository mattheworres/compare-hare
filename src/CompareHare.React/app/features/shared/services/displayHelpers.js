export function translateBooleanForDisplay(boolean) {
  return boolean === true ? 'Yes' : 'No';
}

export function retrieveAttributeValue(domEvent, valueName) {
  return domEvent && domEvent.currentTarget
    ? domEvent.currentTarget.getAttribute(valueName)
    : null;
}

export function printMoney(amount) {
  return amount.toLocaleString('en-US', { style: 'currency', currency: 'USD'});
} 

export default {translateBooleanForDisplay, retrieveAttributeValue, printMoney};
