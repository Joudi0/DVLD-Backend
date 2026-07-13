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
    public class ApplicationController : ControllerBase
    {

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(ApplicationFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] ApplicationBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await Application.addApplication(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await Application.getApplicationByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Retrieves a record by its ApplicationID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(ApplicationFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            ApplicationFullOutputDTO result = await Application.getApplicationByID(id);
            if (result == null) return NotFound($"Application with ApplicationID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] ApplicationBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await Application.updateApplication(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("ApplicationID/{ApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int ApplicationID)
        {
            bool isDeleted = await Application.deleteApplication(ApplicationID);
            if (!isDeleted) return NotFound($"Application not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/ApplicationID/{ApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int ApplicationID)
        {      
            bool exists = await Application.isApplicationExistByID(ApplicationID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "ApplicationID", [FromQuery] string direction = "ASC")
        {
            List<ApplicationBriefOutputDTO> list = await Application.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<ApplicationBriefOutputDTO> list = await Application.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<ApplicationFullOutputDTO> list = await Application.getAllFull();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief/by/ApplicantPersonID/{ApplicantPersonID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByApplicantPersonID(int ApplicantPersonID)
        {
            List<ApplicationBriefOutputDTO> list = await Application.getAllBriefByApplicantPersonID(ApplicantPersonID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full/by/ApplicantPersonID/{ApplicantPersonID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByApplicantPersonID(int ApplicantPersonID)
        {
            List<ApplicationFullOutputDTO> list = await Application.getAllFullByApplicantPersonID(ApplicantPersonID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief/by/ApplicationTypeID/{ApplicationTypeID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByApplicationTypeID(int ApplicationTypeID)
        {
            List<ApplicationBriefOutputDTO> list = await Application.getAllBriefByApplicationTypeID(ApplicationTypeID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full/by/ApplicationTypeID/{ApplicationTypeID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByApplicationTypeID(int ApplicationTypeID)
        {
            List<ApplicationFullOutputDTO> list = await Application.getAllFullByApplicationTypeID(ApplicationTypeID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-brief/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<ApplicationBriefOutputDTO> list = await Application.getAllBriefByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-full/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<ApplicationFullOutputDTO> list = await Application.getAllFullByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

    }
}
