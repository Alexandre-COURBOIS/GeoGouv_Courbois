using GeoGouv_Courbois.Models;
using GeoGouv_Courbois.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoGouv_Courbois.Controllers
{
    public class DepartementController : Controller
    {

        private readonly ILogger<DepartementController> _logger;
        private readonly ISqlService _sqlService;

        public DepartementController(ILogger<DepartementController> logger, ISqlService sqlService)
        {
            _logger = logger;
            _sqlService = sqlService;
        }

        //Permet de récupérer en base de donnée les villes et département via une procédure stockée
        public ActionResult Departement()
        {
            var departements = _sqlService.GetAllDepartementsWithVilles();
            return View(departements);
        }

        
    }
}
