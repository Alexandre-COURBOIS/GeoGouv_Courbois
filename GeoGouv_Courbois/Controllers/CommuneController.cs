using GeoGouv_Courbois.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GeoGouv_Courbois.Services.Interfaces;

namespace GeoGouv_Courbois.Controllers
{
    public class CommuneController : Controller
    {
        private readonly ILogger<CommuneController> _logger;
        private readonly ICommuneService _communeService;

        public CommuneController(ILogger<CommuneController> logger, ICommuneService geoGouvApiService, ISqlService sqlService)
        {
            _logger = logger;
            _communeService = geoGouvApiService;
        }


        public async Task<IActionResult> Commune()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedCommunes(int page = 1, int pageSize = 50, string sortBy = "nom", bool? ascending = null, string search = "")
        {
            var allCommunes = await _communeService.GetOrderedCommune(page, pageSize, sortBy, ascending, search);

            return Ok(allCommunes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
