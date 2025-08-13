using System.ComponentModel.DataAnnotations;

namespace FavorilerimKlasoru.Models
{
    public class FavoriKlasor
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Klasör adı zorunludur")]
        [StringLength(100, ErrorMessage = "Klasör adı en fazla 100 karakter olabilir")]
        public string KlasorAdi { get; set; } = string.Empty;
        
        public string? Aciklama { get; set; }
        
        public string KullaniciId { get; set; } = string.Empty;
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        
        public DateTime? GuncellemeTarihi { get; set; }
        
        public bool Aktif { get; set; } = true;
        
        public int FavoriSayisi { get; set; } = 0;
        
        // Görsel özellikler
        public string? RenkKodu { get; set; } = "#6c757d"; // Varsayılan gri
        
        public string? IkonAdi { get; set; } = "bi-folder"; // Varsayılan klasör ikonu
    }
}
