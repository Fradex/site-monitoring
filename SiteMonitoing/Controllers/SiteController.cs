using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteMonitoring.Model.Model;
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

        /// <summary>
        /// Получить все записи
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<SiteDTO>> GetAllAsync()
        {
            return await SiteRepository.GetAllAsync();
        }

        /// <summary>
        /// Получить запись по ИД
        /// </summary>
        [AllowAnonymous]
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

        /// <summary>
        /// Сохранить запись
        /// </summary>
        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult<Site>> SaveAsync([FromBody] SiteDTO site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await SiteRepository.SaveAsync(site);
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null || id.Equals(Guid.Empty))
            {
                return BadRequest("Не передан идентификатор объекта.");
            }

            await SiteRepository.DeleteAsync(id.Value);
            return this.Ok();
        }

        /// <summary>
        /// Обновить статус сайта и идентификатор джобы
        /// </summary>
        [HttpPost, Route("UpdateStatus")]
        public async Task<ActionResult> UpdateStatus([FromBody]SiteDTO site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await SiteRepository.UpdateStatus(site);

            return this.Ok();
        }
    }
}
