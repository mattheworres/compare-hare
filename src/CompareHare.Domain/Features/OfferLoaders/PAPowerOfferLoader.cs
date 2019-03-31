using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Io;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.OfferLoaders.Interfaces;
using CompareHare.Domain.Services;
using CompareHare.Domain.Services.Interfaces;
using Serilog;

namespace CompareHare.Domain.Features.OfferLoaders
{
    public class PAPowerOfferLoader : IOfferLoader
    {
        //TODO: Remove once live.....
        private const string URL = "http://localhost:8000/public/PA_Response.html";
        //private const string URL = "https://www.papowerswitch.com/shop-for-electricity/shop-for-your-home?zipcode={0}";
        private const string SPACE = " ";
        private const string NO_ANSWER = "No";
        private const string NO_TERM_LENGTH = "No term length";
        private const string YES_ANSWER = "Yes";
        private const string FIXED_PRICE_STRUCTURE_CLASS = "Fixed";
        private const string VARIABLE_PRICE_STRUCTURE_CLASS = "Variable";
        private const string UNLIMITED_PRICE_STRUCTURE_CLASS = "Unlimited";
        private readonly IParserWrapper _parserWrapper;
        private readonly ParserHelper _parserHelper;

        public PAPowerOfferLoader(IParserWrapper parserWrapper, ParserHelper parserHelper) {
            _parserWrapper = parserWrapper;
            _parserHelper = parserHelper;
        }

        public async Task<List<UtilityPrice>> LoadOffers(int utilityIndexId, string loaderIdentifier, IRequester requester = null) {
            var url = string.Format(URL, loaderIdentifier);
            var document = await _parserWrapper.OpenUrlAsync(url, requester);

            var offers = new List<UtilityPrice>();

            //var fieldContents = document.QuerySelectorAll("div.views-field-nothing div.shop-inner span.field-content");
            var fieldContents = document.QuerySelectorAll("div.views-field-nothing span.field-content");

            foreach(var fieldContent in fieldContents) {
                try {
                    var utilityPrice = ParseSupplier(fieldContent, utilityIndexId);
                    utilityPrice = ParseRates(fieldContent, utilityPrice);
                    utilityPrice = ParseCopy(fieldContent, utilityPrice);

                    offers.Add(utilityPrice);
                } catch (Exception ex) {
                    Log.Logger.Error(ex, "PAPower Parse Error for {0}", loaderIdentifier);
                }
            }

            return offers;
        }

        private UtilityPrice ParseSupplier(IElement element, int utilityIndexId) {
            var supplierElement = element.QuerySelector("div.supplier-name");
            var uniqueIdentifier = _parserHelper.GetElementClassStartingWith(supplierElement, "nid");

            return new UtilityPrice {
                Id = 0,
                Name = supplierElement.QuerySelector("span.name a").Text().Trim(),
                SupplierPhone = supplierElement.QuerySelector("span.phone").Text().Trim(),
                StateUtilityIndexId = utilityIndexId,
                OfferId = uniqueIdentifier,
            };
        }

        private UtilityPrice ParseRates(IElement element, UtilityPrice utilityPrice) {
            var supplierRateElement = element.QuerySelector("div.supplier-rate");
            var rateText = supplierRateElement.QuerySelector("span.rate").Text();

            utilityPrice.PricePerUnit = float.Parse(rateText.Substring(1, rateText.Length - 1), CultureInfo.InvariantCulture.NumberFormat);

            if (utilityPrice.PricePerUnit == 0) {
                utilityPrice.PricePerUnit = null;
                utilityPrice.FlatRate = supplierRateElement.QuerySelector("span.unlimited-rate").Text();
            } else {
                //var word = supplierRateElement.QuerySelector("span.unit span.word").Text();
                var unit = supplierRateElement.QuerySelector("span.unit span#unit").Text();
                utilityPrice.PriceUnit = $"{unit}";
            }

            return utilityPrice;
        }

