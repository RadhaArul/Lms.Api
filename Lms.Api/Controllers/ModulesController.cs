#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

using System.Text.Json;
using Lms.Core.Repositories;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        const int MaxPageSize = 8;

        public ModulesController(IUnitOfWork unitofwork, IMapper mapper)
        {
            this.uow = unitofwork;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        //Sorting by Asc , Desc , Pagination

        public async Task<ActionResult<(IEnumerable<Module>, PaginationMetaData)>> 
            GetModule([FromQuery(Name = "Sorting_A_for_asc / D_for_desc")]
            string sort = "a",int PageNumber=1, int PageSize=4)
        {
            if (PageSize>MaxPageSize)
                PageSize = MaxPageSize;

            (var modules,int totalItemCount) = await uow.LmsRepo.GetAllModules(sort, PageNumber, PageSize);

            var paginationMetadata = new PaginationMetaData(totalItemCount, PageSize, PageNumber);


            Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));
            
            return Ok(mapper.Map<IEnumerable<ModuleGetDto>>(modules));
        }

        // GET: api/Modules/5
        [HttpGet("{title}")]
        //Filter by Title
        public async Task<ActionResult<IEnumerable<Module>>> GetAllModule(string title)
        {
            //var @module = await _context.Module.FindAsync(id);
            if (title == null)
                return NotFound();

            var moduleobj = await uow.LmsRepo.GetAllModules(title);
                

            if (moduleobj == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<ModuleGetDto>>(moduleobj));
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModulePostPutDto module)
        {
            var moduleobj = mapper.Map<Module>(module);
            if (id != moduleobj.Id)
            {
                return BadRequest();
            }

           await uow.LmsRepo.UpdateModule(moduleobj);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.LmsRepo.ModuleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }

            return NoContent();
        }

        // PartialPUT: api/Modules/5
        [HttpPatch("{moduleId}")]
        public async Task<IActionResult> PartialUpdateModule(int moduleId, JsonPatchDocument<Module> patchmodule)
        {
            var moduleobj = uow.LmsRepo.PartialUpdateModule(moduleId);

            patchmodule.ApplyTo(await moduleobj);
            await uow.CompleteAsync();
            return StatusCode(200);

        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModulePostPutDto module)
        {
            var moduleobj=mapper.Map<Module>(module);
            try {
                await uow.LmsRepo.AddModule(moduleobj);
                await uow.CompleteAsync();

            return CreatedAtAction("GetModule", new { id = moduleobj.Id }, moduleobj);
            }
            catch {
                return StatusCode(500);
            }
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            await uow.LmsRepo.DeleteModule(id);
            

            
            await uow.CompleteAsync();

            return NoContent();
        }

       
    }
}
