using Microsoft.AspNetCore.Mvc;
using netzwelt_exam.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace netzwelt_exam.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
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
                        var resultModel = JsonConvert.DeserializeObject<TerritoryResponseModel>(result);
                        FormatResults(resultModel);
                    }
                }
            }

            return View();
        }

        private List<TerritoryViewModel> FormatResults(TerritoryResponseModel data)
        {
            var result = new List<TerritoryViewModel>();

            foreach (var item in data.Territories)
            {
                var parentId = item.Id;

                //get all places that are under parentId
                var subPlaces = data.Territories.Where(x => x.Parent.HasValue && x.Parent.Value == parentId);
                var subPlacesNames = subPlaces.Select(x => x.Name);
                result.Add(new TerritoryViewModel
                {
                    Parent = item.Name,
                    Children = subPlacesNames
                });
            }

            return result;
        }
    }
}
