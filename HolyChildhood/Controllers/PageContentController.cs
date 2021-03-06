﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HolyChildhood.Models;
using Microsoft.AspNetCore.Authorization;

namespace HolyChildhood.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PageContentController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public PageContentController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/PageContent/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PageContent>> GetPageContent(int id)
        {
            var pageContent = await dbContext.PageContents
                .Include(pc => pc.TextContent)
                .Include(pc => pc.TabContent).ThenInclude(tc => tc.Tabs).ThenInclude(t => t.TextContent)
                .Include(pc => pc.CalendarContent).ThenInclude(cc => cc.Calendar).ThenInclude(c => c.Events)
                .FirstOrDefaultAsync(pc => pc.Id == id);

            if (pageContent == null) return NotFound();

            return pageContent;
        }

        // PUT: api/PageContent/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutPageContent(int id, PageContent pageContent)
        {
            if (id != pageContent.Id) return BadRequest(); 

            dbContext.Entry(pageContent).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageContentExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/PageContent
        [HttpPost]
        [Authorize]
        [ProducesResponseType(201)]
        public async Task<ActionResult<PageContent>> PostPageContent(PageContent pageContent)
        {
            //var page = await dbContext.Pages.Include(p => p.PageContents).FirstOrDefaultAsync(p => p.Id == pageContent.Page.Id);
            //if (page == null) return NotFound();

            // Find max Y
            //var y = 0;
            //foreach (var content in page.PageContents)
            //{
            //    if (content.Y >= y) y = content.Y + 1;
            //}

            //pageContent.Page = page;
            //pageContent.Height = 1;
            //pageContent.Width = 4;
            //pageContent.X = 0;
            //pageContent.Y = y;

            dbContext.PageContents.AddAsync(pageContent);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetPageContent", new { id = pageContent.Id }, pageContent);
        }

        // DELETE: api/PageContent/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PageContent>> DeletePageContent(int id)
        {
            var pageContent = await dbContext.PageContents.FindAsync(id);
            if (pageContent == null) return NotFound();

            dbContext.PageContents.Remove(pageContent);
            await dbContext.SaveChangesAsync();

            return pageContent;
        }

        private bool PageContentExists(int id)
        {
            return dbContext.PageContents.Any(e => e.Id == id);
        }
    }
}
