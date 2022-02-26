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

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;

        public ModulesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        //Sorting by Asc , Desc
        public async Task<ActionResult<IEnumerable<Module>>> GetModule([FromQuery(Name = "Sorting By Title enter A for asc /D for desc")] string sort = "a")
        {
            var modules = mapper.ProjectTo<ModuleGetDto>(_context.Module);
            if (sort.ToUpper() == "A")
                modules = modules.OrderBy(x => x.Title);
             else
                modules=modules.OrderByDescending(x=>x.Title);
            return Ok(modules);
        }

        // GET: api/Modules/5
        [HttpGet("{title}")]
        public async Task<ActionResult<IEnumerable<Module>>> GetAllModule(string title)
        {
            //var @module = await _context.Module.FindAsync(id);
            if (title == null)
                return NotFound();

            var moduleobj = mapper.ProjectTo<ModuleGetDto>(_context.Module)
                .Where(c => c.Title == title);

            if (moduleobj == null)
            {
                return NotFound();
            }

            return Ok(moduleobj);
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

            _context.Entry(moduleobj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
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
            var moduleobj = await _context.Module.FindAsync(moduleId);

            // var course = mapper.Map<CoursePatchDto>(courseobj);
            patchmodule.ApplyTo(moduleobj);
            await _context.SaveChangesAsync();
            return StatusCode(200);

        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModulePostPutDto module)
        {
            var moduleobj=mapper.Map<Module>(module);
            try { 
            _context.Module.Add(moduleobj);
            await _context.SaveChangesAsync();

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
            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            _context.Module.Remove(@module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
