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
    public class LocalDrivingLicenseApplicationController : ControllerBase
    {

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(LocalDrivingLicenseApplicationFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] LocalDrivingLicenseApplicationBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await LocalDrivingLicenseApplication.addLocalDrivingLicenseApplication(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await LocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Retrieves a record by its LocalDrivingLicenseApplicationID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(LocalDrivingLicenseApplicationFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            LocalDrivingLicenseApplicationFullOutputDTO result = await LocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByID(id);
            if (result == null) return NotFound($"LocalDrivingLicenseApplication with LocalDrivingLicenseApplicationID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] LocalDrivingLicenseApplicationBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await LocalDrivingLicenseApplication.updateLocalDrivingLicenseApplication(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("LocalDrivingLicenseApplicationID/{LocalDrivingLicenseApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int LocalDrivingLicenseApplicationID)
        {
            bool isDeleted = await LocalDrivingLicenseApplication.deleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            if (!isDeleted) return NotFound($"LocalDrivingLicenseApplication not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/LocalDrivingLicenseApplicationID/{LocalDrivingLicenseApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int LocalDrivingLicenseApplicationID)
        {      
            bool exists = await LocalDrivingLicenseApplication.isLocalDrivingLicenseApplicationExistByID(LocalDrivingLicenseApplicationID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LocalDrivingLicenseApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "LocalDrivingLicenseApplicationID", [FromQuery] string direction = "ASC")
        {
            List<LocalDrivingLicenseApplicationBriefOutputDTO> list = await LocalDrivingLicenseApplication.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LocalDrivingLicenseApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<LocalDrivingLicenseApplicationBriefOutputDTO> list = await LocalDrivingLicenseApplication.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LocalDrivingLicenseApplicationFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<LocalDrivingLicenseApplicationFullOutputDTO> list = await LocalDrivingLicenseApplication.getAllFull();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves a record by its ApplicationID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("ApplicationID/{ApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(LocalDrivingLicenseApplicationFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByApplicationID(int ApplicationID)
        {            LocalDrivingLicenseApplicationFullOutputDTO result = await LocalDrivingLicenseApplication.getLocalDrivingLicenseApplicationByApplicationID(ApplicationID);
            if (result == null) return NotFound($"LocalDrivingLicenseApplication with ApplicationID {ApplicationID} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/ApplicationID/{ApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByApplicationID(int ApplicationID)
        {      
            bool exists = await LocalDrivingLicenseApplication.isLocalDrivingLicenseApplicationExistByApplicationID(ApplicationID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief/by/LicenseClassID/{LicenseClassID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LocalDrivingLicenseApplicationBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByLicenseClassID(int LicenseClassID)
        {
            List<LocalDrivingLicenseApplicationBriefOutputDTO> list = await LocalDrivingLicenseApplication.getAllBriefByLicenseClassID(LicenseClassID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full/by/LicenseClassID/{LicenseClassID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LocalDrivingLicenseApplicationFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByLicenseClassID(int LicenseClassID)
        {
            List<LocalDrivingLicenseApplicationFullOutputDTO> list = await LocalDrivingLicenseApplication.getAllFullByLicenseClassID(LicenseClassID);
            return Ok(list);
        }

    }
}
