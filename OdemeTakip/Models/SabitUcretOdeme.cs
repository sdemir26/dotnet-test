using System.ComponentModel.DataAnnotations;

namespace OdemeTakip.Models
{
    /// <summary>
    /// Sabit ücret ödemeleri için model
    /// </summary>
    public class SabitUcretOdeme
    {
        public int Id { get; set; }
        
        public int FirmaId { get; set; }
        
        [Display(Name = "Ödeme Tarihi")]
        public DateTime OdemeTarihi { get; set; }
        
        [Display(Name = "Ödeme Dönemi")]
        public string OdemeDonemi { get; set; } = string.Empty;
        
        [Display(Name = "Ödeme Yöntemi")]
        public OdemeYontemi OdemeYontemi { get; set; }
        
        [Display(Name = "Ödeme Durumu")]
        public OdemeDurumu OdemeDurumu { get; set; }
        
        [Display(Name = "Tutar")]
        public decimal Tutar { get; set; } = 250.00m;
        
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
