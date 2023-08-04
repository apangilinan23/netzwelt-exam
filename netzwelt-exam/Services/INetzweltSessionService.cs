using netzwelt_exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netzwelt_exam.Services
{
    public interface INetzweltSessionService
    {
        bool IsAuthenticated();

        string GetErrorMessage();

        void SaveSessionData(LoginViewModel model);
    }
}
