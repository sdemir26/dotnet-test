using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Kademeli fiyatlandırma tablosunu tutan model
    /// </summary>
    public class Fiyatlandirma
    {
        public int Id { get; set; }
        
        [Display(Name = "Kademe Adı")]
        [Required(ErrorMessage = "Kademe adı zorunludur")]
        public string KademeAdi { get; set; } = string.Empty;
        
        [Display(Name = "Minimum İlan Sayısı")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum ilan sayısı 0'dan büyük olmalıdır")]
        public int MinIlanSayisi { get; set; }
        
        [Display(Name = "Maksimum İlan Sayısı")]
        [Range(0, int.MaxValue, ErrorMessage = "Maksimum ilan sayısı 0'dan büyük olmalıdır")]
        public int MaxIlanSayisi { get; set; }
        
        [Display(Name = "İskonto Oranı (%)")]
        [Range(0, 100, ErrorMessage = "İskonto oranı 0-100 arasında olmalıdır")]
        public decimal IskontoOrani { get; set; }
        
        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }
        
        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
        
        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        
        [Display(Name = "Güncelleme Tarihi")]
        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;
    }
    
    /// <summary>
    /// Fiyatlandırma hesaplama sonuçlarını tutan model
    /// </summary>
    public class FiyatlandirmaHesaplama
    {
        public int FirmaId { get; set; }
        public string FirmaAdi { get; set; } = string.Empty;
        public int IlanSayisi { get; set; }
        public decimal SabitUcret { get; set; } = 250.00m;
        public decimal BirimFiyat { get; set; } = 50.00m;
        public int UcretsizIlanSayisi { get; set; } = 5;
        public int UcretliIlanSayisi { get; set; }
        public decimal UcretliIlanTutari { get; set; }
        public decimal IskontoOrani { get; set; }
        public decimal IskontoTutari { get; set; }
        public decimal NetUcretliIlanTutari { get; set; }
        public decimal ToplamTutar { get; set; }
        public string KademeAdi { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Sistem ayarlarını tutan model
    /// </summary>
    public class SistemAyarlari
    {
        public int Id { get; set; }
        
        [Display(Name = "Aylık Sabit Ücret")]
        [Range(0, double.MaxValue, ErrorMessage = "Sabit ücret 0'dan büyük olmalıdır")]
        public decimal AylikSabitUcret { get; set; } = 250.00m;
        
        [Display(Name = "Birim İlan Fiyatı")]
        [Range(0, double.MaxValue, ErrorMessage = "Birim fiyat 0'dan büyük olmalıdır")]
        public decimal BirimIlanFiyati { get; set; } = 50.00m;
        
        [Display(Name = "Ücretsiz İlan Sayısı")]
        [Range(0, int.MaxValue, ErrorMessage = "Ücretsiz ilan sayısı 0'dan büyük olmalıdır")]
        public int UcretsizIlanSayisi { get; set; } = 5;
        
        // Gecikme faizi kaldırıldı - sözleşme tarihinden itibaren ay ay fatura kesilecek
        
        [Display(Name = "Güncelleme Tarihi")]
        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;
    }
}
