/* eslint-disable prettier/prettier */
/* eslint-disable no-param-reassign */
import requestPromise from 'request-promise';
import cheerio from 'cheerio';

const UTILITY_STATE = 'pa';
const UTILITY_TYPE = 'power';
const NO_ANSWER = 'No';

function generateUrlForIndex(index) {
  return `https://www.papowerswitch.com/shop-for-electricity/shop-for-your-home?zipcode=${index.loaderDataIdentifier}`;
}

function parseLeftCopy(utilityPrice, leftElement) {
  const $element = cheerio(leftElement);
  const priceStructureClasses = $element.find('span.price.price-structure').attr('class').split(' ');
  let priceStructure = 'Fixed';

  if (priceStructureClasses.includes('Variable')) priceStructure = 'Variable';
  else if (priceStructureClasses.includes('Unlimited')) priceStructure = 'Unlimited';

  utilityPrice.priceStructure = priceStructure;

  utilityPrice.hasBulkDiscounts = $element.find('span.discounts strong').text() === 'Yes';
  utilityPrice.isIntroductoryPrice = $element.find('span.introductory strong').text() === 'Yes';
  const renewableText = $element.find('span.renewable strong').text();
  utilityPrice.hasRenewable = renewableText !== NO_ANSWER;
  if (utilityPrice.hasRenewable) utilityPrice.renewablePercentage = renewableText;

  return utilityPrice;
}

function parseMiddleCopy(utilityPrice, middleCopyElement) {
  const $middleElement = cheerio(middleCopyElement);

  const cancellationText = $middleElement.find('span.cancellation strong').text();
  utilityPrice.hasCancellationFee = cancellationText !== NO_ANSWER;
  if (utilityPrice.hasCancellationFee) utilityPrice.cancellationFee = cancellationText;

  const termLengthText = $middleElement.find('span.term-length strong').text();
  if (termLengthText !== NO_ANSWER) utilityPrice.termMonthLength = parseInt(termLengthText, 10);

  const monthlyFeeText = $middleElement.find('span.monthly-fee strong').text();
  utilityPrice.hasMonthlyFee = monthlyFeeText !== NO_ANSWER;
  if (utilityPrice.hasMonthlyFee) utilityPrice.monthlyFee = monthlyFeeText;

  const termEndDateText = $middleElement.find('span.term-end-date strong').text();
  if (termEndDateText !== NO_ANSWER) {
    utilityPrice.termEndDate = $middleElement.find('span.term-end-date strong span').text();
  }

  const enrollmentFeeText = $middleElement.find('span.enrollment-fee strong').text();
  utilityPrice.hasEnrollmentFee = enrollmentFeeText !== NO_ANSWER;
  if (utilityPrice.hasEnrollmentFee) utilityPrice.enrollmentFee = enrollmentFeeText;

  return utilityPrice;
}

function parseRightCopy(utilityPrice, rightCopyElement) {
  const $rightElement = cheerio(rightCopyElement);

  utilityPrice.offerUrl = $rightElement.find('span.sign-up a').attr('href');

  return utilityPrice;
}

function parseCopy(utilityPrice, copyElement) {
  const $element = cheerio(copyElement);

  utilityPrice = parseLeftCopy(utilityPrice, $element.find('div.left'));
  utilityPrice = parseMiddleCopy(utilityPrice, $element.find('div.middle'));
  utilityPrice = parseRightCopy(utilityPrice, $element.find('div.right'));

  return utilityPrice;
}

function parseRates(utilityPrice, supplierRateElement) {
  const $element = cheerio(supplierRateElement);
  const rateText = $element.find('span.rate').text();
  utilityPrice.pricePerUnit = parseFloat(
    rateText.substr(1, rateText.length - 1),
  );

  if (utilityPrice.pricePerUnit === 0) {
    utilityPrice.pricePerUnit = null;
    utilityPrice.flatRate = $element.find('span.unlimited-rate').text();
  } else {
    utilityPrice.priceUnit = `${$element
      .find('span.unit span.word')
      .text()} ${$element.find('span.unit span#unit').text()}`;
  }

  return utilityPrice;
}

function parseSupplier(supplierNameElement) {
  const $element = cheerio(supplierNameElement);
  const nameClasses = $element.attr('class').split(' ');
  const uniqueIdentifier = nameClasses.filter(className =>
    className.startsWith('nid'),
  )[0];

  return {
    name: $element.find('span.name a').text(),
    supplierPhone: $element.find('span.phone').text(),
    utilityState: UTILITY_STATE,
    utilityType: UTILITY_TYPE,
    uniqueIdentifier,
  };
}

function parseHtml(html) {
  const offers = [];

  cheerio('div.views-field-nothing div.shop-inner span.field-content', html)
    .each((index, fieldContent) => {
      let utilityPrice = parseSupplier(fieldContent.find('div.supplier-name'));
      utilityPrice = parseRates(utilityPrice, fieldContent.find('div.supplier-rate'));
      utilityPrice = parseCopy(utilityPrice, fieldContent.find('div.copy'));

      offers.push(utilityPrice);
    });

  return offers;
}

export default function loadOffers(stateUtilityIndex) {
  const url = generateUrlForIndex(stateUtilityIndex);

  return requestPromise(url).then(html => {
    const offers = parseHtml(html);

    // eslint-disable-next-line no-console
    console.log(`Haha. dicks, I got ${offers.length} offers here`);
  });
}
