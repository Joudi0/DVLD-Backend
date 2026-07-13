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
    public class ApplicationTypeController : ControllerBase
    {

        /// <summary>
        /// Retrieves a record by its ApplicationTypeID wrapping it in a comprehensive FullOutputDTO containing nested entities.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(ApplicationTypeFullOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {            ApplicationTypeFullOutputDTO result = await ApplicationType.getApplicationTypeByID(id);
            if (result == null) return NotFound($"ApplicationType with ApplicationTypeID {id} not found.");
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in clean BriefOutputDTOs.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-brief")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationTypeBriefOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrief()
        {
            List<ApplicationTypeBriefOutputDTO> list = await ApplicationType.getAllBrief();
            return Ok(list);
        }

        /// <summary>
        /// Retrieves all matching records filtered by a specific column wrapping them in heavy FullOutputDTOs with nested composition.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-full")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.ReadPolicy)]
        [ProducesResponseType(typeof(List<ApplicationTypeFullOutputDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            List<ApplicationTypeFullOutputDTO> list = await ApplicationType.getAllFull();
            return Ok(list);
        }

    }
}
