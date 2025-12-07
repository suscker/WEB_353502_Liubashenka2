using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WEB_353502_Liubashenka2.Models; 

namespace WEB_353502_Liubashenka2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Лабораторная работа №2";

            var model = new List<ListDemo>
            {
                new ListDemo { Id = 1, Name = "Первый элемент" },
                new ListDemo { Id = 2, Name = "Второй элемент" },
                new ListDemo { Id = 3, Name = "Третий элемент" },
                new ListDemo { Id = 4, Name = "Четвертый элемент" },
                new ListDemo { Id = 5, Name = "Пятый элемент" }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int selectedItem)
        {
            return RedirectToAction("Index");
        }
    }
}