using API_Sat_2023II.DAL.Entities;

namespace API_Sat_2023II.Domain.Interfaces
{
    public interface IStateService
    {
        //se va a devolver un solo país, entonces singular
        Task<IEnumerable<State>> GetStatesByCountryIdAsync(Guid countryId);
        Task<State> CreateStateAsync(State state, Guid countryId);
        Task<State> GetStateByIdAsync(Guid id);
        Task<State> EditStateAsync(State state, Guid id);
        Task<State> DeleteStateAsync(Guid id);

    }
}
