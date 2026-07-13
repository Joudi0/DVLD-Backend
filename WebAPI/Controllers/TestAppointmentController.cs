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
    public class TestAppointmentController : ControllerBase
    {

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(TestAppointmentFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] TestAppointmentBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await TestAppointment.addTestAppointment(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await TestAppointment.getTestAppointmentByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Retrieves a record by its TestAppointmentID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(TestAppointmentFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            TestAppointmentFullOutputDTO result = await TestAppointment.getTestAppointmentByID(id);
            if (result == null) return NotFound($"TestAppointment with TestAppointmentID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] TestAppointmentBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await TestAppointment.updateTestAppointment(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("TestAppointmentID/{TestAppointmentID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int TestAppointmentID)
        {
            bool isDeleted = await TestAppointment.deleteTestAppointment(TestAppointmentID);
            if (!isDeleted) return NotFound($"TestAppointment not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/TestAppointmentID/{TestAppointmentID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int TestAppointmentID)
        {      
            bool exists = await TestAppointment.isTestAppointmentExistByID(TestAppointmentID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<TestAppointmentBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "TestAppointmentID", [FromQuery] string direction = "ASC")
        {
            List<TestAppointmentBriefOutputDTO> list = await TestAppointment.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<TestAppointmentBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<TestAppointmentBriefOutputDTO> list = await TestAppointment.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<TestAppointmentFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<TestAppointmentFullOutputDTO> list = await TestAppointment.getAllFull();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves a record by its LocalDrivingLicenseApplicationID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("LocalDrivingLicenseApplicationID/{LocalDrivingLicenseApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(TestAppointmentFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {            TestAppointmentFullOutputDTO result = await TestAppointment.getTestAppointmentByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if (result == null) return NotFound($"TestAppointment with LocalDrivingLicenseApplicationID {LocalDrivingLicenseApplicationID} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a record by its RetakeTestApplicationID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("RetakeTestApplicationID/{RetakeTestApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(TestAppointmentFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByRetakeTestApplicationID(int? RetakeTestApplicationID)
        {            TestAppointmentFullOutputDTO result = await TestAppointment.getTestAppointmentByRetakeTestApplicationID(RetakeTestApplicationID);
            if (result == null) return NotFound($"TestAppointment with RetakeTestApplicationID {RetakeTestApplicationID} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/LocalDrivingLicenseApplicationID/{LocalDrivingLicenseApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {      
            bool exists = await TestAppointment.isTestAppointmentExistByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            return Ok(exists);
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/RetakeTestApplicationID/{RetakeTestApplicationID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByRetakeTestApplicationID(int? RetakeTestApplicationID)
        {      
            bool exists = await TestAppointment.isTestAppointmentExistByRetakeTestApplicationID(RetakeTestApplicationID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief/by/TestTypeID/{TestTypeID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<TestAppointmentBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByTestTypeID(int TestTypeID)
        {
            List<TestAppointmentBriefOutputDTO> list = await TestAppointment.getAllBriefByTestTypeID(TestTypeID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full/by/TestTypeID/{TestTypeID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<TestAppointmentFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByTestTypeID(int TestTypeID)
        {
            List<TestAppointmentFullOutputDTO> list = await TestAppointment.getAllFullByTestTypeID(TestTypeID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-brief/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<TestAppointmentBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<TestAppointmentBriefOutputDTO> list = await TestAppointment.getAllBriefByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-full/by/CreatedByUserID/{CreatedByUserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<TestAppointmentFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<TestAppointmentFullOutputDTO> list = await TestAppointment.getAllFullByCreatedByUserID(CreatedByUserID);
            return Ok(list);
        }

    }
}
