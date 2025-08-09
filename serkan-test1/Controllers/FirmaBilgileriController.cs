using Microsoft.AspNetCore.Mvc;
using serkan_test1.Models;
using serkan_test1.Data;
using System.Linq;

namespace serkan_test1.Controllers
{
    public class FirmaBilgileriController : Controller
    {
        private readonly UygulamaDbContext _db;

        public FirmaBilgileriController(UygulamaDbContext db)
        {
            _db = db;
        }

        public IActionResult Bilgiler()
        {
            var firma = _db.Firmalar.FirstOrDefault();
            ViewBag.TalepMesaj = TempData["TalepMesaj"];
            
            // Son değişiklik taleplerini getir (ana sayfada gösterilecek)
            var sonTalepler = _db.DegisiklikTalepleri
                .OrderByDescending(t => t.TalepTarihi)
                .Take(5)
                .ToList();
            
            ViewBag.SonTalepler = sonTalepler;
            
            return View("Bilgiler", firma);
        }

        // Talep listesi sayfası
        public IActionResult TalepListesi(int sayfa = 1, int sayfaBoyutu = 5)
        {
            var toplamTalep = _db.DegisiklikTalepleri.Count();
            var toplamSayfa = (int)Math.Ceiling((double)toplamTalep / sayfaBoyutu);
            
            var talepler = _db.DegisiklikTalepleri
                .OrderByDescending(t => t.TalepTarihi)
                .Skip((sayfa - 1) * sayfaBoyutu)
                .Take(sayfaBoyutu)
                .ToList();

            ViewBag.Sayfa = sayfa;
            ViewBag.SayfaBoyutu = sayfaBoyutu;
            ViewBag.ToplamSayfa = toplamSayfa;
            ViewBag.ToplamTalep = toplamTalep;

            return View(talepler);
        }

