using System.Collections.Generic;
using AutoMapper;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Services.Interfaces;
using CompareHare.Domain.Services.Models;

namespace CompareHare.Domain.Services
{
    public class UtilityPriceHasherHelper : IUtilityPriceHasherHelper
    {
        private readonly IMapper _mapper;
        private readonly IObjectHasher _objectHasher;

        public UtilityPriceHasherHelper(IMapper mapper, IObjectHasher objectHasher) {
            _mapper = mapper;
            _objectHasher = objectHasher;
        }

        public bool AreOffersDifferent(IEnumerable<UtilityPrice> utilityPrices, string existingHash)
        {
            var modelHash = GetModelHash(utilityPrices);

            return modelHash != existingHash;
        }

        public bool AreOffersDifferent(IEnumerable<UtilityPriceHistory> histories, string existingHash) {
            var modelHash = GetModelHash(histories);

            return modelHash != existingHash;
        }

        public string GetModelHash(IEnumerable<UtilityPrice> utilityPrices)
        {
            var models = _mapper.Map<IEnumerable<UtilityPriceHashModel>>(utilityPrices);

            return _objectHasher.HashObject(models);
        }

        public string GetModelHash(IEnumerable<UtilityPriceHistory> histories) {
            var models = _mapper.Map<IEnumerable<UtilityPriceHashModel>>(histories);

            return _objectHasher.HashObject(models);
        }
    }
}
