using Microsoft.AspNetCore.Mvc;
using netzwelt_exam.Models;
using netzwelt_exam.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace netzwelt_exam.Controllers
{
    public class HomeController : Controller
    {
        private INetzweltSessionService _netzweltSessionService;

        public HomeController(INetzweltSessionService netzweltSessionService)
        {
            _netzweltSessionService = netzweltSessionService;
        }

        public async Task<IActionResult> Index()
        {
            TerritoryResponseModel parsedResponse = new TerritoryResponseModel();
            var returnModel = new TerritoryViewModel();
            if (_netzweltSessionService.IsAuthenticated())
            {
                var url = "https://netzwelt-devtest.azurewebsites.net/Territories/All";                
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (result != null)
                        {
                            parsedResponse = JsonConvert.DeserializeObject<TerritoryResponseModel>(result);
                            returnModel.Territories = GetGroupedResults(parsedResponse);                            
                        }
                    }
                }

                return View(returnModel);
            }
            else
                return Redirect("/Account/Login");

        }

        private List<TerritoryDataViewModel> GetGroupedResults(TerritoryResponseModel data)
        {
            var result = new List<TerritoryDataViewModel>();

            var groupedPlaces = data.Territories
                .GroupBy(item => item.Parent.HasValue ? item.Parent.Value : -1)
                .Where(item => item.Key > 0)
                .Select(group => new { Parent = group.FirstOrDefault().Name, Places = group.Skip(1) })//skip 1 to avoid duplicating the parent
                .ToList();

            foreach (var groupedPlace in groupedPlaces)
            {
                result.Add(new TerritoryDataViewModel
                {
                    Parent = groupedPlace.Parent,
                    Children = groupedPlace.Places.Select(x => x.Name)
                });                
            }

            return result;
        }
    }
}
