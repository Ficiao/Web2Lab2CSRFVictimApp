using Lab2VictimServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Web;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Http;

namespace Lab2VictimServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string name = Request.Cookies["LoginName"];
            string pass = Request.Cookies["LoginPass"];
            string balance = Request.Cookies["Balance"];
            
            if(name != null)
            {
                ViewBag.AccountData = new BankAccount()
                {
                    AccountName = name,
                    AccountPass = pass,
                    Balance = Int32.Parse(balance)
                };
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string accountName, string accountPass)
        {
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("LoginName", accountName, options);
            Response.Cookies.Append("LoginPass", accountPass, options);
            Response.Cookies.Append("Balance", "1000", options);

            ViewBag.AccountData = new BankAccount()
            {
                AccountName = accountName,
                AccountPass = accountPass,
                Balance = 1000
            };

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SafeBalanceChange(float changeAmmount, string recieverName)
        {
            string name = Request.Cookies["LoginName"];
            string pass = Request.Cookies["LoginPass"];
            string balance = Request.Cookies["Balance"];

            if (name != null)
            {
                int newValue = Int32.Parse(balance) - (int)changeAmmount;
                ViewBag.AccountData = new BankAccount()
                {
                    AccountName = name,
                    AccountPass = pass,
                    Balance = Int32.Parse(balance) - (int)changeAmmount
                };

                ViewBag.Reciever = recieverName;
                ViewBag.RecieveAmmount = changeAmmount;

                CookieOptions options = new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1)
                };
                Response.Cookies.Delete("Balance");
                Response.Cookies.Append("Balance", newValue.ToString(), options);
            }

            return View("Index");
        }

        [HttpPost]
        public IActionResult VolunerableBalanceChange(float changeAmmount, string recieverName)
        {
            string name = Request.Cookies["LoginName"];
            string pass = Request.Cookies["LoginPass"];
            string balance = Request.Cookies["Balance"];

            if (name != null)
            {
                int newValue = Int32.Parse(balance) - (int)changeAmmount;
                ViewBag.AccountData = new BankAccount()
                {
                    AccountName = name,
                    AccountPass = pass,
                    Balance = Int32.Parse(balance) - (int)changeAmmount
                };

                ViewBag.Reciever = recieverName;
                ViewBag.RecieveAmmount = changeAmmount;

                CookieOptions options = new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1)
                };
                Response.Cookies.Delete("Balance");
                Response.Cookies.Append("Balance", newValue.ToString(), options);
            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
