using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Services.Interfaces
{
    public interface IObjectHasher : IFeatureService
    {
        string HashObject(object obj);
    }
}
