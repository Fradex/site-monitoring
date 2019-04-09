using System;
using Microsoft.AspNetCore.Mvc;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Service.Services.Interfaces;

namespace SiteMonitoring.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckSiteController : Controller
    {
        protected IJobService JobService;
        public CheckSiteController(IJobService jobService)
        {
            this.JobService = jobService;
        }

        // POST api/values
        [HttpPost, Route("AddOrUpdateJob")]
        public ActionResult<SiteDTO> Post([FromBody] SiteDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return JobService.AddOrUpdateJob(model);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Не пердан идентификатор объекта.");
            }

            this.JobService.DeleteJob(id);
            return this.Ok();
        }
    }
}
