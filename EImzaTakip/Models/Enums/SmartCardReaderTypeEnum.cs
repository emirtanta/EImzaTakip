using System.ComponentModel.DataAnnotations;

namespace EImzaTakip.Models.Enums
{
    public enum SmartCardReaderTypeEnum
    {
        [Display(Name = "Mini okuyucu")]
        MiniOkuyucu = 1,
        [Display(Name = "Masaüstü Okuyuc")]
        MasautuOkuyucu = 2,
        [Display(Name = "İstenmiyor")]
        Istenmiyor = 3,
    }
}
