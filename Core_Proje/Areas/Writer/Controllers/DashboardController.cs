using Core_Proje.Areas.Writer.Models.ResponseModel;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;  // Newtonsoft.Json kullanarak JSON veriyi işleyeceğiz.
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core_Proje.Areas.Writer.Controllers
{
    [Area("Writer")]
    public class DashboardController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;

        public DashboardController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.v = values.Name + " " + values.Surname;

            // Weather API
            string apiKey = "53ba0460399abc34d44cd55f29faf2ed";  // Geçerli API anahtarınızı buraya koyun
            string latitude = "41.0082";  // İstanbul enlemi
            string longitude = "28.9784";  // İstanbul boylamı
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    // API'den dönen JSON veriyi string olarak alıyoruz
                    string responseBody = await response.Content.ReadAsStringAsync();



                    // JSON veriyi bir JObject'e parse ediyoruz
                    //var weatherData = JObject.Parse(responseBody);
                    var weatherData = JsonSerializer.Deserialize<WeatherResponseModel>(responseBody);

                    // Sıcaklık bilgisini JSON'dan alıyoruz
                    //ViewBag.v5 = weatherData["main"]["temp"].ToString();
                    ViewBag.v5 = weatherData.main.temp.ToString();
                }
                else
                {
                    // Hata durumunu kontrol edin ve kullanıcıya mesaj gösterin
                    ViewBag.ErrorMessage = $"API isteği başarısız oldu: {response.StatusCode}";
                }
            }

            // Diğer istatistikler
            Context c = new Context();
            ViewBag.v1 = c.WriterMessages.Where(x => x.Receiver == values.Email).Count();
            ViewBag.v2 = c.Announcements.Count();
            ViewBag.v3 = c.Users.Count();
            ViewBag.v4 = c.Skills.Count();


            return View();
        }
    }
}
