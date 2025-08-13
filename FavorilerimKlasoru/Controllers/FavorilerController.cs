using Microsoft.AspNetCore.Mvc;
using FavorilerimKlasoru.Models;
using System.Collections.Generic;

namespace FavorilerimKlasoru.Controllers
{
    public class FavorilerController : Controller
    {
        // Geçici olarak statik veri kullanıyoruz
        private static List<FavoriKlasor> _favoriKlasorler = new List<FavoriKlasor>
        {
            new FavoriKlasor { Id = 1, KlasorAdi = "test klasörü", FavoriSayisi = 1, RenkKodu = "#007bff", IkonAdi = "bi-folder-fill" },
            new FavoriKlasor { Id = 2, KlasorAdi = "favoriler 1", FavoriSayisi = 1, RenkKodu = "#28a745", IkonAdi = "bi-star-fill" },
            new FavoriKlasor { Id = 3, KlasorAdi = "test", FavoriSayisi = 0, RenkKodu = "#ffc107", IkonAdi = "bi-folder" },
            new FavoriKlasor { Id = 4, KlasorAdi = "sdfdsfaa", FavoriSayisi = 0, RenkKodu = "#dc3545", IkonAdi = "bi-heart-fill" },
            new FavoriKlasor { Id = 5, KlasorAdi = "emlak 2", FavoriSayisi = 0, RenkKodu = "#6f42c1", IkonAdi = "bi-house-fill" },
            new FavoriKlasor { Id = 6, KlasorAdi = "Favorilerim", FavoriSayisi = 5, RenkKodu = "#fd7e14", IkonAdi = "bi-collection-fill" }
        };

        public IActionResult Index()
        {
            return View(_favoriKlasorler);
        }

        public IActionResult KlasorDetay(int id)
        {
            var klasor = _favoriKlasorler.FirstOrDefault(k => k.Id == id);
            if (klasor == null)
            {
                return NotFound();
            }
            return View(klasor);
        }

        [HttpPost]
        public IActionResult YeniKlasor(string klasorAdi)
        {
            if (string.IsNullOrWhiteSpace(klasorAdi))
            {
                return Json(new { success = false, message = "Klasör adı boş olamaz" });
            }

            var yeniKlasor = new FavoriKlasor
            {
                Id = _favoriKlasorler.Max(k => k.Id) + 1,
                KlasorAdi = klasorAdi,
                FavoriSayisi = 0,
                RenkKodu = GetRandomColor(),
                IkonAdi = GetRandomIcon()
            };

            _favoriKlasorler.Add(yeniKlasor);
            return Json(new { success = true, klasor = yeniKlasor });
        }

        [HttpPost]
        public IActionResult KlasorSil(int id)
        {
            var klasor = _favoriKlasorler.FirstOrDefault(k => k.Id == id);
            if (klasor != null)
            {
                _favoriKlasorler.Remove(klasor);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Klasör bulunamadı" });
        }

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

        private string GetRandomColor()
        {
            var renkler = new[] { "#007bff", "#28a745", "#ffc107", "#dc3545", "#6f42c1", "#fd7e14", "#20c997", "#e83e8c" };
            return renkler[new Random().Next(renkler.Length)];
        }

        private string GetRandomIcon()
        {
            var ikonlar = new[] { "bi-folder-fill", "bi-star-fill", "bi-heart-fill", "bi-house-fill", "bi-collection-fill", "bi-bookmark-fill" };
            return ikonlar[new Random().Next(ikonlar.Length)];
        }
    }
}
