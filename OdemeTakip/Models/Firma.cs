using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Firma bilgileri ve kontör sistemi için model
    /// </summary>
    public class Firma
    {
        public int Id { get; set; }
        
        [Display(Name = "Firma Adı")]
        public string FirmaAdi { get; set; } = string.Empty;
        
        [Display(Name = "Firma Ünvanı")]
        public string FirmaUnvani { get; set; } = string.Empty;
        
        [Display(Name = "Vergi No")]
        public string VergiNo { get; set; } = string.Empty;
        
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;
        
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Adres")]
        public string Adres { get; set; } = string.Empty;
        
        [Display(Name = "Üyelik Başlangıç Tarihi")]
        public DateTime UyelikBaslangicTarihi { get; set; }
        
        [Display(Name = "Kalan İlan Hakkı")]
        public int KalanIlanHakki { get; set; }
        
        [Display(Name = "Toplam Kullanılan İlan")]
        public int ToplamKullanilanIlan { get; set; }
        
        [Display(Name = "Üyelik Durumu")]
        public UyelikDurumu UyelikDurumu { get; set; }
        
        [Display(Name = "Bu Ay Sabit Ücret")]
        public decimal BuAySabitUcret { get; set; } = 250.00m;
        
        [Display(Name = "Sabit Ücret Borcu")]
        public decimal SabitUcretBorc { get; set; }
        
        [Display(Name = "Son Ödeme Tarihi")]
        public DateTime? SonOdemeTarihi { get; set; }
        
        [Display(Name = "Beklemede Durumuna Düşme Sayısı")]
        public int BeklemedeDurumunaDusmeSayisi { get; set; }
        
        [Display(Name = "Son Beklemede Durumuna Düşme Tarihi")]
        public DateTime? SonBeklemedeDurumunaDusmeTarihi { get; set; }
        
        [Display(Name = "Güncelleme Tarihi")]
        public DateTime GuncellemeTarihi { get; set; }
        
        // Navigation Properties
        public virtual ICollection<Odeme> Odemeler { get; set; } = new List<Odeme>();
        public virtual ICollection<SabitUcretOdeme> SabitUcretOdemeler { get; set; } = new List<SabitUcretOdeme>();
        public virtual ICollection<KontorPaketSatis> KontorPaketSatislar { get; set; } = new List<KontorPaketSatis>();
    }
    
    /// <summary>
    /// Üyelik durumları
    /// </summary>
    public enum UyelikDurumu
    {
        [Display(Name = "Aktif")]
        Aktif = 1,
        
        [Display(Name = "Beklemede")]
        Beklemede = 2,
        
        [Display(Name = "Pasif")]
        Pasif = 3,
        
        [Display(Name = "Askıya Alınmış")]
        AskıyaAlınmış = 4,
        
        [Display(Name = "İptal Edilmiş")]
        İptalEdilmiş = 5
    }
}
