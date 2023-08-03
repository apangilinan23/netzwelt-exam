using Microsoft.AspNetCore.Mvc;
using netzwelt_exam.Models;
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
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            if (IsUserAuthenticated())
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
                model.Session.ErrorMessage = GetErrorMessage();
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
                        SaveToSession(model);
                    }
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<ErrorViewModel>(result);
                    model.Session.IsAuthenticated = false;
                    model.Session.ErrorMessage = error.Message;

                    //simulate db saving
                    SaveToSession(model);
                }

            }
            if (model.Session.IsAuthenticated)
                return Redirect("/Home/Index");
            return RedirectToAction("Login");
        }

        private void SaveToSession(LoginViewModel model)
        {
            HttpContext.Session.Set("user-authenticated", BitConverter.GetBytes(model.Session.IsAuthenticated));
            HttpContext.Session.Set("user-error-message", Encoding.ASCII.GetBytes(model.Session.ErrorMessage));
        }

        private bool IsUserAuthenticated()
        {
            bool result = false;
            if (HttpContext.Session.TryGetValue("user-authenticated", out byte[] authByteArrayVal))
            {
                result = BitConverter.ToBoolean(authByteArrayVal);
            }

            return result;
        }

        private string GetErrorMessage()
        {
            string result = string.Empty;
            if (HttpContext.Session.TryGetValue("user-error-message", out byte[] errorMessageByteArrayVal))
            {
                result = Encoding.ASCII.GetString(errorMessageByteArrayVal);
            }

            return result;
        }
    }
}
