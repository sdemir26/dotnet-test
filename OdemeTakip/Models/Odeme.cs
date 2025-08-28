using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Ödeme bilgilerini tutan model
    /// </summary>
    public class Odeme
    {
        public int Id { get; set; }
        
        [Required]
        public int FirmaId { get; set; }
        
        [Display(Name = "Ödeme Tarihi")]
        public DateTime OdemeTarihi { get; set; } = DateTime.Now;
        
        [Display(Name = "Ödeme Dönemi")]
        public string OdemeDonemi { get; set; } = string.Empty; // "2024-01", "2024-02" gibi
        

        
        [Display(Name = "Tutar")]
        [Range(0, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
        public decimal Tutar { get; set; }
        
        [Display(Name = "İskonto Oranı")]
        [Range(0, 100, ErrorMessage = "İskonto oranı 0-100 arasında olmalıdır")]
        public decimal IskontoOrani { get; set; } = 0;
        
        [Display(Name = "İskonto Tutarı")]
        public decimal IskontoTutari { get; set; } = 0;
        
        [Display(Name = "Net Tutar")]
        public decimal NetTutar { get; set; }
        
        [Display(Name = "İlan Sayısı")]
        public int IlanSayisi { get; set; } = 0;
        
        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }
        
        [Display(Name = "Fatura No")]
        public string? FaturaNo { get; set; }
        
        [Display(Name = "Makbuz No")]
        public string? MakbuzNo { get; set; }
        
        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        
        [Display(Name = "Güncelleme Tarihi")]
        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;
        
        // Navigation property
        public virtual Firma Firma { get; set; } = null!;
    }
    

}
