using Microsoft.AspNetCore.Mvc;
using netzwelt_exam.Models;
using netzwelt_exam.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace netzwelt_exam.Controllers
{
    public class AccountController : Controller
    {

        private INetzweltSessionService _netzweltSessionService;

        public AccountController(INetzweltSessionService netzweltSessionService)
        {
            _netzweltSessionService = netzweltSessionService;
        }

        public IActionResult Login()
        {
            var model = new LoginViewModel();
            if (_netzweltSessionService.IsAuthenticated())
            {
                var session = new LoginSessionModel
                {
                    ErrorMessage = string.Empty,
                    IsAuthenticated = true
                };
                model.Session = session;
            }
            else
            {
                model.Session.IsAuthenticated = false;
                model.Session.ErrorMessage = _netzweltSessionService.GetErrorMessage();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var url = "https://netzwelt-devtest.azurewebsites.net/Account/SignIn";
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resultModel = JsonConvert.DeserializeObject<LoginResponseModel>(result);
                    if (resultModel.Roles.Any(x => string.Equals("basic-user", x.ToLower())))
                    {
                        model.Session.IsAuthenticated = true;
                        model.Session.ErrorMessage = string.Empty;

                        //simulate db saving
                        _netzweltSessionService.SaveSessionData(model);
                    }
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<ErrorResponseModel>(result);
                    model.Session.IsAuthenticated = false;
                    model.Session.ErrorMessage = error.Message;

                    //simulate db saving
                    _netzweltSessionService.SaveSessionData(model);
                }

            }
            if (model.Session.IsAuthenticated)
                return Redirect("/Home/Index");
            return RedirectToAction("Login");
        }
    }
}
