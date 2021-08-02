using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;


namespace MVCTest.Controllers
{
    public class HomeController : Controller
    {

        MyDataBase db = new MyDataBase();
        public ActionResult Home()
        {
            //ViewBag.account = Account;
            Debug.WriteLine("---------------------------");
            Debug.WriteLine(Request.Url.Query);
            string querystring = Request.Url.Query;
            if (!string.IsNullOrEmpty(querystring) && querystring.Contains("?"))
            {
                querystring = querystring.Replace("?", string.Empty);
            }

            var nvc = HttpUtility.ParseQueryString(querystring);

            Debug.WriteLine(nvc.Get("Account"));
            Debug.WriteLine("---------------------------");

            ViewBag.Account = nvc.Get("Account");
            ViewBag.Title = "aac";
            //List<National_Character> national_Characters = db.GetNational_Charactert();
            //ViewBag.national_Characters = national_Characters;
            return View();
        }
        public ActionResult Index()
        {
            //db.GetNational_Character();
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManyCharacter(string Character = "")
        {
            List<National_Character> list = db.GetNational_Character(Character); ;
            string result = "";
            if (list == null)
            {
                //讀取資料庫錯誤
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }
        }
        [HttpPost]
        public ActionResult Index(UserData data)
        {
            if (string.IsNullOrWhiteSpace(data.password1) || data.password1 != data.password2)
            {
                List<City> cityList = db.GetCityList();
                List<Village> villageList = new List<Village>();
                if (!string.IsNullOrWhiteSpace(data.city))
                    villageList = db.GetVillageList(data.city);
                ViewBag.CityList = cityList;
                ViewBag.VillageList = villageList;
                ViewBag.Msg = "密碼輸入錯誤";
                return View(data);
            }
            else
            {
                if (db.AddUserData(data))
                {
                    Response.Redirect("~/Home/Login");
                    return new EmptyResult();
                }
                else
                {
                    ViewBag.Msg = "註冊失敗...";
                    return View(data);
                }
            }
        }
        [HttpPost]
        public ActionResult Village(string id = "")
        {
            List<Village> list = db.GetVillageList(id);
            string result = "";
            if (list == null)
            {
                //讀取資料庫錯誤
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }
        }

        [HttpPost]
        public ActionResult NationalUnicode(string Unicode = "")
        {
            National_Character list = db.GetNational_Unicode_One(Unicode);
            string result = "";
            if (list == null)
            {
                //讀取資料庫錯誤
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }
        }
        [HttpPost]
        public ActionResult NationalCharacter(string Character = "")
        {
            National_Character list = db.GetNational_Character_One(Character);
            string result = "";
            if (list == null)
            {
                //讀取資料庫錯誤
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }
        }
        [HttpPost]
        public ActionResult Login(FormCollection post)
        {
            string account = post["account"];
            string password = post["password"];

            //驗證密碼
            if (db.CheckUserData(account, password))
            {
                Response.Redirect("~/Home/Home?Account="+account);
                
                return new EmptyResult();
            }
            else
            {
                ViewBag.Msg = "登入失敗...";
                return View();
            }
        }
    }
}