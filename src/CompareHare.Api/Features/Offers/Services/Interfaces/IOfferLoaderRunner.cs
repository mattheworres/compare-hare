using System.Threading.Tasks;

namespace CompareHare.Api.Features.Offers.Services.Interfaces
{
    public interface IOfferLoaderRunner
    {
        Task LoadAllOffers();
    }
}
