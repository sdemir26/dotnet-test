using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Dinamik ilan satışları için model (paketsiz sistem)
    /// </summary>
    public class DinamikIlanSatis
    {
        public int Id { get; set; }
        
        public int FirmaId { get; set; }
        
        [Display(Name = "Satış Tarihi")]
        public DateTime SatisTarihi { get; set; }
        
        [Display(Name = "İstenen İlan Sayısı")]
        public int IstenenIlanSayisi { get; set; }
        
        [Display(Name = "Mevcut Toplam İlan")]
        public int MevcutToplamIlan { get; set; }
        
        [Display(Name = "Yeni Toplam İlan")]
        public int YeniToplamIlan { get; set; }
        
        [Display(Name = "Birim Fiyat")]
        public decimal BirimFiyat { get; set; } = 50.00m;
        
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
    }
}
