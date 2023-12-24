using System.ComponentModel.DataAnnotations;

namespace Entities.Enums
{
    public enum CurrencyType : int
    {
        [Display(Name = "TL")]
        TL = 0,
        [Display(Name = "DOLAR")]
        DOLAR = 1,
        [Display(Name = "EURO")]
        EURO = 2,
        [Display(Name = "GBP")]
        GBP = 3
    }
}
