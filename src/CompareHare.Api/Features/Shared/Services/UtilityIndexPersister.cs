using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Shared.Services
{
    public class UtilityIndexPersister : IUtilityIndexPersister
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IMapper _mapper;

        public UtilityIndexPersister(CompareHareDbContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Given the model, determine if the matching SUI exists.
        /// Return value helpful to determine if offer loaders must be run (false)
        /// </summary>
        /// <param name="model">The SUI-agnostic validated view model from a Create Alert frontend process</param>
        /// <returns>True if it already existed, false if it required persistence or updating.</returns>
        public async Task<bool> DiscoverOrPersistIndex(CreateAlertModel model)
        {
            var mappedIndex = _mapper.Map<StateUtilityIndex>(model);

            if (mappedIndex.UtilityState == UtilityStates.Pennsylvania
                && mappedIndex.UtilityType == UtilityTypes.Power) {
                //First check and see if one matches that is active
                var activeStateUtilityIndexExists = await _dbContext.StateUtilityIndices
                    .AnyAsync(x => x.UtilityState == mappedIndex.UtilityState
                        && x.UtilityType == mappedIndex.UtilityType
                        && x.LoaderDataIdentifier == model.Zip
                        && x.Active == true);

                //Already exists & active, lets get outta this joint
                if (activeStateUtilityIndexExists) return true;

                //Now, see if one exists but is inactive
                var stateUtility = await _dbContext.StateUtilityIndices
                    .FirstOrDefaultAsync(x => x.UtilityState == mappedIndex.UtilityState
                        && x.UtilityType == mappedIndex.UtilityType
                        && x.LoaderDataIdentifier == model.Zip);

                //If it exists, we gotta update it then just return false
                if (stateUtility != null) {
                    stateUtility.Active = true;
                    //Wiping this out lets future processes (PopulateHandler) know to load new offers
                    stateUtility.LastUpdatedHash = string.Empty;
                    await _dbContext.SaveChangesAsync();

                    return false;
                }

                mappedIndex.LoaderDataIdentifier = model.Zip;
                mappedIndex.Active = true;

                await _dbContext.StateUtilityIndices.AddAsync(mappedIndex);
                await _dbContext.SaveChangesAsync();

                return false;
            }

            throw new NotImplementedException("No matching state/type support yet, sorry");
        }

        /// <summary>
        /// Given the model, fetch and return the matching SUI ID
        /// </summary>
        /// <param name="model">The SUI-agnostic validated view model from a Create Alert frontend process</param>
        /// <returns>The matching SUI ID, which should've been created by this service previously
        ///     already if it didnt already exist</returns>
        public async Task<int> FetchIndexId(CreateAlertModel model)
        {
            var mappedIndex = _mapper.Map<StateUtilityIndex>(model);

            if (mappedIndex.UtilityState == UtilityStates.Pennsylvania
                && mappedIndex.UtilityType == UtilityTypes.Power)
            {
                return await _dbContext.StateUtilityIndices
                    .Where(x => x.UtilityState == mappedIndex.UtilityState
                        && x.UtilityType == mappedIndex.UtilityType
                        && x.LoaderDataIdentifier == model.Zip)
                    .Select(x => x.Id)
                    .SingleAsync();
            }

            throw new NotImplementedException("No matching state/type support yet, sorry");
        }
    }
}
