using DotnetSqliteDemo.Models;

namespace DotnetSqliteDemo;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetPeopleAsync();
    Task<Person> GetPeopleByIdAsync(int id);
    Task<Person> AddPersonAsync(Person person);
    Task UpdatePersonAsync(Person person);
    Task DeletePersonAsync(int id);
}
