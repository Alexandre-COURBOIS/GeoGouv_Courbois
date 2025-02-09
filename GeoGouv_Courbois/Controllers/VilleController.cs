using GeoGouv_Courbois.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GeoGouv_Courbois.Services.Interfaces;

namespace GeoGouv_Courbois.Controllers
{
    public class VilleController : Controller
    {
        private readonly ILogger<VilleController> _logger;
        private readonly IVilleService _villeService;

        public VilleController(ILogger<VilleController> logger, IVilleService villeService, ISqlService sqlService)
        {
            _logger = logger;
            _villeService = villeService;
        }


        public async Task<IActionResult> Ville()
        {
            return View();
        }

        //Retourne les villes/communes sous forme de pagination pour l'affichage tous les 50
        [HttpGet]
        public async Task<IActionResult> GetPaginatedVilles(int page = 1, int pageSize = 50, string sortBy = "nom", bool? ascending = null, string search = "")
        {
            var villes = await _villeService.GetOrdereVille(page, pageSize, sortBy, ascending, search);

            return Ok(villes);
        }
        
        // Insert en base de donnée la ville ainsi que le département voulu
        [HttpGet]
        public async Task<bool> GetDetails(string code)
        {
            Ville ville = await _villeService.GetVilleDetails(code);
            Departement departement = await _villeService.GetDepartementDetails(ville.CodeDepartement);

            
            if (ville == null || departement == null)
            {
                return false;
            }

           return _villeService.InsertVilleAndDepartementInDB(departement, ville); 
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
