using API_Sat_2023II.DAL.Entities;
using API_Sat_2023II.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    }
}
