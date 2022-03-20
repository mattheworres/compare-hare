using System.ComponentModel.DataAnnotations;

namespace CompareHare.Domain.Entities.Constants
{
    public enum ProductRetailer
    {
        [Display(Name = "Lowes")]
        Lowes = 1,

        [Display(Name = "Home Depot")]
        HomeDepot = 2,

        [Display(Name = "Best Buy")]
        BestBuy = 3,

        [Display(Name = "Appliances Connection")]
        AppliancesConnection = 4,

        [Display(Name = "Menards")]
        Menards = 5,

        [Display(Name = "Newegg")]
        Newegg = 6,

        [Display(Name = "Amazon")]
        Amazon = 7,

        [Display(Name = "Wayfair")]
        Wayfair = 8,

        [Display(Name = "eBay")]
        Ebay = 9,

        [Display(Name = "Alibaba")]
        Alibaba = 10,

        [Display(Name = "Monoprice")]
        Monoprice = 11,

        [Display(Name = "B&H Photo")]
        BhPhoto = 12,

        [Display(Name = "Walmart")]
        Walmart = 13,

        [Display(Name = "Costco")]
        Costco = 14,

        [Display(Name = "Sam's Club")]
        SamsClub = 15,
    }
}
