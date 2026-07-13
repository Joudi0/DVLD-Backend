using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Shared;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(PersonFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] PersonBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await Person.addPerson(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await Person.getPersonByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Retrieves a record by its PersonID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(PersonFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            PersonFullOutputDTO result = await Person.getPersonByID(id);
            if (result == null) return NotFound($"Person with PersonID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] PersonBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await Person.updatePerson(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("PersonID/{PersonID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int PersonID)
        {
            bool isDeleted = await Person.deletePerson(PersonID);
            if (!isDeleted) return NotFound($"Person not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/PersonID/{PersonID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int PersonID)
        {      
            bool exists = await Person.isPersonExistByID(PersonID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<PersonBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "PersonID", [FromQuery] string direction = "ASC")
        {
            List<PersonBriefOutputDTO> list = await Person.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<PersonBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<PersonBriefOutputDTO> list = await Person.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<PersonFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<PersonFullOutputDTO> list = await Person.getAllFull();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves a record by its NationalNo wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("NationalNo/{NationalNo}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(PersonFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNationalNo(string NationalNo)
        {            PersonFullOutputDTO result = await Person.getPersonByNationalNo(NationalNo);
            if (result == null) return NotFound($"Person with NationalNo {NationalNo} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("exists/NationalNo/{NationalNo}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByNationalNo(string NationalNo)
        {      
            bool exists = await Person.isPersonExistByNationalNo(NationalNo);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief/by/FirstName/{FirstName}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<PersonBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByFirstName(string FirstName)
        {
            List<PersonBriefOutputDTO> list = await Person.getAllBriefByFirstName(FirstName);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full/by/FirstName/{FirstName}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<PersonFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByFirstName(string FirstName)
        {
            List<PersonFullOutputDTO> list = await Person.getAllFullByFirstName(FirstName);
            return Ok(list);
        }

    }
}
