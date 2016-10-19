using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HighchartsNETCore;

namespace HighchartsNETCoreWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ChartsSeries series = new ChartsSeries();
            Dictionary<object, object> dic = new Dictionary<object, object>();
            Random r = new Random();
            for (int i = 0; i < 12; i++)
            {
                dic.Add(DateTime.Now.AddDays(i).ToString("yyyyMMdd"), r.Next(20));
            }
            series.SeriesName = "温度";
            series.SeriesData = dic;
            ViewBag.Series = series;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
