using System;
using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    public class DegisiklikTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required]
        public string AlanAdi { get; set; } = string.Empty;

        [Required]
        public string YeniDeger { get; set; } = string.Empty;

        public DateTime TalepTarihi { get; set; }
    }
}
