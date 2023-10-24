using API_Sat_2023II.DAL.Entities;
using API_Sat_2023II.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Sat_2023II.Controllers
{
    //DataNotation --> siempre se pone, es importantísimo, para que se identifique q es el controlador de la API.
    [ApiController]
    [Route("api/[controller]")] //esta es la primera parte de la URL de la API: URL = api/countries
    //en el controlador no se debe poner ningún tipo de lógica, no se puede poner algo como la conexión a BD.
    public class CountriesController : Controller //se hereda de la clase controller para que provea métodos
    {
        private readonly ICountryService _countryService;
        //para conectarme a la interfaz necesito esta dependencia a la interfaz
        public CountriesController(ICountryService countryService) 
        { 
            _countryService = countryService;
        }

        //método que me conecta a la interfaz, en un controlador los métodos se llaman ACCIONES, si es una API, se llama ENDPOINT
        //todo ENDPOINT retorna un ActionResult, significa que retorna el resultado de una ACCIÓN.

        /*este método va a servir para que se pueda traer una lista de países, no para traer un país específico
         para un país en específico se va a crear otro método */

        [HttpGet, ActionName("Get")] //DataNotation
        [Route("Get")] //Aquí concateno a URL inicial: URL = api/countries/Get
        public async Task<ActionResult<IEnumerable<Country>>> GetCountriesAsync() 
        {
            //aquí se está yendo a la capa de domain para traer la lista de países.
            var countries = await _countryService.GetCountriesAsync();  //está trayendo al dependencia del servicio más el método
        
            if (countries == null || !countries.Any()) //si countries es nulo o si está vacío, el Any es para listas
            {
                return NotFound(); //NotFound = 404 HTTP Status Code
            }

            //si encuentra al menos un país, retorna un OK. Lo que va dentro del OK es la lista de países pq es lo que se quiere mostrar
            return Ok(countries);  //Ok = 200 HTTP Status Code  
        }

        //nueva acción
        [HttpPost, ActionName("Create")] //DataNotation
        [Route("Create")] //Aquí concateno a URL inicial
        public async Task<ActionResult> CreateCountryAsync(Country country)
        {
            try
            {
                var createdCountry = await _countryService.CreateCountryAsync(country); 
                if (createdCountry == null)
                {
                    return NotFound(); //NotFound = 404 HTTP Status Code

                }

                return Ok(createdCountry);  //retorne 200 y el objeto Country
            }
            catch (Exception ex) 
            {
                if (ex.Message.Contains("duplicate"))
                {
                    return Conflict(string.Format("El país {0} ya existe.", country.Name)); //conflict es un error 409
                }

                return Conflict(ex.Message); //
            }
        }
    }
}
