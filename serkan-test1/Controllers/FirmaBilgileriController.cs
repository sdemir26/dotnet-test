using Microsoft.AspNetCore.Mvc;
using serkan_test1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace serkan_test1.Controllers
{
    /// <summary>
    /// Firma bilgileri ve değişiklik talepleri yönetimi için controller
    /// </summary>
    public class FirmaBilgileriController : Controller
    {
        // Geçici olarak statik veri kullanıyoruz - gerçek uygulamada veritabanı kullanılacak
        private static List<DegisiklikTalebi> _degisiklikTalepleri = new List<DegisiklikTalebi>();
        private static int _talepIdCounter = 1;
        
        // Firma bilgileri için static veri
        private static FirmaBilgileriViewModel _firmaBilgileri = new FirmaBilgileriViewModel
        {
            FirmaAdi = "Test Firma A.Ş.",
            FirmaUnvani = "Test Firma Anonim Şirketi",
            FirmaTipi = "Anonim Şirket",
            Domain = "testfirma.com",
            YetkiliAdi = "Ahmet",
            YetkiliSoyadi = "Yılmaz",
            YetkiliTCNo = "12345678901",
            Sifre = "123456",
            CepTelefonu = "0555 123 45 67",
            EPosta = "info@testfirma.com",
            Ulke = "Türkiye",
            Il = "İstanbul",
            Ilce = "Kadıköy",
            Mahalle = "Fenerbahçe",
            VergiDairesiIl = "İstanbul",
            VergiDairesi = "Kadıköy",
            VergiNo = "1234567890"
        };

        /// <summary>
        /// Ana firma bilgileri sayfasını gösterir
        /// </summary>
        /// <returns>Firma bilgileri view'ı</returns>
        public IActionResult Bilgiler()
        {
            // Son 5 değişiklik talebini view'a gönder
            ViewBag.SonTalepler = _degisiklikTalepleri.OrderByDescending(t => t.TalepTarihi).Take(5);
            return View(_firmaBilgileri);
        }

        /// <summary>
        /// Son değişiklik taleplerini JSON formatında döndürür (AJAX için)
        /// </summary>
        /// <returns>Son 5 değişiklik talebi</returns>
        [HttpGet]
        public IActionResult SonTalepleri()
        {
            var sonTalepler = _degisiklikTalepleri
                .OrderByDescending(t => t.TalepTarihi)
                .Take(5)
                .Select(t => new
                {
                    t.Id,
                    t.AlanAdi,
                    t.EskiDeger,
                    t.YeniDeger,
                    t.TalepTarihi,
                    t.Durum,
                    t.BelgeVarMi
                });
            
            return Json(new { success = true, talepler = sonTalepler });
        }

        /// <summary>
        /// Belge gerektirmeyen alanları direkt günceller
        /// </summary>
        /// <param name="alanAdi">Güncellenecek alanın adı</param>
        /// <param name="yeniDeger">Alanın yeni değeri</param>
        /// <returns>JSON sonucu</returns>
        [HttpPost]
        public IActionResult AlanGuncelle(string alanAdi, string yeniDeger)
        {
            try
            {
                // Firma bilgilerini güncelle
                GuncelleFirmaBilgileri(alanAdi, yeniDeger);
                
                return Json(new { success = true, message = $"{alanAdi} başarıyla güncellendi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Güncelleme sırasında hata oluştu: {ex.Message}" });
            }
        }

        /// <summary>
        /// Firma bilgilerini günceller (admin onayı sonrası veya direkt güncelleme için)
        /// </summary>
        /// <param name="alanAdi">Güncellenecek alanın adı</param>
        /// <param name="yeniDeger">Alanın yeni değeri</param>
        private void GuncelleFirmaBilgileri(string alanAdi, string yeniDeger)
        {
            switch (alanAdi)
            {
                case "Firma Adı":
                    _firmaBilgileri.FirmaAdi = yeniDeger;
                    break;
                case "Firma Ünvanı":
                    _firmaBilgileri.FirmaUnvani = yeniDeger;
                    break;
                case "Firma Tipi":
                    _firmaBilgileri.FirmaTipi = yeniDeger;
                    break;
                case "Domain":
                    _firmaBilgileri.Domain = yeniDeger;
                    break;
                case "Yetkili Adı":
                    _firmaBilgileri.YetkiliAdi = yeniDeger;
                    break;
                case "Yetkili Soyadı":
                    _firmaBilgileri.YetkiliSoyadi = yeniDeger;
                    break;
                case "TC Kimlik Numarası":
                    _firmaBilgileri.YetkiliTCNo = yeniDeger;
                    break;
                case "Şifre":
                    _firmaBilgileri.Sifre = yeniDeger;
                    break;
                case "Cep Telefonu":
                    _firmaBilgileri.CepTelefonu = yeniDeger;
                    break;
                case "E-Posta":
                    _firmaBilgileri.EPosta = yeniDeger;
                    break;
                case "Ülke":
                    _firmaBilgileri.Ulke = yeniDeger;
                    break;
                case "İl":
                    _firmaBilgileri.Il = yeniDeger;
                    break;
                case "İlçe":
                    _firmaBilgileri.Ilce = yeniDeger;
                    break;
                case "Mahalle":
                    _firmaBilgileri.Mahalle = yeniDeger;
                    break;
                case "Vergi Dairesi İl":
                    _firmaBilgileri.VergiDairesiIl = yeniDeger;
                    break;
                case "Vergi Dairesi":
                    _firmaBilgileri.VergiDairesi = yeniDeger;
                    break;
                case "Vergi No":
                    _firmaBilgileri.VergiNo = yeniDeger;
                    break;
                default:
                    throw new ArgumentException($"Bilinmeyen alan: {alanAdi}");
            }
        }

        /// <summary>
        /// Yeni değişiklik talebi oluşturur ve belge yükleme işlemini gerçekleştirir
        /// </summary>
        /// <param name="alanAdi">Değiştirilecek alanın adı</param>
        /// <param name="eskiDeger">Alanın eski değeri</param>
        /// <param name="yeniDeger">Alanın yeni değeri</param>
        /// <param name="degisiklikSebebi">Değişiklik sebebi</param>
        /// <param name="belge">Yüklenen belge dosyası (opsiyonel)</param>
        /// <returns>JSON sonucu</returns>
        [HttpPost]
        public IActionResult TalepOlustur(string alanAdi, string eskiDeger, string yeniDeger, string degisiklikSebebi, IFormFile? belge)
        {
            try
            {
                string? belgeDosyaAdi = null;
                
                // Belge yükleme işlemi - eğer belge varsa
                if (belge != null && belge.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "belgeler");
                    
                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    // Benzersiz dosya adı oluştur (tarih + GUID + uzantı)
                    var dosyaUzantisi = Path.GetExtension(belge.FileName);
                    var benzersizAd = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}{dosyaUzantisi}";
                    var dosyaYolu = Path.Combine(uploadsFolder, benzersizAd);
                    
                    // Dosyayı fiziksel olarak kaydet
                    using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                    {
                        belge.CopyTo(stream);
                    }
                    
                    belgeDosyaAdi = benzersizAd;
                }

                // Yeni değişiklik talebi nesnesi oluştur
                var yeniTalep = new DegisiklikTalebi
                {
                    Id = _talepIdCounter++,
                    FirmaAdi = "Test Firma A.Ş.",
                    KullaniciAdi = "testuser",
                    AlanAdi = alanAdi,
                    EskiDeger = eskiDeger ?? "-",
                    YeniDeger = yeniDeger,
                    DegisiklikSebebi = degisiklikSebebi ?? "Kullanıcı tarafından talep edildi",
                    TalepTarihi = DateTime.Now,
                    Durum = TalepDurumu.KontrolEdiliyor,
                    BelgeDosyaAdi = belgeDosyaAdi,
                    BelgeYuklemeTarihi = belgeDosyaAdi != null ? DateTime.Now : null
                };

                // Talebi listeye ekle
                _degisiklikTalepleri.Add(yeniTalep);

                return Json(new { success = true, message = "Değişiklik talebi başarıyla oluşturuldu", talep = yeniTalep });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcı tarafından görüntülenecek değişiklik talepleri listesi
        /// </summary>
        /// <returns>Talep listesi view'ı</returns>
        public IActionResult TalepListesi()
        {
            return View(_degisiklikTalepleri.OrderByDescending(t => t.TalepTarihi));
        }

        /// <summary>
        /// Admin tarafından görüntülenecek değişiklik talepleri listesi
        /// </summary>
        /// <returns>Admin talep listesi view'ı</returns>
        public IActionResult AdminTalepListesi()
        {
            return View(_degisiklikTalepleri.OrderByDescending(t => t.TalepTarihi));
        }

        /// <summary>
        /// Belirli bir değişiklik talebinin detaylarını gösterir
        /// </summary>
        /// <param name="id">Talep ID'si</param>
        /// <returns>Talep detay view'ı</returns>
        public IActionResult AdminTalepDetay(int id)
        {
            var talep = _degisiklikTalepleri.FirstOrDefault(t => t.Id == id);
            if (talep == null)
            {
                return NotFound();
            }
            return View(talep);
        }

        /// <summary>
        /// Admin tarafından değişiklik talebini onaylar
        /// </summary>
        /// <param name="talepId">Onaylanacak talep ID'si</param>
        /// <param name="adminNotu">Admin notu</param>
        /// <returns>Liste sayfasına yönlendirme</returns>
        [HttpPost]
        public IActionResult AdminOnayla(int talepId, string adminNotu)
        {
            var talep = _degisiklikTalepleri.FirstOrDefault(t => t.Id == talepId);
            if (talep != null)
            {
                talep.Durum = TalepDurumu.Onaylandi;
                talep.AdminNotu = adminNotu;
                talep.IslemTarihi = DateTime.Now;
                
                // Gerçek firma bilgilerini güncelle
                GuncelleFirmaBilgileri(talep.AlanAdi, talep.YeniDeger);
                
                // Başarı mesajı ekle
                TempData["SuccessMessage"] = "Talep başarıyla onaylandı ve firma bilgileri güncellendi!";
                return RedirectToAction("AdminTalepListesi");
            }
            
            // Hata mesajı ekle
            TempData["ErrorMessage"] = "Talep bulunamadı!";
            return RedirectToAction("AdminTalepListesi");
        }

        /// <summary>
        /// Admin tarafından değişiklik talebini reddeder
        /// </summary>
        /// <param name="talepId">Reddedilecek talep ID'si</param>
        /// <param name="adminNotu">Admin notu</param>
        /// <returns>Liste sayfasına yönlendirme</returns>
        [HttpPost]
        public IActionResult AdminReddet(int talepId, string adminNotu)
        {
            var talep = _degisiklikTalepleri.FirstOrDefault(t => t.Id == talepId);
            if (talep != null)
            {
                talep.Durum = TalepDurumu.Reddedildi;
                talep.AdminNotu = adminNotu;
                talep.IslemTarihi = DateTime.Now;
                
                // Başarı mesajı ekle
                TempData["SuccessMessage"] = "Talep başarıyla reddedildi!";
                return RedirectToAction("AdminTalepListesi");
            }
            
            // Hata mesajı ekle
            TempData["ErrorMessage"] = "Talep bulunamadı!";
            return RedirectToAction("AdminTalepListesi");
        }

        /// <summary>
        /// Yüklenen belgeyi indirme için sunar
        /// </summary>
        /// <param name="talepId">Talep ID'si</param>
        /// <returns>Dosya indirme response'u</returns>
        public IActionResult BelgeIndir(int talepId)
        {
            var talep = _degisiklikTalepleri.FirstOrDefault(t => t.Id == talepId);
            if (talep == null || string.IsNullOrEmpty(talep.BelgeDosyaAdi))
            {
                return NotFound("Belge bulunamadı");
            }

            var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "belgeler", talep.BelgeDosyaAdi);
            
            if (!System.IO.File.Exists(dosyaYolu))
            {
                return NotFound("Dosya bulunamadı");
            }

            var dosyaBytes = System.IO.File.ReadAllBytes(dosyaYolu);
            return File(dosyaBytes, "application/octet-stream", talep.BelgeDosyaAdi);
        }

        /// <summary>
        /// Yüklenen belgeyi tarayıcıda görüntülemek için sunar
        /// </summary>
        /// <param name="talepId">Talep ID'si</param>
        /// <returns>Dosya görüntüleme response'u</returns>
        public IActionResult BelgeGoruntule(int talepId)
        {
            var talep = _degisiklikTalepleri.FirstOrDefault(t => t.Id == talepId);
            if (talep == null || string.IsNullOrEmpty(talep.BelgeDosyaAdi))
            {
                return NotFound("Belge bulunamadı");
            }

            var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "belgeler", talep.BelgeDosyaAdi);
            
            if (!System.IO.File.Exists(dosyaYolu))
            {
                return NotFound("Dosya bulunamadı");
            }

            var dosyaBytes = System.IO.File.ReadAllBytes(dosyaYolu);
            var dosyaUzantisi = Path.GetExtension(talep.BelgeDosyaAdi).ToLower();
            
            // Dosya türüne göre content type belirle (tarayıcıda görüntüleme için)
            string contentType = dosyaUzantisi switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".txt" => "text/plain",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };

            return File(dosyaBytes, contentType);
        }
    }
}
