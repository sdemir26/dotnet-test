using Microsoft.AspNetCore.Mvc;
using serkan_test1.Models;
using System.Collections.Generic;
using System.Linq;

namespace serkan_test1.Controllers
{
    /// <summary>
    /// Favori klasörleri yönetimi için controller
    /// </summary>
    public class FavorilerController : Controller
    {
        // Geçici olarak statik veri kullanıyoruz - gerçek uygulamada veritabanı kullanılacak
        private static List<FavoriKlasor> _favoriKlasorler = new List<FavoriKlasor>
        {
            new FavoriKlasor { Id = 1, KlasorAdi = "test klasörü", FavoriSayisi = 3, RenkKodu = "#007bff", IkonAdi = "bi-folder-fill" },
            new FavoriKlasor { Id = 2, KlasorAdi = "favoriler 1", FavoriSayisi = 7, RenkKodu = "#28a745", IkonAdi = "bi-star-fill" },
            new FavoriKlasor { Id = 3, KlasorAdi = "test", FavoriSayisi = 0, RenkKodu = "#ffc107", IkonAdi = "bi-folder" },
            new FavoriKlasor { Id = 4, KlasorAdi = "sdfdsfaa", FavoriSayisi = 1, RenkKodu = "#dc3545", IkonAdi = "bi-heart-fill" },
            new FavoriKlasor { Id = 5, KlasorAdi = "emlak 2", FavoriSayisi = 12, RenkKodu = "#6f42c1", IkonAdi = "bi-house-fill" },
            new FavoriKlasor { Id = 6, KlasorAdi = "Favorilerim", FavoriSayisi = 25, RenkKodu = "#fd7e14", IkonAdi = "bi-collection-fill" }
        };

