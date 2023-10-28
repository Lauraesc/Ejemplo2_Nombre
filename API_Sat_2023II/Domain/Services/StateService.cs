using API_Sat_2023II.DAL.Entities;
using API_Sat_2023II.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace API_Sat_2023II.Domain.Services
{   
    //Este es el servicio que se va a conectar a la BD.
    public class StateService : IStateService
    {
        //¿Para qué un constructor en el servicio?
        //Para poder inyectar la dependencia de la BD, del contexto, porque de ahí se va a conectar a la BD, pero para conectarse hay que inyectar la dependencia.
        
        public readonly DataBaseContext _context;

        //aquí en los paréntesis se inyectó la dependencia de la BD 
        public StateService(DataBaseContext context) 
        {   
            //¿contexto de la BD? Efecto espejo de lo que se tiene en BD, pq genera en entity framework un BD de forma temporal en memoria
            //de modo que se puedan hacer la transacciones sin tener que ir a la BD en sí.
                _context  = context;
        }

       
        public async Task<IEnumerable<State>> GetStatesByCountryIdAsync(Guid countryId)
        {
            //aquí se están trayendo todos los datos que se tienen en la tabla States.
            return await _context.States.Where(s => s.CountryId == countryId).ToListAsync();
            
        }

        public async Task<State> CreateStateAsync(State state, Guid countryId)
        {
            try 
            {   
                state.Id = Guid.NewGuid(); //así se asigna un Id a un nuevo registro
                state.CreatedDate = DateTime.Now;
                state.CountryId = countryId;
                state.Country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == countryId);
                state.ModifiedDate = null;
                
                _context.States.Add(state); //adicionar un nuevo registro en mi contexto, se está creando el objeto
                //Country en el contexto de mi BD
                await _context.SaveChangesAsync(); //se pone esta línea porque se va a modificar la BD, o sea,
                //se está yendo a la BD para hacer el insert en la tabla Countries

                return state;
            }
            catch  (DbUpdateException dbUpdateException)
            { 
                //esta excepción me captura un mensaje cuando el país ya existe (duplicados)
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
                //coallesences notation
            }
        }

        /*recordar la importancia de indicar el 'async' en el método */
        public async Task<Country> GetCountryByIdAsync(Guid id)
        {
            //diferentes formas de obtener el país:
            return await _context.Countries.FindAsync(id); //FindAsync es un método propio del DbContext (DbSet)
            //return await _context.Countries.FirstAsync(x => x.Id == id); este usa una lambda expression en paréntesis y es de EF Core
            //return await _context.Countries.FirstOrDefaultAsync(id); este método también usa lambda expression y es de EF Core

        }

        public async Task<State> GetStateByIdAsync(Guid id)
        {
            return await _context.States.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<State> EditStateAsync(State state, Guid id)
        {
            try
            {
                
                state.ModifiedDate = DateTime.Now;

                _context.States.Update(state); //el método Update de EF Core sirve para actualizar un objeto
                await _context.SaveChangesAsync(); //actualizar para el país lo que se acaba de indicar

                return state;
            }
            catch (DbUpdateException dbUpdateException)
            {
          
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
   
            }
        }

        public async Task<State> DeleteStateAsync(Guid id)
        {
            try
            {
                //Aquí con el ID que se trae desde el controller, se está recuperando el país que luego se va a eliminar
                //el país que se recupera, se guarda en la variable country
                var state = await _context.States.FirstOrDefaultAsync(s => s.Id == id);

                if (state == null) return null; //si el país no existe, esto retorna un null
                _context.States.Remove(state);
                await _context.SaveChangesAsync();

                return state;
            }
            catch (DbUpdateException dbUpdateException)
            {

                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);

            }
        }


    }
}