        // Kullanıcı tarafından değiştirilebilen alanlar
        [HttpPost]
        public IActionResult GuncelleFirmaUnvani([FromForm] string firmaUnvani)
        {
            if (string.IsNullOrWhiteSpace(firmaUnvani))
            {
                TempData["TalepMesaj"] = "Firma ünvanı boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.FirmaUnvani = firmaUnvani, true);
        }

        [HttpPost]
        public IActionResult GuncelleSifre([FromForm] string sifre)
        {
            if (string.IsNullOrWhiteSpace(sifre) || sifre.Length < 6)
            {
                TempData["TalepMesaj"] = "Şifre en az 6 karakter olmalıdır.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.Sifre = sifre, true);
        }

        [HttpPost]
        public IActionResult GuncelleIl([FromForm] string il)
        {
            if (string.IsNullOrWhiteSpace(il))
            {
                TempData["TalepMesaj"] = "İl boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.Il = il, true);
        }

        [HttpPost]
        public IActionResult GuncelleIlce([FromForm] string ilce)
        {
            if (string.IsNullOrWhiteSpace(ilce))
            {
                TempData["TalepMesaj"] = "İlçe boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.Ilce = ilce, true);
        }

        [HttpPost]
        public IActionResult GuncelleMahalle([FromForm] string mahalle)
        {
            if (string.IsNullOrWhiteSpace(mahalle))
            {
                TempData["TalepMesaj"] = "Mahalle boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.Mahalle = mahalle, true);
        }

        [HttpPost]
        public IActionResult GuncelleCepTelefonu([FromForm] string cepTelefonu)
        {
            if (string.IsNullOrWhiteSpace(cepTelefonu))
            {
                TempData["TalepMesaj"] = "Cep telefonu boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.CepTelefonu = cepTelefonu, true);
        }

        [HttpPost]
        public IActionResult GuncelleEPosta([FromForm] string ePosta)
        {
            if (string.IsNullOrWhiteSpace(ePosta))
            {
                TempData["TalepMesaj"] = "E-posta boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.EPosta = ePosta, true);
        }

        [HttpPost]
        public IActionResult GuncelleVergiDairesiIl([FromForm] string vergiDairesiIl)
        {
            if (string.IsNullOrWhiteSpace(vergiDairesiIl))
            {
                TempData["TalepMesaj"] = "Vergi dairesi ili boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.VergiDairesiIl = vergiDairesiIl, true);
        }

        [HttpPost]
        public IActionResult GuncelleVergiDairesi([FromForm] string vergiDairesi)
        {
            if (string.IsNullOrWhiteSpace(vergiDairesi))
            {
                TempData["TalepMesaj"] = "Vergi dairesi boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            return GuncelleAlan(f => f.VergiDairesi = vergiDairesi, true);
        }

        private IActionResult GuncelleAlan(Action<FirmaBilgileriViewModel> guncelle, bool kosul)
        {
            var firma = _db.Firmalar.FirstOrDefault();
            if (firma != null && kosul)
            {
                guncelle(firma);
                _db.SaveChanges();
                TempData["TalepMesaj"] = "Bilgi başarıyla güncellendi.";
            }
            return RedirectToAction("Bilgiler");
        }

        // Admin onayı gerektiren alanlar için değişiklik talepleri
        [HttpPost]
        public IActionResult TalepFirmaAdi([FromForm] string yeniFirmaAdi, [FromForm] string degisiklikSebebi)
        {
            if (string.IsNullOrWhiteSpace(yeniFirmaAdi))
            {
                TempData["TalepMesaj"] = "Firma adı boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            if (string.IsNullOrWhiteSpace(degisiklikSebebi))
            {
                TempData["TalepMesaj"] = "Değişiklik sebebi zorunludur.";
                return RedirectToAction("Bilgiler");
            }
            return TalepEkle("Firma Adı", yeniFirmaAdi, "Firma Adı değişiklik talebiniz alınmıştır.", degisiklikSebebi);
        }

        [HttpPost]
        public IActionResult TalepDomain([FromForm] string yeniDomain, [FromForm] string degisiklikSebebi)
        {
            if (string.IsNullOrWhiteSpace(yeniDomain))
            {
                TempData["TalepMesaj"] = "Domain boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            if (string.IsNullOrWhiteSpace(degisiklikSebebi))
            {
                TempData["TalepMesaj"] = "Değişiklik sebebi zorunludur.";
                return RedirectToAction("Bilgiler");
            }
            return TalepEkle("Domain", yeniDomain, "Domain değişiklik talebiniz alınmıştır.", degisiklikSebebi);
        }

        [HttpPost]
        public IActionResult TalepYetkiliAdi([FromForm] string yeniYetkiliAdi, [FromForm] string degisiklikSebebi)
        {
            if (string.IsNullOrWhiteSpace(yeniYetkiliAdi))
            {
                TempData["TalepMesaj"] = "Yetkili adı boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            if (string.IsNullOrWhiteSpace(degisiklikSebebi))
            {
                TempData["TalepMesaj"] = "Değişiklik sebebi zorunludur.";
                return RedirectToAction("Bilgiler");
            }
            return TalepEkle("Yetkili Adı", yeniYetkiliAdi, "Yetkili adı değişiklik talebiniz alınmıştır.", degisiklikSebebi);
        }

        [HttpPost]
        public IActionResult TalepYetkiliSoyadi([FromForm] string yeniYetkiliSoyadi, [FromForm] string degisiklikSebebi)
        {
            if (string.IsNullOrWhiteSpace(yeniYetkiliSoyadi))
            {
                TempData["TalepMesaj"] = "Yetkili soyadı boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            if (string.IsNullOrWhiteSpace(degisiklikSebebi))
            {
                TempData["TalepMesaj"] = "Değişiklik sebebi zorunludur.";
                return RedirectToAction("Bilgiler");
            }
            return TalepEkle("Yetkili Soyadı", yeniYetkiliSoyadi, "Yetkili soyadı değişiklik talebiniz alınmıştır.", degisiklikSebebi);
        }

        [HttpPost]
        public IActionResult TalepYetkiliTCNo([FromForm] string yeniYetkiliTCNo, [FromForm] string degisiklikSebebi)
        {
            if (string.IsNullOrWhiteSpace(yeniYetkiliTCNo))
            {
                TempData["TalepMesaj"] = "TC No boş olamaz.";
                return RedirectToAction("Bilgiler");
            }
            if (yeniYetkiliTCNo.Length != 11)
            {
                TempData["TalepMesaj"] = "TC No 11 haneli olmalıdır.";
                return RedirectToAction("Bilgiler");
            }
            if (string.IsNullOrWhiteSpace(degisiklikSebebi))
            {
                TempData["TalepMesaj"] = "Değişiklik sebebi zorunludur.";
                return RedirectToAction("Bilgiler");
            }
            return TalepEkle("Yetkili TC No", yeniYetkiliTCNo, "TC kimlik no değişiklik talebiniz alınmıştır.", degisiklikSebebi);
        }

        private IActionResult TalepEkle(string alanAdi, string yeniDeger, string mesaj, string degisiklikSebebi = null)
        {
            var talep = new DegisiklikTalebi
            {
                KullaniciAdi = "testUser", // Gerçek sistemde: User.Identity.Name
                FirmaAdi = "A Emlak", // Gerçek sistemde: mevcut firma adı
                AlanAdi = alanAdi,
                EskiDeger = "Mevcut değer", // Gerçek sistemde: mevcut değer
                YeniDeger = yeniDeger,
                DegisiklikSebebi = degisiklikSebebi ?? "Kullanıcı talebi",
                TalepTarihi = DateTime.Now,
                Durum = TalepDurumu.KontrolEdiliyor
            };

            _db.DegisiklikTalepleri.Add(talep);
            _db.SaveChanges();

            TempData["TalepMesaj"] = mesaj;
            return RedirectToAction("Bilgiler");
        }
    }
}
