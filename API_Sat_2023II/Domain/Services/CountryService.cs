using API_Sat_2023II.DAL.Entities;
using API_Sat_2023II.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace API_Sat_2023II.Domain.Services
{   
    //Este es el servicio que se va a conectar a la BD.
    public class CountryService : ICountryService
    {
        //¿Para qué un constructor en el servicio?
        //Para poder inyectar la dependencia de la BD, del contexto, porque de ahí se va a conectar a la BD, pero para conectarse hay que inyectar la dependencia.
        
        public readonly DataBaseContext _context;

        //aquí en los paréntesis se inyectó la dependencia de la BD 
        public CountryService(DataBaseContext context) 
        {   
            //¿contexto de la BD? Efecto espejo de lo que se tiene en BD, pq genera en entity framework un BD de forma temporal en memoria
            //de modo que se puedan hacer la transacciones sin tener que ir a la BD en sí.
                _context  = context;
        }

       
        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            //aquí se están trayendo todos los datos que se tienen en la tabla Countries.
            return await _context.Countries.ToListAsync();
           

            /*ToListAsync es de .Net; en este método básicamente dice "en la variable nueva conuntries, conéctese al contexto de la BD,
         vaya a la tabla Countries y traígame una lista de todos los elementos que tengo en dicha tabla".
            Los trae y los guarda en la variable countries que se devuelve.
             */
        }

        public async Task<Country> CreateCountryAsync(Country country)
        {
            try 
            {
                country.Id = Guid.NewGuid(); //así se asigna un Id a un nuevo registro
                country.CreatedDate = DateTime.Now;
                _context.Countries.Add(country); //adicionar un nuevo registro en mi contexto, se está creando el objeto
                //Country en el contexto de mi BD
                await _context.SaveChangesAsync(); //se pone esta línea porque se va a modificar la BD, o sea,
                //se está yendo a la BD para hacer el insert en la tabla Countries

                return country;
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

        public async Task<Country> GetCountryByNameAsync(string name)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Country> EditCountryAsync(Country country)
        {
            try
            {
                
                country.ModifiedDate = DateTime.Now;

                _context.Countries.Update(country); //el método Update de EF Core sirve para actualizar un objeto
                await _context.SaveChangesAsync(); //actualizar para el país lo que se acaba de indicar

                return country;
            }
            catch (DbUpdateException dbUpdateException)
            {
          
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
   
            }
        }

        public async Task<Country> DeleteCountryAsync(Guid id)
        {
            try
            {
                //Aquí con el ID que se trae desde el controller, se está recuperando el país que luego se va a eliminar
                //el país que se recupera, se guarda en la variable country
                var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);

                if (country == null) return null; //si el país no existe, esto retorna un null
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                return country;
            }
            catch (DbUpdateException dbUpdateException)
            {

                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);

            }
        }

    }
}
