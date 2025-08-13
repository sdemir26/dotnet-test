using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    /// <summary>
    /// Kullanıcının favori klasörlerini temsil eden model
    /// </summary>
    public class FavoriKlasor
    {
        /// <summary>
        /// Klasörün benzersiz kimlik numarası
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Klasörün görünen adı
        /// </summary>
        public string KlasorAdi { get; set; } = string.Empty;

        /// <summary>
        /// Klasörde bulunan favori sayısı
        /// </summary>
        public int FavoriSayisi { get; set; }

        /// <summary>
        /// Klasörün görsel rengi (hex formatında)
        /// </summary>
        public string RenkKodu { get; set; } = "#007bff";

        /// <summary>
        /// Klasörün Bootstrap ikonu sınıf adı
        /// </summary>
        public string IkonAdi { get; set; } = "bi-folder-fill";

        /// <summary>
        /// Klasörün oluşturulma tarihi
        /// </summary>
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        /// <summary>
        /// Klasörün son güncellenme tarihi (opsiyonel)
        /// </summary>
        public DateTime? GuncellemeTarihi { get; set; }

        /// <summary>
        /// Klasörün aktif olup olmadığı
        /// </summary>
        public bool Aktif { get; set; } = true;
    }
}

