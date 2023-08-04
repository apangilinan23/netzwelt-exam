using Microsoft.AspNetCore.Http;
using netzwelt_exam.Models;
using System;
using System.Text;

namespace netzwelt_exam.Services
{
    public class NetzweltSessionService : INetzweltSessionService
    {
        private IHttpContextAccessor _accessor;

        public NetzweltSessionService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public bool IsAuthenticated()
        {
            bool result = false;
            if (_accessor.HttpContext.Session.TryGetValue("user-authenticated", out byte[] authByteArrayVal))
            {
                result = BitConverter.ToBoolean(authByteArrayVal);
            }
            return result;
        }

        public string GetErrorMessage()
        {
            string result = string.Empty;
            if (_accessor.HttpContext.Session.TryGetValue("user-error-message", out byte[] errorMessageByteArrayVal))
            {
                result = Encoding.ASCII.GetString(errorMessageByteArrayVal);
            }
            return result;
        }

        public void SaveSessionData(LoginViewModel model)
        {
            _accessor.HttpContext.Session.Set("user-authenticated", BitConverter.GetBytes(model.Session.IsAuthenticated));
            _accessor.HttpContext.Session.Set("user-error-message", Encoding.ASCII.GetBytes(model.Session.ErrorMessage));
        }
    }
}
