using Microsoft.AspNetCore.Mvc;
using OdemeTakip.Models;

namespace OdemeTakip.Controllers
{
    public class AdminController : Controller
    {
        // In-memory data
        private static List<Firma> _firmalar = new();
        private static List<KontorPaketi> _kontorPaketleri = new();
        private static List<SabitUcretOdeme> _sabitUcretOdemeler = new();
        private static List<KontorPaketSatis> _kontorPaketSatislar = new();
        private static List<DinamikIlanSatis> _dinamikIlanSatislar = new();

        public AdminController()
        {
            if (!_firmalar.Any())
            {
                OrnekVerileriYukle();
            }
        }

        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel
            {
                ToplamFirmaSayisi = _firmalar.Count,
                PasifFirmaSayisi = _firmalar.Count(f => f.UyelikDurumu == UyelikDurumu.Pasif),
                
                // Toplam gelirler (sabit ücret + kontör paket + dinamik satış)
                BuAyToplamGelir = _sabitUcretOdemeler
                    .Where(o => o.OdemeTarihi.Month == DateTime.Now.Month && o.OdemeTarihi.Year == DateTime.Now.Year && o.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(o => o.Tutar) +
                    _kontorPaketSatislar
                    .Where(s => s.SatisTarihi.Month == DateTime.Now.Month && s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.NetTutar) +
                    _dinamikIlanSatislar
                    .Where(s => s.SatisTarihi.Month == DateTime.Now.Month && s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.NetTutar),
                
                BuYilToplamGelir = _sabitUcretOdemeler
                    .Where(o => o.OdemeTarihi.Year == DateTime.Now.Year && o.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(o => o.Tutar) +
                    _kontorPaketSatislar
                    .Where(s => s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.NetTutar) +
                    _dinamikIlanSatislar
                    .Where(s => s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.NetTutar),
                
                // Ödenmemiş sabit ücret sayısı
                OdenmemisSabitUcretSayisi = _firmalar.Count(f => f.SabitUcretBorc > 0),
                
                // Satın alınan ilan sayıları
                BuAySatilanIlanSayisi = _kontorPaketSatislar
                    .Where(s => s.SatisTarihi.Month == DateTime.Now.Month && s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.IlanSayisi) +
                    _dinamikIlanSatislar
                    .Where(s => s.SatisTarihi.Month == DateTime.Now.Month && s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.IstenenIlanSayisi),
                
                BuYilSatilanIlanSayisi = _kontorPaketSatislar
                    .Where(s => s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.IlanSayisi) +
                    _dinamikIlanSatislar
                    .Where(s => s.SatisTarihi.Year == DateTime.Now.Year && s.OdemeDurumu == OdemeDurumu.Odendi)
                    .Sum(s => s.IstenenIlanSayisi),
                
                // Son ödemeler
                SonOdemeler = _sabitUcretOdemeler
                    .Where(o => o.OdemeDurumu == OdemeDurumu.Odendi)
                    .OrderByDescending(o => o.OdemeTarihi)
                    .Take(5)
                    .ToList()
            };

            return View(viewModel);
        }

        public IActionResult FirmaListesi()
        {
            // Firma üyelik durumlarını güncelle
            foreach (var firma in _firmalar)
            {
                FirmaUyelikDurumunuKontrolEt(firma);
            }
            
            ViewBag.Firmalar = _firmalar;
            return View();
        }

        public IActionResult FirmaDetay(int id)
        {
            var firma = _firmalar.FirstOrDefault(f => f.Id == id);
            if (firma == null) return NotFound();
            
            FirmaUyelikDurumunuKontrolEt(firma);
            
            ViewBag.Firma = firma;
            ViewBag.SabitUcretOdemeler = _sabitUcretOdemeler.Where(o => o.FirmaId == id).OrderByDescending(o => o.OdemeTarihi).ToList();
            ViewBag.KontorPaketSatislar = _kontorPaketSatislar.Where(s => s.FirmaId == id).OrderByDescending(s => s.SatisTarihi).ToList();
            ViewBag.DinamikIlanSatislar = _dinamikIlanSatislar.Where(s => s.FirmaId == id).OrderByDescending(s => s.SatisTarihi).ToList();
            
            return View();
        }

        public IActionResult Odemeler()
        {
            ViewBag.SabitUcretOdemeler = _sabitUcretOdemeler.OrderByDescending(o => o.OdemeTarihi).ToList();
            ViewBag.KontorPaketSatislar = _kontorPaketSatislar.OrderByDescending(s => s.SatisTarihi).ToList();
            ViewBag.DinamikIlanSatislar = _dinamikIlanSatislar.OrderByDescending(s => s.SatisTarihi).ToList();
            return View();
        }

        public IActionResult KontorPaketleri()
        {
            return View(_kontorPaketleri);
        }

        public IActionResult Fiyatlandirma()
        {
            // Kademeli fiyatlandırma tablosu
            var fiyatlandirmalar = new List<object>
            {
                new { Kademe = "Başlangıç", MinIlan = 0, MaxIlan = 5, UstBarem = 5, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 0, Aciklama = "5 ilan dahil" },
                new { Kademe = "Bronz", MinIlan = 6, MaxIlan = 10, UstBarem = 10, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 5, Aciklama = "6-10 ilan arası %5 iskonto" },
                new { Kademe = "Gümüş", MinIlan = 11, MaxIlan = 25, UstBarem = 25, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 10, Aciklama = "11-25 ilan arası %10 iskonto" },
                new { Kademe = "Altın", MinIlan = 26, MaxIlan = 50, UstBarem = 50, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 15, Aciklama = "26-50 ilan arası %15 iskonto" },
                new { Kademe = "Platin", MinIlan = 51, MaxIlan = 100, UstBarem = 100, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 20, Aciklama = "51-100 ilan arası %20 iskonto" },
                new { Kademe = "Elmas", MinIlan = 101, MaxIlan = 250, UstBarem = 250, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 25, Aciklama = "101-250 ilan arası %25 iskonto" },
                new { Kademe = "Kral", MinIlan = 251, MaxIlan = 500, UstBarem = 500, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 30, Aciklama = "251-500 ilan arası %30 iskonto" },
                new { Kademe = "İmparator", MinIlan = 501, MaxIlan = 1000, UstBarem = 1000, BirimFiyat = 50.00m, SabitFiyat = 250.00m, IskontoOrani = 35, Aciklama = "501+ ilan arası %35 iskonto" }
            };

            ViewBag.Fiyatlandirmalar = fiyatlandirmalar;
            ViewBag.SistemAyarlari = new { BirimIlanFiyati = 50.00m, AylikSabitUcret = 250.00m };
            
            return View();
        }

        public IActionResult PasifFirmayiAktifYap(int id)
        {
            var firma = _firmalar.FirstOrDefault(f => f.Id == id);
            if (firma != null)
            {
                firma.UyelikDurumu = UyelikDurumu.Aktif;
                firma.BeklemedeDurumunaDusmeSayisi = 0;
                firma.SonBeklemedeDurumunaDusmeTarihi = null;
                firma.SabitUcretBorc = 0.00m;
                firma.GuncellemeTarihi = DateTime.Now;
                TempData["SuccessMessage"] = $"{firma.FirmaAdi} firması başarıyla aktif hale getirildi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Firma bulunamadı.";
            }
            
            return RedirectToAction("FirmaListesi");
        }

        private void OrnekVerileriYukle()
        {
            var random = new Random();
            
            // Örnek firmalar
            _firmalar = new List<Firma>
            {
                                 new() {
                     Id = 1,
                     FirmaAdi = "ABC Teknoloji A.Ş.",
                     FirmaUnvani = "ABC Teknoloji Anonim Şirketi",
                     VergiNo = "1234567890",
                     Telefon = "0212 555 0101",
                     Email = "info@abcteknoloji.com",
                     Adres = "İstanbul, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-12),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                                 new() {
                     Id = 2,
                     FirmaAdi = "XYZ Gıda Ltd.",
                     FirmaUnvani = "XYZ Gıda Limited Şirketi",
                     VergiNo = "2345678901",
                     Telefon = "0216 555 0202",
                     Email = "info@xyzgida.com",
                     Adres = "İstanbul, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-8),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 3,
                     FirmaAdi = "DEF İnşaat A.Ş.",
                     FirmaUnvani = "DEF İnşaat Anonim Şirketi",
                     VergiNo = "3456789012",
                     Telefon = "0312 555 0303",
                     Email = "info@definsaat.com",
                     Adres = "Ankara, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-15),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 4,
                     FirmaAdi = "GHI Turizm Ltd.",
                     FirmaUnvani = "GHI Turizm Limited Şirketi",
                     VergiNo = "4567890123",
                     Telefon = "0242 555 0404",
                     Email = "info@ghiturizm.com",
                     Adres = "Antalya, Türkiye",
                     UyelikDurumu = UyelikDurumu.Pasif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-6),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 250.00m,
                     BeklemedeDurumunaDusmeSayisi = 1,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 5,
                     FirmaAdi = "JKL Eğitim A.Ş.",
                     FirmaUnvani = "JKL Eğitim Anonim Şirketi",
                     VergiNo = "5678901234",
                     Telefon = "0232 555 0505",
                     Email = "info@jklegitim.com",
                     Adres = "İzmir, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-9),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 6,
                     FirmaAdi = "MNO Sağlık Ltd.",
                     FirmaUnvani = "MNO Sağlık Limited Şirketi",
                     VergiNo = "6789012345",
                     Telefon = "0224 555 0606",
                     Email = "info@mnosaglik.com",
                     Adres = "Bursa, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-4),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 7,
                     FirmaAdi = "PQR Sağlık Kliniği",
                     FirmaUnvani = "PQR Sağlık Kliniği",
                     VergiNo = "7890123456",
                     Telefon = "0258 555 0707",
                     Email = "info@pqrsaglik.com",
                     Adres = "Denizli, Türkiye",
                     UyelikDurumu = UyelikDurumu.Pasif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-5),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 250.00m,
                     BeklemedeDurumunaDusmeSayisi = 1,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 8,
                     FirmaAdi = "STU Spor Kulübü",
                     FirmaUnvani = "STU Spor Kulübü",
                     VergiNo = "8901234567",
                     Telefon = "0332 555 0808",
                     Email = "info@stuspor.com",
                     Adres = "Konya, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-7),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 9,
                     FirmaAdi = "VWX Mobilya A.Ş.",
                     FirmaUnvani = "VWX Mobilya Anonim Şirketi",
                     VergiNo = "9012345678",
                     Telefon = "0352 555 0909",
                     Email = "info@vwxmobilya.com",
                     Adres = "Kayseri, Türkiye",
                     UyelikDurumu = UyelikDurumu.Pasif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-10),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 250.00m,
                     BeklemedeDurumunaDusmeSayisi = 4,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 10,
                     FirmaAdi = "YZA Otomotiv Ltd.",
                     FirmaUnvani = "YZA Otomotiv Limited Şirketi",
                     VergiNo = "0123456789",
                     Telefon = "0342 555 1010",
                     Email = "info@yzaotomotiv.com",
                     Adres = "Gaziantep, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-3),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 11,
                     FirmaAdi = "BCD Tekstil A.Ş.",
                     FirmaUnvani = "BCD Tekstil Anonim Şirketi",
                     VergiNo = "1234567891",
                     Telefon = "0262 555 1111",
                     Email = "info@bcdtekstil.com",
                     Adres = "Kocaeli, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-2),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 12,
                     FirmaAdi = "EFG Emlak Ltd.",
                     FirmaUnvani = "EFG Emlak Limited Şirketi",
                     VergiNo = "2345678902",
                     Telefon = "0324 555 1212",
                     Email = "info@efgemlak.com",
                     Adres = "Mersin, Türkiye",
                     UyelikDurumu = UyelikDurumu.Pasif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-4),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 250.00m,
                     BeklemedeDurumunaDusmeSayisi = 2,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 13,
                     FirmaAdi = "HIJ Hukuk Bürosu",
                     FirmaUnvani = "HIJ Hukuk Bürosu",
                     VergiNo = "3456789013",
                     Telefon = "0362 555 1313",
                     Email = "info@hijhukuk.com",
                     Adres = "Samsun, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-1),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 14,
                     FirmaAdi = "KLM Muhasebe Ltd.",
                     FirmaUnvani = "KLM Muhasebe Limited Şirketi",
                     VergiNo = "4567890124",
                     Telefon = "0382 555 1414",
                     Email = "info@klmmuhasebe.com",
                     Adres = "Kırıkkale, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-6),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                                  new() {
                      Id = 15,
                      FirmaAdi = "NOP Reklam A.Ş.",
                      FirmaUnvani = "NOP Reklam Anonim Şirketi",
                      VergiNo = "5678901235",
                      Telefon = "0374 555 1515",
                      Email = "info@nopreklam.com",
                      Adres = "Bolu, Türkiye",
                      UyelikDurumu = UyelikDurumu.Beklemede,
                      UyelikBaslangicTarihi = DateTime.Now.AddMonths(-9),
                      KalanIlanHakki = 0,
                      ToplamKullanilanIlan = 0,
                      BuAySabitUcret = 250.00m,
                      SabitUcretBorc = 250.00m,
                      BeklemedeDurumunaDusmeSayisi = 3,
                      GuncellemeTarihi = DateTime.Now
                  },
                 new() {
                     Id = 16,
                     FirmaAdi = "QRS Temizlik Ltd.",
                     FirmaUnvani = "QRS Temizlik Limited Şirketi",
                     VergiNo = "6789012346",
                     Telefon = "0388 555 1616",
                     Email = "info@qrstemizlik.com",
                     Adres = "Niğde, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-7),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 17,
                     FirmaAdi = "TUV Güvenlik A.Ş.",
                     FirmaUnvani = "TUV Güvenlik Anonim Şirketi",
                     VergiNo = "7890123457",
                     Telefon = "0392 555 1717",
                     Email = "info@tuvguvenlik.com",
                     Adres = "Mersin, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-5),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 18,
                     FirmaAdi = "VWX Çiçek Ltd.",
                     FirmaUnvani = "VWX Çiçek Limited Şirketi",
                     VergiNo = "8901234568",
                     Telefon = "0422 555 1818",
                     Email = "info@vwxcicek.com",
                     Adres = "Malatya, Türkiye",
                     UyelikDurumu = UyelikDurumu.Pasif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-3),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 250.00m,
                     BeklemedeDurumunaDusmeSayisi = 1,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 19,
                     FirmaAdi = "YZA Kozmetik A.Ş.",
                     FirmaUnvani = "YZA Kozmetik Anonim Şirketi",
                     VergiNo = "9012345679",
                     Telefon = "0444 555 1919",
                     Email = "info@yzakozmetik.com",
                     Adres = "Elazığ, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-4),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 },
                 new() {
                     Id = 20,
                     FirmaAdi = "BCD Kitap Ltd.",
                     FirmaUnvani = "BCD Kitap Limited Şirketi",
                     VergiNo = "0123456780",
                     Telefon = "0462 555 2020",
                     Email = "info@bcdkitap.com",
                     Adres = "Trabzon, Türkiye",
                     UyelikDurumu = UyelikDurumu.Aktif,
                     UyelikBaslangicTarihi = DateTime.Now.AddMonths(-1),
                     KalanIlanHakki = 0,
                     ToplamKullanilanIlan = 0,
                     BuAySabitUcret = 250.00m,
                     SabitUcretBorc = 0.00m,
                     BeklemedeDurumunaDusmeSayisi = 0,
                     GuncellemeTarihi = DateTime.Now
                 }
            };

            // Örnek kontör paketleri
            _kontorPaketleri = new List<KontorPaketi>
            {
                new() {
                    Id = 1,
                    PaketAdi = "Başlangıç Paketi",
                    IlanSayisi = 10,
                    PaketFiyati = 500.00m,
                    BirimIlanFiyati = 50.00m,
                    IskontoOrani = 0,
                    NetFiyat = 500.00m,
                    Aciklama = "Küçük işletmeler için ideal",
                    Aktif = true,
                    OlusturmaTarihi = DateTime.Now.AddDays(-30)
                },
                new() {
                    Id = 2,
                    PaketAdi = "Standart Paket",
                    IlanSayisi = 25,
                    PaketFiyati = 1125.00m,
                    BirimIlanFiyati = 45.00m,
                    IskontoOrani = 10,
                    NetFiyat = 1125.00m,
                    Aciklama = "Orta ölçekli firmalar için",
                    Aktif = true,
                    OlusturmaTarihi = DateTime.Now.AddDays(-30)
                },
                new() {
                    Id = 3,
                    PaketAdi = "Premium Paket",
                    IlanSayisi = 50,
                    PaketFiyati = 2000.00m,
                    BirimIlanFiyati = 40.00m,
                    IskontoOrani = 20,
                    NetFiyat = 2000.00m,
                    Aciklama = "Büyük firmalar için",
                    Aktif = true,
                    OlusturmaTarihi = DateTime.Now.AddDays(-30)
                },
                new() {
                    Id = 4,
                    PaketAdi = "Kurumsal Paket",
                    IlanSayisi = 100,
                    PaketFiyati = 3500.00m,
                    BirimIlanFiyati = 35.00m,
                    IskontoOrani = 30,
                    NetFiyat = 3500.00m,
                    Aciklama = "Kurumsal müşteriler için",
                    Aktif = true,
                    OlusturmaTarihi = DateTime.Now.AddDays(-30)
                }
            };

                                                   // Sabit ücret ödemeleri oluştur
              _sabitUcretOdemeler = new List<SabitUcretOdeme>();
              var odemeId = 1;
  
              foreach (var firma in _firmalar)
              {
                  var sozlesmeTarihi = firma.UyelikBaslangicTarihi;
                  var bugun = DateTime.Now;
                  
                  // İlk ödeme sözleşme tarihinde olmalı
                  var currentDate = sozlesmeTarihi;
                  
                  while (currentDate <= bugun)
                  {
                      var odemeDurumu = OdemeDurumu.Odendi;
                      var odemeTarihi = currentDate;
                      
                      // Bu ay ödemesi için bazı firmalar ödeme yapmış, bazıları yapmamış
                      if (currentDate.Month == bugun.Month && currentDate.Year == bugun.Year)
                      {
                          if (firma.Id <= 10)
                          {
                              // Aktif firmalar: Ödeme yapılmış, ödeme tarihi sözleşme tarihinden sonra
                              odemeDurumu = OdemeDurumu.Odendi;
                              odemeTarihi = currentDate.AddDays(random.Next(1, 15)); // 1-15 gün gecikme
                          }
                          else
                          {
                              // Pasif firmalar: Ödeme yapılmamış, bekliyor durumunda
                              odemeDurumu = OdemeDurumu.Bekliyor;
                              odemeTarihi = currentDate; // Sözleşme tarihi
                          }
                      }
                      else
                      {
                          // Geçmiş aylar: Bazı firmalar ödeme yapmamış (beklemede durumu için)
                          if (firma.Id == 15 && currentDate.Month >= 3) // NOP Reklam - 3+ ay ödeme yapmamış
                          {
                              odemeDurumu = OdemeDurumu.Bekliyor;
                              odemeTarihi = currentDate; // Sözleşme tarihi
                          }
                          else if (firma.Id == 9 && currentDate.Month >= 2) // VWX Mobilya - 2+ ay ödeme yapmamış
                          {
                              odemeDurumu = OdemeDurumu.Bekliyor;
                              odemeTarihi = currentDate; // Sözleşme tarihi
                          }
                          else
                          {
                              // Diğer firmalar: Hepsi ödenmiş, gecikme ile
                              odemeDurumu = OdemeDurumu.Odendi;
                              odemeTarihi = currentDate.AddDays(random.Next(1, 20)); // 1-20 gün gecikme
                          }
                      }
                      
                                             _sabitUcretOdemeler.Add(new SabitUcretOdeme
                       {
                           Id = odemeId++,
                           FirmaId = firma.Id,
                           OdemeTarihi = odemeTarihi,
                           OdemeDonemi = currentDate.ToString("yyyy-MM-dd"),
                           OdemeDurumu = odemeDurumu,
                           Tutar = 250.00m,
                           FaturaNo = $"FTR-{currentDate:yyyy}-{odemeId:D3}",
                           MakbuzNo = $"MKB-{currentDate:yyyy}-{odemeId:D3}",
                           Aciklama = $"{currentDate:MMMM yyyy} ayı sabit ücret",
                           Firma = firma
                       });
                      
                      // Sonraki ay aynı gün
                      currentDate = currentDate.AddMonths(1);
                  }
              }

            // Kontör paket satışları oluştur
            _kontorPaketSatislar = new List<KontorPaketSatis>();
            var satisId = 1;

            foreach (var firma in _firmalar.Where(f => f.UyelikDurumu == UyelikDurumu.Aktif))
            {
                var sozlesmeTarihi = firma.UyelikBaslangicTarihi;
                var bugun = DateTime.Now;
                
                var currentDate = sozlesmeTarihi.AddDays(random.Next(15, 30));
                
                while (currentDate <= bugun)
                {
                    if (random.Next(1, 101) <= 30)
                    {
                        var paket = _kontorPaketleri[random.Next(0, _kontorPaketleri.Count)];
                        
                        _kontorPaketSatislar.Add(new KontorPaketSatis
                        {
                            Id = satisId++,
                            FirmaId = firma.Id,
                            KontorPaketiId = paket.Id,
                            SatisTarihi = currentDate,
                            IlanSayisi = paket.IlanSayisi,
                            BirimFiyat = paket.BirimIlanFiyati,
                            ToplamFiyat = paket.PaketFiyati,
                            IskontoOrani = paket.IskontoOrani,
                            IskontoTutari = paket.PaketFiyati * (paket.IskontoOrani / 100m),
                            NetTutar = paket.NetFiyat,
                            OdemeDurumu = OdemeDurumu.Odendi,
                            FaturaNo = $"FTR-{currentDate:yyyy}-{satisId:D3}",
                            MakbuzNo = $"MKB-{currentDate:yyyy}-{satisId:D3}",
                            Aciklama = $"{paket.IlanSayisi} ilan paketi satın alındı",
                            Firma = firma,
                            KontorPaketi = paket
                        });
                    }
                    
                    currentDate = currentDate.AddMonths(random.Next(1, 4));
                }
            }

                         // Dinamik ilan satışları oluştur
             _dinamikIlanSatislar = new List<DinamikIlanSatis>();
             var dinamikSatisId = 1;
 
             foreach (var firma in _firmalar.Where(f => f.UyelikDurumu == UyelikDurumu.Aktif))
             {
                 var sozlesmeTarihi = firma.UyelikBaslangicTarihi;
                 var bugun = DateTime.Now;
                 
                 var currentDate = sozlesmeTarihi.AddDays(random.Next(20, 40));
                 
                 while (currentDate <= bugun)
                 {
                     if (random.Next(1, 101) <= 40)
                     {
                         var istenenIlanSayisi = random.Next(5, 21);
                         
                         // O ay içinde daha önce alınan ilan sayısını bul (sabit ücret dahil 5 ilan + dinamik alınanlar)
                         var oAyIcinDahaOnceAlinanDinamikIlan = _dinamikIlanSatislar
                             .Where(s => s.FirmaId == firma.Id && 
                                        s.SatisTarihi.Month == currentDate.Month && 
                                        s.SatisTarihi.Year == currentDate.Year)
                             .Sum(s => s.IstenenIlanSayisi);
                         
                         // Toplam kullanılan ilan: 5 (sabit) + daha önce alınan dinamik ilanlar
                         var mevcutToplamIlan = 5 + oAyIcinDahaOnceAlinanDinamikIlan;
                         var yeniToplamIlan = mevcutToplamIlan + istenenIlanSayisi;
                         
                         decimal birimFiyat = 50.00m;
                         decimal iskontoOrani = 0;
                         
                         // Toplam ilan sayısına göre iskonto hesapla
                         if (yeniToplamIlan <= 5) iskontoOrani = 0;
                         else if (yeniToplamIlan <= 10) iskontoOrani = 5;
                         else if (yeniToplamIlan <= 25) iskontoOrani = 10;
                         else if (yeniToplamIlan <= 50) iskontoOrani = 15;
                         else if (yeniToplamIlan <= 100) iskontoOrani = 20;
                         else iskontoOrani = 25;
                         
                         var toplamFiyat = istenenIlanSayisi * birimFiyat;
                         var iskontoTutari = toplamFiyat * (iskontoOrani / 100m);
                         var netTutar = toplamFiyat - iskontoTutari;
                         
                         _dinamikIlanSatislar.Add(new DinamikIlanSatis
                         {
                             Id = dinamikSatisId++,
                             FirmaId = firma.Id,
                             SatisTarihi = currentDate,
                             IstenenIlanSayisi = istenenIlanSayisi,
                             MevcutToplamIlan = mevcutToplamIlan,
                             YeniToplamIlan = yeniToplamIlan,
                             BirimFiyat = birimFiyat,
                             ToplamFiyat = toplamFiyat,
                             IskontoOrani = iskontoOrani,
                             IskontoTutari = iskontoTutari,
                             NetTutar = netTutar,
                             OdemeDurumu = OdemeDurumu.Odendi,
                             FaturaNo = $"FTR-{currentDate:yyyy}-{dinamikSatisId:D3}",
                             MakbuzNo = $"MKB-{currentDate:yyyy}-{dinamikSatisId:D3}",
                             Aciklama = $"{istenenIlanSayisi} ilan dinamik satın alındı (Mevcut: {mevcutToplamIlan}, Toplam: {yeniToplamIlan}, %{iskontoOrani} iskonto)",
                             Firma = firma
                         });
                     }
                     
                     currentDate = currentDate.AddMonths(random.Next(1, 3));
                 }
             }

            // Firma verilerini güncelle
            foreach (var firma in _firmalar)
            {
                FirmaOdemeTutariniHesapla(firma);
                FirmaUyelikDurumunuKontrolEt(firma);
            }
        }

                 private static void FirmaOdemeTutariniHesapla(Firma firma)
         {
             var bugun = DateTime.Now;
             
             // Bu ay sabit ücret
             firma.BuAySabitUcret = 250.00m;
             
             // Toplam kullanılan ilan sayısını hesapla (sabit ücret + paket + dinamik)
             var sabitUcretIlanSayisi = _sabitUcretOdemeler
                 .Where(o => o.FirmaId == firma.Id && o.OdemeDurumu == OdemeDurumu.Odendi)
                 .Count() * 5; // Her ay 5 ilan
             
             var paketIlanSayisi = _kontorPaketSatislar
                 .Where(s => s.FirmaId == firma.Id && s.OdemeDurumu == OdemeDurumu.Odendi)
                 .Sum(s => s.IlanSayisi);
             
             var dinamikIlanSayisi = _dinamikIlanSatislar
                 .Where(s => s.FirmaId == firma.Id && s.OdemeDurumu == OdemeDurumu.Odendi)
                 .Sum(s => s.IstenenIlanSayisi);
             
             firma.ToplamKullanilanIlan = sabitUcretIlanSayisi + paketIlanSayisi + dinamikIlanSayisi;
             
             // Kalan ilan hakkını hesapla (bu ay sabit ücret + bu ay alınan paket/dinamik - kullanılan)
             var buAySabitUcretIlan = 5; // Bu ay sabit ücret dahil 5 ilan
             
             var buAyPaketIlan = _kontorPaketSatislar
                 .Where(s => s.FirmaId == firma.Id && 
                            s.SatisTarihi.Month == bugun.Month && 
                            s.SatisTarihi.Year == bugun.Year && 
                            s.OdemeDurumu == OdemeDurumu.Odendi)
                 .Sum(s => s.IlanSayisi);
             
             var buAyDinamikIlan = _dinamikIlanSatislar
                 .Where(s => s.FirmaId == firma.Id && 
                            s.SatisTarihi.Month == bugun.Month && 
                            s.SatisTarihi.Year == bugun.Year && 
                            s.OdemeDurumu == OdemeDurumu.Odendi)
                 .Sum(s => s.IstenenIlanSayisi);
             
             firma.KalanIlanHakki = buAySabitUcretIlan + buAyPaketIlan + buAyDinamikIlan;
         }

        private static void FirmaUyelikDurumunuKontrolEt(Firma firma)
        {
            var bugun = DateTime.Now;
            
            // Bu ay için ödeme var mı kontrol et
            var buAyOdemeVarMi = _sabitUcretOdemeler
                .Any(o => o.FirmaId == firma.Id && 
                          o.OdemeDonemi.StartsWith(bugun.ToString("yyyy-MM")) && 
                          o.OdemeDurumu == OdemeDurumu.Odendi);
            
            // Bu yıl içindeki gecikme olan ödemeleri say (sadece ödenmiş ama gecikmiş olanlar)
            var buYilGecikmeSayisi = _sabitUcretOdemeler
                .Count(o => o.FirmaId == firma.Id && 
                           o.OdemeTarihi.Year == bugun.Year &&
                           o.OdemeDurumu == OdemeDurumu.Odendi &&
                           o.OdemeTarihi > DateTime.ParseExact(o.OdemeDonemi, "yyyy-MM-dd", null));
            
            // Son beklemede durumuna düşme tarihini bul (en son gecikme olan ödeme)
            var sonGecikmeTarihi = _sabitUcretOdemeler
                .Where(o => o.FirmaId == firma.Id && 
                           o.OdemeDurumu == OdemeDurumu.Odendi &&
                           o.OdemeTarihi > DateTime.ParseExact(o.OdemeDonemi, "yyyy-MM-dd", null))
                .OrderByDescending(o => o.OdemeTarihi)
                .Select(o => o.OdemeTarihi)
                .FirstOrDefault();
            
            if (buAyOdemeVarMi)
            {
                // Ödeme yapılmışsa aktif yap
                firma.UyelikDurumu = UyelikDurumu.Aktif;
                firma.SabitUcretBorc = 0.00m;
            }
            else
            {
                // Ödeme yapılmamışsa
                if (buYilGecikmeSayisi >= 3)
                {
                    // 3+ kez gecikme: Beklemede (admin onayı gerekli)
                    firma.UyelikDurumu = UyelikDurumu.Beklemede;
                    firma.SabitUcretBorc = 250.00m;
                }
                else
                {
                    // 1-2 kez gecikme: Pasif (otomatik aktif olabilir)
                    firma.UyelikDurumu = UyelikDurumu.Pasif;
                    firma.SabitUcretBorc = 250.00m;
                }
            }
            
            // Beklemede durumuna düşme sayısını güncelle (sadece gecikme sayısı)
            firma.BeklemedeDurumunaDusmeSayisi = buYilGecikmeSayisi;
            firma.SonBeklemedeDurumunaDusmeTarihi = sonGecikmeTarihi != default(DateTime) ? sonGecikmeTarihi : null;
        }
    }
    
    public class DashboardViewModel
    {
        public int ToplamFirmaSayisi { get; set; }
        public int PasifFirmaSayisi { get; set; }
        public decimal BuAyToplamGelir { get; set; }
        public decimal BuYilToplamGelir { get; set; }
        public int OdenmemisSabitUcretSayisi { get; set; }
        public int BuAySatilanIlanSayisi { get; set; }
        public int BuYilSatilanIlanSayisi { get; set; }
        public List<SabitUcretOdeme> SonOdemeler { get; set; } = new();
    }
}