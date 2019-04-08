using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Model.Model.Requests;
using SiteMonitoring.Repositories.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SiteMonitoring.Controllers
{
    [Route("api/[controller]")]
    public class SiteController : Controller
    {
        protected ISiteRepository SiteRepository;
        public SiteController(ISiteRepository siteRepository)
        {
            SiteRepository = siteRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<SiteDTO>> GetAll()
        {
            return await SiteRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Site>> GetByIdAsync(Guid id)
        {
            var site = await SiteRepository.GetAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return site;
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult<Site>> CreateAsync([FromBody] SiteRequestModel site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await SiteRepository.SaveAsync(site);

            return CreatedAtAction(nameof(GetByIdAsync),
                new { id = site.Id }, site);
        }
    }
}
