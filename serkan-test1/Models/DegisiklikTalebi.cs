using System;
using System.ComponentModel.DataAnnotations;

namespace serkan_test1.Models
{
    /// <summary>
    /// Firma bilgilerinde yapılan değişiklik taleplerini temsil eden model
    /// </summary>
    public class DegisiklikTalebi
    {
        /// <summary>
        /// Talebin benzersiz kimlik numarası
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Değişiklik talebinde bulunan firmanın adı
        /// </summary>
        [Required(ErrorMessage = "Firma adı gereklidir")]
        public string FirmaAdi { get; set; } = string.Empty;

        /// <summary>
        /// Değişiklik talebini oluşturan kullanıcının adı
        /// </summary>
        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        public string KullaniciAdi { get; set; } = string.Empty;

        /// <summary>
        /// Değiştirilmek istenen alanın adı (örn: "Firma Adı", "Vergi No")
        /// </summary>
        [Required(ErrorMessage = "Alan adı gereklidir")]
        public string AlanAdi { get; set; } = string.Empty;

        /// <summary>
        /// Alanın mevcut (eski) değeri
        /// </summary>
        public string EskiDeger { get; set; } = string.Empty;

        /// <summary>
        /// Alanın yeni değeri
        /// </summary>
        [Required(ErrorMessage = "Yeni değer gereklidir")]
        public string YeniDeger { get; set; } = string.Empty;

        /// <summary>
        /// Değişiklik yapılma sebebi
        /// </summary>
        public string DegisiklikSebebi { get; set; } = string.Empty;

        /// <summary>
        /// Talebin oluşturulma tarihi
        /// </summary>
        public DateTime TalepTarihi { get; set; }

        /// <summary>
        /// Talebin mevcut durumu (Kontrol Ediliyor, Onaylandı, Reddedildi)
        /// </summary>
        public TalepDurumu Durum { get; set; }

        /// <summary>
        /// Admin tarafından eklenen not (opsiyonel)
        /// </summary>
        public string? AdminNotu { get; set; }

        /// <summary>
        /// Admin tarafından işlemin yapıldığı tarih
        /// </summary>
        public DateTime? IslemTarihi { get; set; }

        /// <summary>
        /// Yüklenen belge dosyasının adı (dosya sistemi üzerindeki adı)
        /// </summary>
        public string? BelgeDosyaAdi { get; set; }

        /// <summary>
        /// Belgenin yüklendiği tarih
        /// </summary>
        public DateTime? BelgeYuklemeTarihi { get; set; }

        /// <summary>
        /// Belge yüklenip yüklenmediğini kontrol eden hesaplanmış özellik
        /// </summary>
        public bool BelgeVarMi => !string.IsNullOrEmpty(BelgeDosyaAdi);
    }

    /// <summary>
    /// Değişiklik talebinin durumunu belirten enum
    /// </summary>
    public enum TalepDurumu
    {
        /// <summary>
        /// Talep henüz kontrol edilmedi
        /// </summary>
        KontrolEdiliyor = 0,

        /// <summary>
        /// Talep onaylandı
        /// </summary>
        Onaylandi = 1,

        /// <summary>
        /// Talep reddedildi
        /// </summary>
        Reddedildi = 2
    }
}
