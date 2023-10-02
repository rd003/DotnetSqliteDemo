using System.Data;
using System.Data.SQLite;
using Dapper;
using DotnetSqliteDemo.Models;

namespace DotnetSqliteDemo.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;
    public PersonRepository(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("default");
    }

    // private IDbConnection GetConnection()
    // {
    //     using IDbConnection connection = new SQLiteConnection(_connectionString);
    //     return connection;
    // }
    public async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        using IDbConnection connection = new SQLiteConnection(_connectionString);
        string sql = "select * from Person";
        var people = await connection.QueryAsync<Person>(sql);
        return people;
    }

    public async Task<Person> GetPeopleByIdAsync(int id)
    {
        using IDbConnection connection = new SQLiteConnection(_connectionString);
        string sql = "select * from Person where Id=@id";
        var person = await connection.QueryFirstOrDefaultAsync<Person>(sql, new { id });
        return person;
    }


    public async Task<Person> AddPersonAsync(Person person)
    {
        using IDbConnection connection = new SQLiteConnection(_connectionString);
        string sql = @"insert into Person (Name,Email) values (@Name,@Email);
                       SELECT last_insert_rowid()";
        int createdId = await connection.ExecuteScalarAsync<int>(sql, new
        {
            person.Name,
            person.Email
        });
        person.Id = createdId;
        return person;
    }

    public async Task UpdatePersonAsync(Person person)
    {
        using IDbConnection connection = new SQLiteConnection(_connectionString);
        string sql = @"update Person set Name=@Name,Email=@Email where Id=@Id";
        await connection.ExecuteAsync(sql, new { Name = person.Name, Email = person.Email, Id = person.Id });
    }

    public async Task DeletePersonAsync(int id)
    {
        using IDbConnection connection = new SQLiteConnection(_connectionString);
        string sql = @"delete from Person where Id=@id";
        await connection.ExecuteAsync(sql, new { id });
    }
}
