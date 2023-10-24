using API_Sat_2023II.DAL.Entities;

namespace API_Sat_2023II.Domain.Interfaces
{
    public interface ICountryService
    {
        //Ilist: manipular listas, hacer order by, seleccionar 10 primeros
        //ICollection: manipular listas, hacer order by, seleccionar 10 primeros
        //IEnumerable: listas estáticas, voy a capturar una lista de la tabla de BD, y esa misma lista la voy a renderizar en una lista

        Task<IEnumerable<Country>> GetCountriesAsync(); //una firma de método
        //primera firma del método que servirá para exponeer a los controladores.

        //se va a devolver un solo país, entonces singular
        Task<Country> CreateCountryAsync(Country country);
    }
}