        // Örnek ilan verileri - gerçek uygulamada veritabanından gelecek
        private static Dictionary<int, List<object>> _klasorIlanlari = new Dictionary<int, List<object>>
        {
            { 1, new List<object> { 
                new { Id = 1, Baslik = "Test İlan 1", Fiyat = "100.000 TL", Tarih = "2024-01-15" },
                new { Id = 2, Baslik = "Test İlan 2", Fiyat = "150.000 TL", Tarih = "2024-01-16" },
                new { Id = 3, Baslik = "Test İlan 3", Fiyat = "200.000 TL", Tarih = "2024-01-17" }
            }},
            { 2, new List<object> { 
                new { Id = 4, Baslik = "Favori İlan 1", Fiyat = "300.000 TL", Tarih = "2024-01-10" },
                new { Id = 5, Baslik = "Favori İlan 2", Fiyat = "350.000 TL", Tarih = "2024-01-11" },
                new { Id = 6, Baslik = "Favori İlan 3", Fiyat = "400.000 TL", Tarih = "2024-01-12" },
                new { Id = 7, Baslik = "Favori İlan 4", Fiyat = "450.000 TL", Tarih = "2024-01-13" },
                new { Id = 8, Baslik = "Favori İlan 5", Fiyat = "500.000 TL", Tarih = "2024-01-14" },
                new { Id = 9, Baslik = "Favori İlan 6", Fiyat = "550.000 TL", Tarih = "2024-01-15" },
                new { Id = 10, Baslik = "Favori İlan 7", Fiyat = "600.000 TL", Tarih = "2024-01-16" }
            }},
            { 3, new List<object>() }, // Boş klasör
            { 4, new List<object> { 
                new { Id = 11, Baslik = "Tek İlan", Fiyat = "75.000 TL", Tarih = "2024-01-20" }
            }},
            { 5, new List<object> { 
                new { Id = 12, Baslik = "Emlak İlan 1", Fiyat = "800.000 TL", Tarih = "2024-01-01" },
                new { Id = 13, Baslik = "Emlak İlan 2", Fiyat = "850.000 TL", Tarih = "2024-01-02" },
                new { Id = 14, Baslik = "Emlak İlan 3", Fiyat = "900.000 TL", Tarih = "2024-01-03" },
                new { Id = 15, Baslik = "Emlak İlan 4", Fiyat = "950.000 TL", Tarih = "2024-01-04" },
                new { Id = 16, Baslik = "Emlak İlan 5", Fiyat = "1.000.000 TL", Tarih = "2024-01-05" },
                new { Id = 17, Baslik = "Emlak İlan 6", Fiyat = "1.050.000 TL", Tarih = "2024-01-06" },
                new { Id = 18, Baslik = "Emlak İlan 7", Fiyat = "1.100.000 TL", Tarih = "2024-01-07" },
                new { Id = 19, Baslik = "Emlak İlan 8", Fiyat = "1.150.000 TL", Tarih = "2024-01-08" },
                new { Id = 20, Baslik = "Emlak İlan 9", Fiyat = "1.200.000 TL", Tarih = "2024-01-09" },
                new { Id = 21, Baslik = "Emlak İlan 10", Fiyat = "1.250.000 TL", Tarih = "2024-01-10" },
                new { Id = 22, Baslik = "Emlak İlan 11", Fiyat = "1.300.000 TL", Tarih = "2024-01-11" },
                new { Id = 23, Baslik = "Emlak İlan 12", Fiyat = "1.350.000 TL", Tarih = "2024-01-12" }
            }},
            { 6, new List<object> { 
                new { Id = 24, Baslik = "Favorilerim İlan 1", Fiyat = "500.000 TL", Tarih = "2024-01-01" },
                new { Id = 25, Baslik = "Favorilerim İlan 2", Fiyat = "550.000 TL", Tarih = "2024-01-02" },
                new { Id = 26, Baslik = "Favorilerim İlan 3", Fiyat = "600.000 TL", Tarih = "2024-01-03" },
                new { Id = 27, Baslik = "Favorilerim İlan 4", Fiyat = "650.000 TL", Tarih = "2024-01-04" },
                new { Id = 28, Baslik = "Favorilerim İlan 5", Fiyat = "700.000 TL", Tarih = "2024-01-05" },
                new { Id = 29, Baslik = "Favorilerim İlan 6", Fiyat = "750.000 TL", Tarih = "2024-01-06" },
                new { Id = 30, Baslik = "Favorilerim İlan 7", Fiyat = "800.000 TL", Tarih = "2024-01-07" },
                new { Id = 31, Baslik = "Favorilerim İlan 8", Fiyat = "850.000 TL", Tarih = "2024-01-08" },
                new { Id = 32, Baslik = "Favorilerim İlan 9", Fiyat = "900.000 TL", Tarih = "2024-01-09" },
                new { Id = 33, Baslik = "Favorilerim İlan 10", Fiyat = "950.000 TL", Tarih = "2024-01-10" },
                new { Id = 34, Baslik = "Favorilerim İlan 11", Fiyat = "1.000.000 TL", Tarih = "2024-01-11" },
                new { Id = 35, Baslik = "Favorilerim İlan 12", Fiyat = "1.050.000 TL", Tarih = "2024-01-12" },
                new { Id = 36, Baslik = "Favorilerim İlan 13", Fiyat = "1.100.000 TL", Tarih = "2024-01-13" },
                new { Id = 37, Baslik = "Favorilerim İlan 14", Fiyat = "1.150.000 TL", Tarih = "2024-01-14" },
                new { Id = 38, Baslik = "Favorilerim İlan 15", Fiyat = "1.200.000 TL", Tarih = "2024-01-15" },
                new { Id = 39, Baslik = "Favorilerim İlan 16", Fiyat = "1.250.000 TL", Tarih = "2024-01-16" },
                new { Id = 40, Baslik = "Favorilerim İlan 17", Fiyat = "1.300.000 TL", Tarih = "2024-01-17" },
                new { Id = 41, Baslik = "Favorilerim İlan 18", Fiyat = "1.350.000 TL", Tarih = "2024-01-18" },
                new { Id = 42, Baslik = "Favorilerim İlan 19", Fiyat = "1.400.000 TL", Tarih = "2024-01-19" },
                new { Id = 43, Baslik = "Favorilerim İlan 20", Fiyat = "1.450.000 TL", Tarih = "2024-01-20" },
                new { Id = 44, Baslik = "Favorilerim İlan 21", Fiyat = "1.500.000 TL", Tarih = "2024-01-21" },
                new { Id = 45, Baslik = "Favorilerim İlan 22", Fiyat = "1.550.000 TL", Tarih = "2024-01-22" },
                new { Id = 46, Baslik = "Favorilerim İlan 23", Fiyat = "1.600.000 TL", Tarih = "2024-01-23" },
                new { Id = 47, Baslik = "Favorilerim İlan 24", Fiyat = "1.650.000 TL", Tarih = "2024-01-24" },
                new { Id = 48, Baslik = "Favorilerim İlan 25", Fiyat = "1.700.000 TL", Tarih = "2024-01-25" }
            }}
        };

