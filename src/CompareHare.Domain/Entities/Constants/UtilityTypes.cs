using System.ComponentModel.DataAnnotations;

namespace CompareHare.Domain.Entities.Constants
{
    public enum UtilityTypes
    {
        [Display(Name = "Power")]
        Power = 1,

        [Display(Name = "Gas")]
        Gas = 2,
    }
}
