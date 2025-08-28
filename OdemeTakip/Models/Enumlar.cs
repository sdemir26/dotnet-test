using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Ödeme yöntemleri
    /// </summary>
    public enum OdemeYontemi
    {
        [Display(Name = "Banka Havalesi")]
        BankaHavalesi = 1,
        
        [Display(Name = "Kredi Kartı")]
        KrediKarti = 2,
        
        [Display(Name = "Nakit")]
        Nakit = 3,
        
        [Display(Name = "Çek")]
        Cek = 4,
        
        [Display(Name = "Bekliyor")]
        Bekliyor = 5
    }
    
    /// <summary>
    /// Ödeme durumları
    /// </summary>
    public enum OdemeDurumu
    {
        [Display(Name = "Bekliyor")]
        Bekliyor = 1,
        
        [Display(Name = "Ödendi")]
        Odendi = 2,
        
        [Display(Name = "İptal Edildi")]
        IptalEdildi = 3,
        
        [Display(Name = "İade Edildi")]
        IadeEdildi = 4
    }
}
