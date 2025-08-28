using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Kontör paket satışları için model
    /// </summary>
    public class KontorPaketi
    {
        public int Id { get; set; }
        
        [Display(Name = "Paket Adı")]
        public string PaketAdi { get; set; } = string.Empty;
        
        [Display(Name = "İlan Sayısı")]
        public int IlanSayisi { get; set; }
        
        [Display(Name = "Paket Fiyatı")]
        public decimal PaketFiyati { get; set; }
        
        [Display(Name = "Birim İlan Fiyatı")]
        public decimal BirimIlanFiyati { get; set; }
        
        [Display(Name = "İskonto Oranı (%)")]
        public decimal IskontoOrani { get; set; }
        
        [Display(Name = "Net Fiyat")]
        public decimal NetFiyat { get; set; }
        
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; } = string.Empty;
        
        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
        
        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public virtual ICollection<KontorPaketSatis> Satislar { get; set; } = new List<KontorPaketSatis>();
    }
}
