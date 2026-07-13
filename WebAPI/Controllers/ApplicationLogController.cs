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
    public class ApplicationLogController : ControllerBase
    {

        /// <summary>
        /// Retrieves a record by its LogID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(ApplicationLogFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            ApplicationLogFullOutputDTO result = await ApplicationLog.getApplicationLogByID(id);
            if (result == null) return NotFound($"ApplicationLog with LogID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] ApplicationLogBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await ApplicationLog.updateApplicationLog(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("LogID/{LogID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int LogID)
        {
            bool isDeleted = await ApplicationLog.deleteApplicationLog(LogID);
            if (!isDeleted) return NotFound($"ApplicationLog not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(ApplicationLogFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] ApplicationLogBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await ApplicationLog.addApplicationLog(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await ApplicationLog.getApplicationLogByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("exists/LogID/{LogID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int LogID)
        {      
            bool exists = await ApplicationLog.isApplicationLogExistByID(LogID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationLogBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "LogID", [FromQuery] string direction = "ASC")
        {
            List<ApplicationLogBriefOutputDTO> list = await ApplicationLog.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationLogBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<ApplicationLogBriefOutputDTO> list = await ApplicationLog.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationLogFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<ApplicationLogFullOutputDTO> list = await ApplicationLog.getAllFull();
            return Ok(list);
        }

    }
}
