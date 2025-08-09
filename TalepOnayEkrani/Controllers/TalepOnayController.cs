using Microsoft.AspNetCore.Mvc;
using TalepOnayEkrani.Models;
using TalepOnayEkrani.Data;
using System.Linq;

namespace TalepOnayEkrani.Controllers
{
    public class TalepOnayController : Controller
    {
        private readonly UygulamaDbContext _db;

        public TalepOnayController(UygulamaDbContext db)
        {
            _db = db;
        }

        // Admin - Talep listesi
        public IActionResult Index(int sayfa = 1, int sayfaBoyutu = 10)
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

        // Admin - Talep detayı
        public IActionResult Detay(int id)
        {
            var talep = _db.DegisiklikTalepleri.Find(id);
            if (talep == null)
            {
                return NotFound();
            }
            return View(talep);
        }

        // Admin - Talebi onayla
        [HttpPost]
        public IActionResult Onayla(int talepId, string adminNotu = null)
        {
            var talep = _db.DegisiklikTalepleri.Find(talepId);
            if (talep != null)
            {
                talep.Durum = TalepDurumu.Onaylandi;
                talep.AdminNotu = adminNotu;
                talep.IslemTarihi = DateTime.Now;
                _db.SaveChanges();
                
                TempData["AdminMesaj"] = "Talep başarıyla onaylandı.";
            }
            return RedirectToAction("Index");
        }

        // Admin - Talebi reddet
        [HttpPost]
        public IActionResult Reddet(int talepId, string adminNotu)
        {
            if (string.IsNullOrWhiteSpace(adminNotu))
            {
                TempData["AdminHata"] = "Red sebebi zorunludur.";
                return RedirectToAction("Detay", new { id = talepId });
            }

            var talep = _db.DegisiklikTalepleri.Find(talepId);
            if (talep != null)
            {
                talep.Durum = TalepDurumu.Reddedildi;
                talep.AdminNotu = adminNotu;
                talep.IslemTarihi = DateTime.Now;
                _db.SaveChanges();
                
                TempData["AdminMesaj"] = "Talep reddedildi.";
            }
            return RedirectToAction("Index");
        }
    }
}
