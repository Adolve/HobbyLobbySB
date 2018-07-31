using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HobbyLobbySBView.Models;
using HobbyLobbySBView.ViewModels;
using Newtonsoft.Json;

namespace HobbyLobbySBView.Controllers
{
    public class FitLocationController : Controller
    {
        // GET: FitLocation
        public ActionResult Index()
        {
            var viewModel = new IndexFitLocationViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Result(IndexFitLocationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }
            

            BoxLocation boxLocation = new BoxLocation()
            {
                Location = new Location()
                {
                    Length = viewModel.LocL,
                    Width = viewModel.LocW,
                    Height = viewModel.LocH
                },
                Box = new Box()
                {
                    Length = viewModel.BoxL,
                    Width = viewModel.BoxW,
                    Height = viewModel.BoxH
                }
            };
            string result = await SaveProduct(boxLocation);
            result = result.Trim(new Char[] {' ', '"'});
            result = result.Replace(@"\n",Environment.NewLine);
            

            return View(new ResultFixLocationViewModel()
            {
                Result = result
                //Result = "Location: LWH\nBox : LWH\n\n52 ÷ 5 = 10 go back\n10 ÷ 5 = 2 go across\n15 ÷ 5 = 3 go up\n\nNumber total of boxes: 60\n\nInches Left\nL: 2\nW: 0\nH: 0\n"


            });
        }
        public async Task<string> SaveProduct(BoxLocation boxLocation)
        {
            string data = "null";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://hobbylobbysb-dev.us-west-2.elasticbeanstalk.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(boxLocation), Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/core/v1/CompleteReport", content);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    
                }
            }
            return data;
        }
    }
}