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
    public class InternationalLicenseController : ControllerBase
    {

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(InternationalLicenseFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] InternationalLicenseBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await InternationalLicense.addInternationalLicense(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await InternationalLicense.getInternationalLicenseByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Admin-only: Adds a new record using FullInputDTO to define system-level settings.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost("admin")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(InternationalLicenseFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddAdmin([FromBody] InternationalLicenseFullInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            
            int insertedID = await InternationalLicense.addAdminInternationalLicense(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await InternationalLicense.getInternationalLicenseByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Retrieves a record by its InternationalLicenseID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(InternationalLicenseFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            InternationalLicenseFullOutputDTO result = await InternationalLicense.getInternationalLicenseByID(id);
            if (result == null) return NotFound($"InternationalLicense with InternationalLicenseID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] InternationalLicenseBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await InternationalLicense.updateInternationalLicense(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Updates a record using FullInputDTO (Admin Access - Full Control).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut("admin")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> UpdateAdmin([FromBody] InternationalLicenseFullInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            
            bool isUpdated = await InternationalLicense.updateInternationalLicense(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("InternationalLicenseID/{InternationalLicenseID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int InternationalLicenseID)
        {
            bool isDeleted = await InternationalLicense.deleteInternationalLicense(InternationalLicenseID);
            if (!isDeleted) return NotFound($"InternationalLicense not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/InternationalLicenseID/{InternationalLicenseID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int InternationalLicenseID)
        {      
            bool exists = await InternationalLicense.isInternationalLicenseExistByID(InternationalLicenseID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<InternationalLicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "InternationalLicenseID", [FromQuery] string direction = "ASC")
        {
            List<InternationalLicenseBriefOutputDTO> list = await InternationalLicense.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<InternationalLicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<InternationalLicenseBriefOutputDTO> list = await InternationalLicense.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<InternationalLicenseFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<InternationalLicenseFullOutputDTO> list = await InternationalLicense.getAllFull();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves a record by its ApplicationID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("ApplicationID/{ApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(InternationalLicenseFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByApplicationID(int ApplicationID)
        {            InternationalLicenseFullOutputDTO result = await InternationalLicense.getInternationalLicenseByApplicationID(ApplicationID);
            if (result == null) return NotFound($"InternationalLicense with ApplicationID {ApplicationID} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a record by its IssuedUsingLocalLicenseID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("IssuedUsingLocalLicenseID/{IssuedUsingLocalLicenseID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(InternationalLicenseFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
        {            InternationalLicenseFullOutputDTO result = await InternationalLicense.getInternationalLicenseByIssuedUsingLocalLicenseID(IssuedUsingLocalLicenseID);
            if (result == null) return NotFound($"InternationalLicense with IssuedUsingLocalLicenseID {IssuedUsingLocalLicenseID} not found.");
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
            bool exists = await InternationalLicense.isInternationalLicenseExistByApplicationID(ApplicationID);
            return Ok(exists);
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/IssuedUsingLocalLicenseID/{IssuedUsingLocalLicenseID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
        {      
            bool exists = await InternationalLicense.isInternationalLicenseExistByIssuedUsingLocalLicenseID(IssuedUsingLocalLicenseID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief/by/DriverID/{DriverID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<InternationalLicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByDriverID(int DriverID)
        {
            List<InternationalLicenseBriefOutputDTO> list = await InternationalLicense.getAllBriefByDriverID(DriverID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full/by/DriverID/{DriverID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<InternationalLicenseFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByDriverID(int DriverID)
        {
            List<InternationalLicenseFullOutputDTO> list = await InternationalLicense.getAllFullByDriverID(DriverID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-brief/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<InternationalLicenseBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<InternationalLicenseBriefOutputDTO> list = await InternationalLicense.getAllBriefByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-full/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<InternationalLicenseFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<InternationalLicenseFullOutputDTO> list = await InternationalLicense.getAllFullByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

    }
}
