using Microsoft.AspNetCore.Mvc;
using StudentManagementWebAPI.Models;
using StudentManagementWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementWebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PassportsController : ControllerBase
    {
        private readonly IPassportService _passportService;

        public PassportsController(IPassportService passportService)
        {
            _passportService = passportService ?? throw new ArgumentNullException(nameof(passportService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passport>>> GetPassports()
        {
            var passports = await _passportService.GetPassports();
            return Ok(passports);
        }

        [HttpGet("{passportId}")]
        public async Task<ActionResult<Passport>> GetPassport(int passportId)
        {
            var passport = await _passportService.GetPassportById(passportId);
            if (passport == null)
            {
                return NotFound();
            }
            return passport;
        }

        [HttpPost]
        public async Task<ActionResult<Passport>> PostPassport(Passport passport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPassport = await _passportService.CreatePassport(passport);
            return CreatedAtAction(nameof(GetPassport), new { passportId = createdPassport.PassportId }, createdPassport);
        }

        [HttpPut("{passportId}")]
        public async Task<IActionResult> PutPassport(int passportId, Passport updatedPassport)
        {
            if (passportId != updatedPassport.PassportId)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _passportService.UpdatePassport(passportId, updatedPassport);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{passportId}")]
        public async Task<IActionResult> DeletePassport(int passportId)
        {
            var success = await _passportService.DeletePassport(passportId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
