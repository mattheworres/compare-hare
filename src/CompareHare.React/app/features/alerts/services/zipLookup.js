export default function zipLookup(zipcode) {
  // Ensure param is a string to prevent unpredictable parsing results
  if (typeof zipcode !== 'string') {
    console.log('Must pass the zipcode as a string.');
    return;
  }

  // Ensure you don't parse codes that start with 0 as octal values
  const zipInt = parseInt(zipcode, 10);
  const values = {
    stateCode: null,
    state: null,
  };

  // Code blocks alphabetized by state
  if (zipInt >= 35000 && zipInt <= 36999) {
    values.state = 'Alabama';
  } else if (zipInt >= 99500 && zipInt <= 99999) {
    values.state = 'Alaska';
  } else if (zipInt >= 85000 && zipInt <= 86999) {
    values.state = 'Arizona';
  } else if (zipInt >= 71600 && zipInt <= 72999) {
    values.state = 'Arkansas';
  } else if (zipInt >= 90000 && zipInt <= 96699) {
    values.state = 'California';
  } else if (zipInt >= 80000 && zipInt <= 81999) {
    values.state = 'Colorado';
  } else if (zipInt >= 6000 && zipInt <= 6999) {
    values.state = 'Connecticut';
  } else if (zipInt >= 19700 && zipInt <= 19999) {
    values.state = 'Deleware';
  } else if (zipInt >= 32000 && zipInt <= 34999) {
    values.state = 'Florida';
  } else if (zipInt >= 30000 && zipInt <= 31999) {
    values.state = 'Georgia';
  } else if (zipInt >= 96700 && zipInt <= 96999) {
    values.state = 'Hawaii';
  } else if (zipInt >= 83200 && zipInt <= 83999) {
    values.state = 'Idaho';
  } else if (zipInt >= 60000 && zipInt <= 62999) {
    values.state = 'Illinois';
  } else if (zipInt >= 46000 && zipInt <= 47999) {
    values.state = 'Indiana';
  } else if (zipInt >= 50000 && zipInt <= 52999) {
    values.state = 'Iowa';
  } else if (zipInt >= 66000 && zipInt <= 67999) {
    values.state = 'Kansas';
  } else if (zipInt >= 40000 && zipInt <= 42999) {
    values.state = 'Kentucky';
  } else if (zipInt >= 70000 && zipInt <= 71599) {
    values.state = 'Louisiana';
  } else if (zipInt >= 3900 && zipInt <= 4999) {
    values.state = 'Maine';
  } else if (zipInt >= 20600 && zipInt <= 21999) {
    values.state = 'Maryland';
  } else if (zipInt >= 1000 && zipInt <= 2799) {
    values.state = 'Massachusetts';
  } else if (zipInt >= 48000 && zipInt <= 49999) {
    values.state = 'Michigan';
  } else if (zipInt >= 55000 && zipInt <= 56999) {
    values.state = 'Minnesota';
  } else if (zipInt >= 38600 && zipInt <= 39999) {
    values.state = 'Mississippi';
  } else if (zipInt >= 63000 && zipInt <= 65999) {
    values.state = 'Missouri';
  } else if (zipInt >= 59000 && zipInt <= 59999) {
    values.state = 'Montana';
  } else if (zipInt >= 27000 && zipInt <= 28999) {
    values.state = 'North Carolina';
  } else if (zipInt >= 58000 && zipInt <= 58999) {
    values.state = 'North Dakota';
  } else if (zipInt >= 68000 && zipInt <= 69999) {
    values.state = 'Nebraska';
  } else if (zipInt >= 88900 && zipInt <= 89999) {
    values.state = 'Nevada';
  } else if (zipInt >= 3000 && zipInt <= 3899) {
    values.state = 'New Hampshire';
  } else if (zipInt >= 7000 && zipInt <= 8999) {
    values.state = 'New Jersey';
  } else if (zipInt >= 87000 && zipInt <= 88499) {
    values.state = 'New Mexico';
  } else if (zipInt >= 10000 && zipInt <= 14999) {
    values.state = 'New York';
  } else if (zipInt >= 43000 && zipInt <= 45999) {
    values.state = 'Ohio';
  } else if (zipInt >= 73000 && zipInt <= 74999) {
    values.state = 'Oklahoma';
  } else if (zipInt >= 97000 && zipInt <= 97999) {
    values.state = 'Oregon';
  } else if (zipInt >= 15000 && zipInt <= 19699) {
    values.stateCode = 1;
    values.state = 'Pennsylvania';
  } else if (zipInt >= 300 && zipInt <= 999) {
    values.state = 'Puerto Rico';
  } else if (zipInt >= 2800 && zipInt <= 2999) {
    values.state = 'Rhode Island';
  } else if (zipInt >= 29000 && zipInt <= 29999) {
    values.state = 'South Carolina';
  } else if (zipInt >= 57000 && zipInt <= 57999) {
    values.state = 'South Dakota';
  } else if (zipInt >= 37000 && zipInt <= 38599) {
    values.state = 'Tennessee';
  } else if (
    (zipInt >= 75000 && zipInt <= 79999) ||
    (zipInt >= 88500 && zipInt <= 88599)
  ) {
    values.state = 'Texas';
  } else if (zipInt >= 84000 && zipInt <= 84999) {
    values.state = 'Utah';
  } else if (zipInt >= 5000 && zipInt <= 5999) {
    values.state = 'Vermont';
  } else if (zipInt >= 22000 && zipInt <= 24699) {
    values.state = 'Virgina';
  } else if (zipInt >= 20000 && zipInt <= 20599) {
    values.state = 'Washington DC';
  } else if (zipInt >= 98000 && zipInt <= 99499) {
    values.state = 'Washington';
  } else if (zipInt >= 24700 && zipInt <= 26999) {
    values.state = 'West Virginia';
  } else if (zipInt >= 53000 && zipInt <= 54999) {
    values.state = 'Wisconsin';
  } else if (zipInt >= 82000 && zipInt <= 83199) {
    values.state = 'Wyoming';
  }

  return values;
}
