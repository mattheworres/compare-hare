using CompareHare.Api.Features.Shared.Models;
using CompareHare.Domain.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompareHare.Api.Features.Shared.Services
{
    public class EnumApiHelper
    {
        public static IEnumerable<SelectListOptionModel> GetEnumSelectListOptions(Type enumType) {
            var models = new List<SelectListOptionModel>();

            foreach(var i in Enum.GetValues(enumType)) {
                var name = EnumDisplayHelper.GetDisplayName(i, enumType);

                models.Add(new SelectListOptionModel(name, (int)i));
            }

            return models;
        }
    }
}
