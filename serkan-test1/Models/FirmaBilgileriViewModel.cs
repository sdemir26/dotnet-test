using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    public class FirmaBilgileriViewModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Firma adı zorunludur")]
        [Display(Name = "Firma Adı")]
        public string FirmaAdi { get; set; } = "";
        
        [Required(ErrorMessage = "Firma ünvanı zorunludur")]
        [Display(Name = "Firma Ünvanı")]
        public string FirmaUnvani { get; set; } = "";
        
        [Required(ErrorMessage = "Firma tipi zorunludur")]
        [Display(Name = "Firma Tipi")]
        public string FirmaTipi { get; set; } = "";
        
        [Required(ErrorMessage = "Domain zorunludur")]
        [Display(Name = "Domain")]
        public string Domain { get; set; } = "";
        
        [Required(ErrorMessage = "Yetkili adı zorunludur")]
        [Display(Name = "Yetkili Adı")]
        public string YetkiliAdi { get; set; } = "";
        
        [Required(ErrorMessage = "Yetkili soyadı zorunludur")]
        [Display(Name = "Yetkili Soyadı")]
        public string YetkiliSoyadi { get; set; } = "";
        
        [Required(ErrorMessage = "Yetkili TC No zorunludur")]
        [Display(Name = "Yetkili TC No")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC No 11 haneli olmalıdır")]
        public string YetkiliTCNo { get; set; } = "";
        
        [Required(ErrorMessage = "Şifre zorunludur")]
        [Display(Name = "Şifre")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Sifre { get; set; } = "";
        
        [Required(ErrorMessage = "Ülke zorunludur")]
        [Display(Name = "Ülke")]
        public string Ulke { get; set; } = "";
        
        [Required(ErrorMessage = "İl zorunludur")]
        [Display(Name = "İl")]
        public string Il { get; set; } = "";
        
        [Required(ErrorMessage = "İlçe zorunludur")]
        [Display(Name = "İlçe")]
        public string Ilce { get; set; } = "";
        
        [Required(ErrorMessage = "Mahalle zorunludur")]
        [Display(Name = "Mahalle")]
        public string Mahalle { get; set; } = "";
        
        [Required(ErrorMessage = "Cep telefonu zorunludur")]
        [Display(Name = "Cep Telefonu")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string CepTelefonu { get; set; } = "";
        
        [Required(ErrorMessage = "E-posta zorunludur")]
        [Display(Name = "E-posta")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string EPosta { get; set; } = "";
        
        [Required(ErrorMessage = "Vergi dairesi ili zorunludur")]
        [Display(Name = "Vergi Dairesi İli")]
        public string VergiDairesiIl { get; set; } = "";
        
        [Required(ErrorMessage = "Vergi dairesi zorunludur")]
        [Display(Name = "Vergi Dairesi")]
        public string VergiDairesi { get; set; } = "";
        
        [Required(ErrorMessage = "Vergi numarası zorunludur")]
        [Display(Name = "Vergi No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Vergi numarası 10 haneli olmalıdır")]
        public string VergiNo { get; set; } = "";
    }
}
