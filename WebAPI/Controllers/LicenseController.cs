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
    public class LicenseController : ControllerBase
    {

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(LicenseFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] LicenseBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await License.addLicense(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await License.getLicenseByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Admin-only: Adds a new record using FullInputDTO to define system-level settings.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost("admin")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(LicenseFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddAdmin([FromBody] LicenseFullInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            
            int insertedID = await License.addAdminLicense(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await License.getLicenseByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Retrieves a record by its LicenseID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(LicenseFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            LicenseFullOutputDTO result = await License.getLicenseByID(id);
            if (result == null) return NotFound($"License with LicenseID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] LicenseBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await License.updateLicense(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Updates a record using FullInputDTO (Admin Access - Full Control).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut("admin")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> UpdateAdmin([FromBody] LicenseFullInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            
            bool isUpdated = await License.updateLicense(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("LicenseID/{LicenseID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int LicenseID)
        {
            bool isDeleted = await License.deleteLicense(LicenseID);
            if (!isDeleted) return NotFound($"License not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/LicenseID/{LicenseID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int LicenseID)
        {      
            bool exists = await License.isLicenseExistByID(LicenseID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "LicenseID", [FromQuery] string direction = "ASC")
        {
            List<LicenseBriefOutputDTO> list = await License.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<LicenseBriefOutputDTO> list = await License.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LicenseFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<LicenseFullOutputDTO> list = await License.getAllFull();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves a record by its ApplicationID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("ApplicationID/{ApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(LicenseFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByApplicationID(int ApplicationID)
        {            LicenseFullOutputDTO result = await License.getLicenseByApplicationID(ApplicationID);
            if (result == null) return NotFound($"License with ApplicationID {ApplicationID} not found.");
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
            bool exists = await License.isLicenseExistByApplicationID(ApplicationID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief/by/DriverID/{DriverID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByDriverID(int DriverID)
        {
            List<LicenseBriefOutputDTO> list = await License.getAllBriefByDriverID(DriverID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full/by/DriverID/{DriverID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LicenseFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByDriverID(int DriverID)
        {
            List<LicenseFullOutputDTO> list = await License.getAllFullByDriverID(DriverID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-brief/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<LicenseBriefOutputDTO> list = await License.getAllBriefByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-full/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<LicenseFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<LicenseFullOutputDTO> list = await License.getAllFullByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

    }
}