        /// <summary>
        /// Ana favoriler sayfasını gösterir
        /// </summary>
        /// <returns>Favoriler listesi view'ı</returns>
        public IActionResult Index()
        {
            return View(_favoriKlasorler);
        }

        /// <summary>
        /// Belirli bir klasördeki ilanları getirir
        /// </summary>
        /// <param name="klasorId">Klasör ID'si</param>
        /// <returns>JSON formatında ilan listesi</returns>
        [HttpGet]
        public IActionResult KlasorIlanlari(int klasorId)
        {
            if (_klasorIlanlari.ContainsKey(klasorId))
            {
                return Json(new { success = true, ilanlar = _klasorIlanlari[klasorId] });
            }
            return Json(new { success = false, message = "Klasör bulunamadı" });
        }

        /// <summary>
        /// Test amaçlı basit endpoint - controller'ın çalışıp çalışmadığını kontrol etmek için
        /// </summary>
        /// <returns>Test mesajı</returns>
        public IActionResult Test()
        {
            return Content("Favoriler Controller çalışıyor!");
        }

        /// <summary>
        /// Yeni favori klasörü oluşturur
        /// </summary>
        /// <param name="klasorAdi">Oluşturulacak klasörün adı</param>
        /// <returns>JSON sonucu</returns>
        [HttpPost]
        public IActionResult YeniKlasor(string klasorAdi)
        {
            // Klasör adı validasyonu
            if (string.IsNullOrWhiteSpace(klasorAdi))
            {
                return Json(new { success = false, message = "Klasör adı boş olamaz" });
            }

            // Yeni klasör nesnesi oluştur
            var yeniKlasor = new FavoriKlasor
            {
                Id = _favoriKlasorler.Max(k => k.Id) + 1,
                KlasorAdi = klasorAdi,
                FavoriSayisi = 0, // Yeni klasörde başlangıçta 0 ilan olur
                RenkKodu = GetRandomColor(),
                IkonAdi = GetRandomIcon()
            };

            _favoriKlasorler.Add(yeniKlasor);
            _klasorIlanlari[yeniKlasor.Id] = new List<object>(); // Boş ilan listesi
            
            return Json(new { success = true, klasor = yeniKlasor });
        }

        /// <summary>
        /// Belirtilen favori klasörünü siler
        /// </summary>
        /// <param name="id">Silinecek klasörün ID'si</param>
        /// <returns>JSON sonucu</returns>
        [HttpPost]
        public IActionResult KlasorSil(int id)
        {
            var klasor = _favoriKlasorler.FirstOrDefault(k => k.Id == id);
            if (klasor != null)
            {
                _favoriKlasorler.Remove(klasor);
                _klasorIlanlari.Remove(id); // İlanları da sil
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Klasör bulunamadı" });
        }

        /// <summary>
        /// Belirtilen favori klasörünün adını günceller
        /// </summary>
        /// <param name="id">Güncellenecek klasörün ID'si</param>
        /// <param name="yeniAd">Yeni klasör adı</param>
        /// <returns>JSON sonucu</returns>
        [HttpPost]
        public IActionResult KlasorAdiGuncelle(int id, string yeniAd)
        {
            var klasor = _favoriKlasorler.FirstOrDefault(k => k.Id == id);
            if (klasor != null && !string.IsNullOrWhiteSpace(yeniAd))
            {
                klasor.KlasorAdi = yeniAd;
                klasor.GuncellemeTarihi = DateTime.Now;
                return Json(new { success = true, klasor = klasor });
            }
            return Json(new { success = false, message = "Güncelleme başarısız" });
        }

        /// <summary>
        /// Rastgele renk kodu döndürür
        /// </summary>
        /// <returns>Hex renk kodu</returns>
        private string GetRandomColor()
        {
            var renkler = new[] { "#007bff", "#28a745", "#ffc107", "#dc3545", "#6f42c1", "#fd7e14", "#20c997", "#e83e8c" };
            return renkler[new Random().Next(renkler.Length)];
        }

        /// <summary>
        /// Rastgele Bootstrap ikonu adı döndürür
        /// </summary>
        /// <returns>Bootstrap ikon sınıf adı</returns>
        private string GetRandomIcon()
        {
            var ikonlar = new[] { "bi-folder-fill", "bi-star-fill", "bi-heart-fill", "bi-house-fill", "bi-collection-fill", "bi-bookmark-fill" };
            return ikonlar[new Random().Next(ikonlar.Length)];
        }
    }
}
