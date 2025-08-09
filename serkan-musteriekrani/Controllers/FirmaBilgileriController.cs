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
            return View("Bilgiler", firma); // ← view adı açıkça belirtildi
        }

        // Kullanıcı tarafından değiştirilebilen alanlar
        [HttpPost]
        public IActionResult GuncelleFirmaUnvani(string firmaUnvani) => GuncelleAlan(f => f.FirmaUnvani = firmaUnvani, !string.IsNullOrWhiteSpace(firmaUnvani));

        [HttpPost]
        public IActionResult GuncelleSifre(string sifre) => GuncelleAlan(f => f.Sifre = sifre, !string.IsNullOrWhiteSpace(sifre));

        [HttpPost]
        public IActionResult GuncelleIl(string il) => GuncelleAlan(f => f.Il = il, !string.IsNullOrWhiteSpace(il));

        [HttpPost]
        public IActionResult GuncelleIlce(string ilce) => GuncelleAlan(f => f.Ilce = ilce, !string.IsNullOrWhiteSpace(ilce));

        [HttpPost]
        public IActionResult GuncelleMahalle(string mahalle) => GuncelleAlan(f => f.Mahalle = mahalle, !string.IsNullOrWhiteSpace(mahalle));

        [HttpPost]
        public IActionResult GuncelleCepTelefonu(string cepTelefonu) => GuncelleAlan(f => f.CepTelefonu = cepTelefonu, !string.IsNullOrWhiteSpace(cepTelefonu));

        [HttpPost]
        public IActionResult GuncelleEPosta(string ePosta) => GuncelleAlan(f => f.EPosta = ePosta, !string.IsNullOrWhiteSpace(ePosta));

        [HttpPost]
        public IActionResult GuncelleVergiDairesiIl(string vergiDairesiIl) => GuncelleAlan(f => f.VergiDairesiIl = vergiDairesiIl, !string.IsNullOrWhiteSpace(vergiDairesiIl));

        [HttpPost]
        public IActionResult GuncelleVergiDairesi(string vergiDairesi) => GuncelleAlan(f => f.VergiDairesi = vergiDairesi, !string.IsNullOrWhiteSpace(vergiDairesi));

        private IActionResult GuncelleAlan(Action<FirmaBilgileriViewModel> guncelle, bool kosul)
        {
            var firma = _db.Firmalar.FirstOrDefault();
            if (firma != null && kosul)
            {
                guncelle(firma);
                _db.SaveChanges();
            }
            return RedirectToAction("Bilgiler");
        }

        // Admin onayı gerektiren alanlar için değişiklik talepleri
        [HttpPost]
        public IActionResult TalepFirmaAdi(string yeniFirmaAdi) => TalepEkle("Firma Adı", yeniFirmaAdi, "Firma Adı değişiklik talebiniz alınmıştır.");

        [HttpPost]
        public IActionResult TalepDomain(string yeniDomain) => TalepEkle("Domain", yeniDomain, "Domain değişiklik talebiniz alınmıştır.");

        [HttpPost]
        public IActionResult TalepYetkiliAdi(string yeniYetkiliAdi) => TalepEkle("Yetkili Adı", yeniYetkiliAdi, "Yetkili adı değişiklik talebiniz alınmıştır.");

        [HttpPost]
        public IActionResult TalepYetkiliSoyadi(string yeniYetkiliSoyadi) => TalepEkle("Yetkili Soyadı", yeniYetkiliSoyadi, "Yetkili soyadı değişiklik talebiniz alınmıştır.");

        [HttpPost]
        public IActionResult TalepYetkiliTCNo(string yeniTCNo) => TalepEkle("Yetkili TC No", yeniTCNo, "TC kimlik no değişiklik talebiniz alınmıştır.");

        private IActionResult TalepEkle(string alanAdi, string yeniDeger, string mesaj)
        {
            if (string.IsNullOrWhiteSpace(yeniDeger))
            {
                TempData["TalepMesaj"] = $"{alanAdi} boş olamaz.";
                return RedirectToAction("Bilgiler");
            }

            var talep = new DegisiklikTalebi
            {
                KullaniciAdi = "testUser", // Gerçek sistemde: User.Identity.Name
                AlanAdi = alanAdi,
                YeniDeger = yeniDeger,
                TalepTarihi = DateTime.Now
            };

            _db.DegisiklikTalepleri.Add(talep);
            _db.SaveChanges();

            TempData["TalepMesaj"] = mesaj;
            return RedirectToAction("Bilgiler");
        }
    }
}