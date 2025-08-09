using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    public class FirmaBilgileriViewModel
    {
        [Key]
        public int Id { get; set; }
        public string FirmaAdi { get; set; } = "";
        public string FirmaUnvani { get; set; } = "";
        public string FirmaTipi { get; set; } = "";
        public string Domain { get; set; } = "";
        public string YetkiliAdi { get; set; } = "";
        public string YetkiliSoyadi { get; set; } = "";
        public string YetkiliTCNo { get; set; } = "";
        public string Sifre { get; set; } = "";
        public string Ulke { get; set; } = "";
        public string Il { get; set; } = "";
        public string Ilce { get; set; } = "";
        public string Mahalle { get; set; } = "";
        public string CepTelefonu { get; set; } = "";
        public string EPosta { get; set; } = "";
        public string VergiDairesiIl { get; set; } = "";
        public string VergiDairesi { get; set; } = "";
        public string VergiNo { get; set; } = "";
    }
}
