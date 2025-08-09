using AutoMapper;
using Microsoft.EntityFrameworkCore;
using serkan_test1.Data;

namespace serkan_test1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            // AutoMapper - MappingProfile sınıfını ekle
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Veritabanı Bağlantısı - Infra.cs'de tanımlanıyor

            // Özel servis kayıtları
            builder.Services.AddInfra(builder.Configuration);

            var app = builder.Build();

            // Ortam kontrolü
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();

            // Default Route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UygulamaDbContext>();

                if (!db.Firmalar.Any())
                {
                    db.Firmalar.Add(new Models.FirmaBilgileriViewModel
                    {
                        FirmaAdi = "A Emlak",
                        FirmaUnvani = "A Emlak Gayrimenkul Ltd. Şti.",
                        FirmaTipi = "Tüzel Kişi",
                        Domain = "aemlak.chatgpt.com",
                        YetkiliAdi = "Ahmet",
                        YetkiliSoyadi = "Yılmaz",
                        YetkiliTCNo = "12345678901",
                        Sifre = "",
                        Ulke = "Türkiye",
                        Il = "İstanbul",
                        Ilce = "Kadıköy",
                        Mahalle = "Fenerbahçe",
                        CepTelefonu = "05554443322",
                        EPosta = "info@aemlak.com",
                        VergiDairesiIl = "İstanbul",
                        VergiDairesi = "Kadıköy VD",
                        VergiNo = "1234567890"
                    });

                    db.SaveChanges();
                }
            }

            app.Run();
        }
    }
}