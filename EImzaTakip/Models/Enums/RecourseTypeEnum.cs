using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Enums
{
    public enum RecourseTypeEnum
    {
        [Display(Name ="İlk Başvuru")]
        İlkBavuru=1,
        [Display(Name ="Kayıp Çalıntı")]
        KayıpCalinti=2,
        [Display(Name ="Yenilenme")]
        Yenilenme=3,
        [Display(Name = "Diğer")]
        Diger = 4

    }
}
