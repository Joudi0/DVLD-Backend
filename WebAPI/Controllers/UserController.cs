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
    public class UserController : ControllerBase
    {

        /// <summary>
        /// Adds a new record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(UserFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] UserBriefInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data payload.");
            int insertedID = await clsUser.addUser(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await clsUser.getUserByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Admin-only: Adds a new record using FullInputDTO to define system-level settings.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPost("admin")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(UserFullOutputDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddAdmin([FromBody] UserFullInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            
            int insertedID = await clsUser.addAdminUser(dto);
            if (insertedID == -1) return StatusCode(500, "Error adding record.");
            
            var newRecord = await clsUser.getUserByID(insertedID);
            return CreatedAtAction("GetByID", new { id = insertedID }, newRecord);
        }

        /// <summary>
        /// Retrieves a record by its UserID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(UserFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id, [FromServices] IAuthorizationService authorizationService)
        {
            // Centralized Policy-Based Resource Authorization Check
            Microsoft.AspNetCore.Authorization.AuthorizationResult authResult = await authorizationService.AuthorizeAsync(User, id, "UserOwnerOrAdmin");
            if (!authResult.Succeeded) return Forbid();
            UserFullOutputDTO result = await clsUser.getUserByID(id);
            if (result == null) return NotFound($"User with UserID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a record by its UserName wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("UserName/{UserName}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(UserFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserName(string UserName)
        {            UserFullOutputDTO result = await clsUser.getUserByUserName(UserName);
            if (result == null) return NotFound($"User with UserName {UserName} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Updates a record using BriefInputDTO (Standard User Access).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> Update([FromBody] UserBriefInputDTO dto, [FromServices] IAuthorizationService authorizationService)
        {
            if (dto == null) return BadRequest("Invalid data.");
            // Add ownership check here if isUser is true
            bool isUpdated = await clsUser.updateUser(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Updates a record using FullInputDTO (Admin Access - Full Control).
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut("admin")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        public async Task<IActionResult> UpdateAdmin([FromBody] UserFullInputDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");
            
            bool isUpdated = await clsUser.updateUser(dto);
            if (!isUpdated) return NotFound();
            return Ok("Updated successfully.");
        }

        /// <summary>
        /// Deletes a specific record using its unique identifier.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("UserID/{UserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.WritePolicy)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int UserID, [FromServices] IAuthorizationService authorizationService)
        {

            // Centralized Policy-Based Resource Authorization Check
            Microsoft.AspNetCore.Authorization.AuthorizationResult authResult = await authorizationService.AuthorizeAsync(User, UserID, "UserOwnerOrAdmin");
            if (!authResult.Succeeded) return Forbid();
            bool isDeleted = await clsUser.deleteUser(UserID);
            if (!isDeleted) return NotFound($"User not found or couldn't be deleted.");
            return Ok("Deleted successfully.");
        }

        /// <summary>
        /// Checks whether a record exists based on the provided criteria.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("exists/UserID/{UserID}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExistsByID(int UserID, [FromServices] IAuthorizationService authorizationService)
        {
            // Centralized Policy-Based Resource Authorization Check
            Microsoft.AspNetCore.Authorization.AuthorizationResult authResult = await authorizationService.AuthorizeAsync(User, UserID, "UserOwnerOrAdmin");
            if (!authResult.Succeeded) return Forbid();
      
            bool exists = await clsUser.isUserExistByID(UserID);
            return Ok(exists);
        }

        /// <summary>
        /// Retrieves a paginated list of records returning a optimized List of BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("page")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<UserBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] int rowsPerPage = 10, [FromQuery] int pageNumber = 1, [FromQuery] string sortColumn = "UserID", [FromQuery] string direction = "ASC")
        {
            List<UserBriefOutputDTO> list = await clsUser.Paging(rowsPerPage, pageNumber, sortColumn, direction);
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<UserBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<UserBriefOutputDTO> list = await clsUser.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<UserFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<UserFullOutputDTO> list = await clsUser.getAllFull();
            return Ok(list);
        }

    }
}
