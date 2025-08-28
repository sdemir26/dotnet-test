using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Kontör paket satışları için model
    /// </summary>
    public class KontorPaketSatis
    {
        public int Id { get; set; }
        
        public int FirmaId { get; set; }
        
        public int KontorPaketiId { get; set; }
        
        [Display(Name = "Satış Tarihi")]
        public DateTime SatisTarihi { get; set; }
        
        [Display(Name = "İlan Sayısı")]
        public int IlanSayisi { get; set; }
        
        [Display(Name = "Birim Fiyat")]
        public decimal BirimFiyat { get; set; }
        
        [Display(Name = "Toplam Fiyat")]
        public decimal ToplamFiyat { get; set; }
        
        [Display(Name = "İskonto Oranı (%)")]
        public decimal IskontoOrani { get; set; }
        
        [Display(Name = "İskonto Tutarı")]
        public decimal IskontoTutari { get; set; }
        
        [Display(Name = "Net Tutar")]
        public decimal NetTutar { get; set; }
        
        [Display(Name = "Ödeme Yöntemi")]
        public OdemeYontemi OdemeYontemi { get; set; }
        
        [Display(Name = "Ödeme Durumu")]
        public OdemeDurumu OdemeDurumu { get; set; }
        
        [Display(Name = "Fatura No")]
        public string? FaturaNo { get; set; }
        
        [Display(Name = "Makbuz No")]
        public string? MakbuzNo { get; set; }
        
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; } = string.Empty;
        
        // Navigation Properties
        public virtual Firma Firma { get; set; } = null!;
        public virtual KontorPaketi KontorPaketi { get; set; } = null!;
    }
}
