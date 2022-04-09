using System;
using System.Collections.Generic;
using CompareHare.Api.Features.Shared.Models;
using CompareHare.Domain.Helpers;

namespace CompareHare.Api.Features.Shared.Services
{
    public class EnumApiHelper
    {
        public static IEnumerable<SelectListOptionModel> GetEnumSelectListOptions(Type enumType) {
            var models = new List<SelectListOptionModel>();

            foreach(var i in Enum.GetValues(enumType)) {
                var name = EnumDisplayHelper.GetDisplayName(i, enumType);

                models.Add(new SelectListOptionModel() {
                    Value = i,
                    Label = name
                });
            }

            return models;
        }
    }
}