        private UtilityPrice ParseCopy(IElement element, UtilityPrice utilityPrice) {
            var copyElement = element.QuerySelector("div.copy");
            var rightCopyElement = copyElement.QuerySelector("div.right");
            var middleCopyElement = copyElement.QuerySelector("div.middle");
            var leftCopyElement = copyElement.QuerySelector("div.left");

            utilityPrice = ParseRightCopy(rightCopyElement, utilityPrice);
            utilityPrice = ParseMiddleCopy(middleCopyElement, utilityPrice);

            return ParseLeftCopy(leftCopyElement, utilityPrice);
        }

        private UtilityPrice ParseRightCopy(IElement element, UtilityPrice utilityPrice) {
            var signUpElement = element.QuerySelector("span.sign-up a");
            if (signUpElement != null) {
                utilityPrice.OfferUrl = signUpElement.GetAttribute("href");
            }

            return utilityPrice;
        }

        private UtilityPrice ParseMiddleCopy(IElement element, UtilityPrice utilityPrice) {
            var cancellationText = element.QuerySelector("span.cancellation strong").Text();
            var cancellationFeeExists = cancellationText != NO_ANSWER;
            if (cancellationFeeExists) {
                utilityPrice.HasCancellationFee = true;
                utilityPrice.CancellationFee = cancellationText;
            }

            var termLengthText = element.QuerySelector("span.term-length strong").Text();

            if (termLengthText != NO_ANSWER && termLengthText != NO_TERM_LENGTH) utilityPrice.TermMonthLength = _parserHelper.ParseFirstIntegerFromString(termLengthText);

            var monthlyFeeText = element.QuerySelector("span.monthly-fee strong").Text();
            var monthlyFeeExists = monthlyFeeText != NO_ANSWER;
            if (monthlyFeeExists) {
                utilityPrice.HasMonthlyFee = true;
                utilityPrice.MonthlyFee = monthlyFeeText;
            }

            var termEndDateText = element.QuerySelector("span.term-end-date strong").Text();
            utilityPrice.HasTermEndDate = termEndDateText != NO_ANSWER;
            if (utilityPrice.HasTermEndDate) utilityPrice.TermEndDate = DateTime.Parse(element.QuerySelector("span.term-end-date strong span").Text());

            var enrollmentFeeText = element.QuerySelector("span.enrollment-fee strong").Text();
            var enrollmentFeeExists = enrollmentFeeText != NO_ANSWER && !string.IsNullOrEmpty(enrollmentFeeText);
            if (enrollmentFeeExists) {
                utilityPrice.HasEnrollmentFee = true;
                utilityPrice.EnrollmentFee = enrollmentFeeText;
            }

            return utilityPrice;
        }

        private UtilityPrice ParseLeftCopy(IElement element, UtilityPrice utilityPrice) {
            var priceStructure = FIXED_PRICE_STRUCTURE_CLASS;
            var priceStructureElement = element.QuerySelector("span.price.price-structure");
            var includesVariableClass = _parserHelper.ElementHasClass(priceStructureElement, VARIABLE_PRICE_STRUCTURE_CLASS);
            var includesUnlimitedClass = _parserHelper.ElementHasClass(priceStructureElement, UNLIMITED_PRICE_STRUCTURE_CLASS);

            if (includesVariableClass) {
                priceStructure = VARIABLE_PRICE_STRUCTURE_CLASS;
            } else if (includesUnlimitedClass) {
                priceStructure = UNLIMITED_PRICE_STRUCTURE_CLASS;
            }

            utilityPrice.PriceStructure = priceStructure;

            utilityPrice.HasBulkDiscounts = element.QuerySelector("span.discounts strong").Text() == YES_ANSWER;
            utilityPrice.IsIntroductoryPrice = element.QuerySelector("span.introductory strong").Text() == YES_ANSWER;

            var renewableText = element.QuerySelector("span.renewable strong").Text();
            var renewableFeeExists = renewableText != NO_ANSWER;

            if (renewableFeeExists) {
                utilityPrice.HasRenewable = true;
                utilityPrice.RenewablePercentage = _parserHelper.ParseFirstFloatFromString(renewableText);
            }

            return utilityPrice;
        }
    }
}
