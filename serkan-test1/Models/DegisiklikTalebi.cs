using System;
using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    public class DegisiklikTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Firma Adı")]
        public string FirmaAdi { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Alan Adı")]
        public string AlanAdi { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Eski Değer")]
        public string EskiDeger { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Yeni Değer")]
        public string YeniDeger { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Değişiklik Sebebi")]
        public string DegisiklikSebebi { get; set; } = string.Empty;

        [Display(Name = "Talep Tarihi")]
        public DateTime TalepTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Durum")]
        public TalepDurumu Durum { get; set; } = TalepDurumu.KontrolEdiliyor;

        [Display(Name = "Admin Notu")]
        public string? AdminNotu { get; set; }

        [Display(Name = "İşlem Tarihi")]
        public DateTime? IslemTarihi { get; set; }
    }

    public enum TalepDurumu
    {
        [Display(Name = "Kontrol Ediliyor")]
        KontrolEdiliyor = 0,
        
        [Display(Name = "Onaylandı")]
        Onaylandi = 1,
        
        [Display(Name = "Reddedildi")]
        Reddedildi = 2
    }
}
