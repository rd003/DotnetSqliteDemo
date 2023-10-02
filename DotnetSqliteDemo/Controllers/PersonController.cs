using DotnetSqliteDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetSqliteDemo;

[Route("/api/people")]
[ApiController]
public class PersonController : ControllerBase
{

    private readonly ILogger<PersonController> _logger;
    private readonly IPersonRepository _personRepo;
    public PersonController(ILogger<PersonController> logger, IPersonRepository personRepo)
    {
        _logger = logger;
        _personRepo = personRepo;
    }
    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        try
        {
            var people = await _personRepo.GetPeopleAsync();
            return Ok(people);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson(Person person)
    {
        try
        {
            var createdPerson = await _personRepo.AddPersonAsync(person);
            return CreatedAtAction(nameof(CreatePerson), createdPerson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePerson(Person personToUpdate)
    {
        try
        {
            var person = await _personRepo.GetPeopleByIdAsync(personToUpdate.Id);
            if (person == null)
            {
                return NotFound(new
                {
                    statusCode = 404
                    ,
                    message = "Person does not found"
                });
            }
            await _personRepo.UpdatePersonAsync(personToUpdate);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }





    [HttpGet("{id}")]
    public async Task<IActionResult> GetPeopleById(int id)
    {
        try
        {
            var person = await _personRepo.GetPeopleByIdAsync(id);
            if (person == null)
            {
                return NotFound(new
                {
                    statusCode = 404
                    ,
                    message = "Person does not found"
                });
            }
            return Ok(person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        try
        {
            var person = await _personRepo.GetPeopleByIdAsync(id);
            if (person == null)
            {
                return NotFound(new
                {
                    statusCode = 404
                    ,
                    message = "Person does not found"
                });
            }
            await _personRepo.DeletePersonAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

}

