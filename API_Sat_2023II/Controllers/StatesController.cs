using API_Sat_2023II.DAL.Entities;
using API_Sat_2023II.Domain.Interfaces;
using API_Sat_2023II.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Sat_2023II.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : Controller
    {
        private readonly IStateService _stateService;
        public StatesController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet, ActionName("Get")] 
        [Route("Get")] //Aquí concateno la URL inicial: URL = api/countries/Get
        public async Task<ActionResult<IEnumerable<State>>> GetStatesByCountryIdAsync(Guid countryId)
        {
            var states = await _stateService.GetStatesByCountryIdAsync(countryId);
            if (states == null || !states.Any()) return NotFound(); //validación del id, por si es 

            //si encuentra al menos un país, retorna un OK
            return Ok(states);  //Ok = 200 HTTP Status Code  y está adentro el país que se va a mostrar
        }

        [HttpPost, ActionName("Create")] //DataNotation
        [Route("Create")] //Aquí concateno a URL inicial
        public async Task<ActionResult> CreateStateAsync(State state, Guid countryId)
        {
            try
            {
                var createdState = await _stateService.CreateStateAsync(state, countryId);
                
                if (createdState == null) return NotFound(); //NotFound = 404 HTTP Status Code


                return Ok(createdState);  //retorne 200 y el objeto Country
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    return Conflict(String.Format("El Estado {0} ya existe.", state.Name)); //conflict es un error 409
                }

                return Conflict(ex.Message);
            }
        }

        [HttpGet, ActionName("Get")] //DataNotation
        [Route("GetById/{id}")] //Aquí concateno la URL inicial: URL = api/countries/Get
        public async Task<ActionResult<State>> GetStateByIdAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!"); //validación del id, por si es 

            //aquí se está yendo a la capa de domain para traer el país.
            var state = await _stateService.GetStateByIdAsync(id);  //está trayendo la dependencia del servicio más el método

            if (state == null) return NotFound();//si country es nulo o si está vacío

            //si encuentra al menos un país, retorna un OK
            return Ok(state);  //Ok = 200 HTTP Status Code  y está adentro el país que se va a mostrar
        }

        [HttpPut, ActionName("Edit")]
        [Route("Edit")]
        public async Task<ActionResult<State>> EditStateAsync(State state, Guid id)
        {
            try
            {
                var editedState = await _stateService.EditStateAsync(state, id);
                return Ok(editedState);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", state.Name));

                return Conflict(ex.Message);
            }
        }

        [HttpDelete, ActionName("Delete")]
        [Route("Delete")]
        public async Task<ActionResult<State>> DeleteStateAsync(Guid id)
        {
            if (id == null) return BadRequest("Id es requerido!");

            var deletedState = await _stateService.DeleteStateAsync(id);

            if (deletedState == null) return NotFound("País no encontrado!");

            return Ok(deletedState);
        }
    }
}
