using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using XmlSigner.Data;
using XmlSigner.Data.Models;
using XmlSigner.Library;

namespace XmlSigner.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class XmlFilesController : ControllerBase
    {
        /*private readonly IWebHostEnvironment _env;
        private UserManager<IdentityUser<long>> _userManager;

        public XmlFilesController(IWebHostEnvironment env, UserManager<IdentityUser<long>> userManager)
        {
            _env = env;
            _userManager = userManager;
        }*/

        private readonly ApplicationDbContext _context;

        public XmlFilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/XmlFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<XmlFile>>> GetXmlFiles()
        {
            return await _context.XmlFiles.ToListAsync();
        }

        // GET: api/XmlFiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<XmlFile>> GetXmlFile(long id)
        {
            var xmlFile = await _context.XmlFiles.FindAsync(id);

            if (xmlFile == null)
            {
                return NotFound();
            }

            return xmlFile;
        }

        // PUT: api/XmlFiles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutXmlFile(long id, XmlFile xmlFile)
        {
            if (id != xmlFile.Id)
            {
                return BadRequest();
            }

            _context.Entry(xmlFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!XmlFileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/XmlFiles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<long>> UploadXmlFile([FromForm]long? previousFileId, [FromForm]IFormFile xmlFile)    //XmlFile xmlFile
        {
            //string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            /*string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;*/
            XmlFile uploadedFile = new XmlFile();
            if(previousFileId != null)
            {
                uploadedFile.PreviousSignedFile = await _context.XmlFiles.FindAsync(previousFileId);
            }
            if (xmlFile.Length > 0)
            {
                uploadedFile.FileContent = await StringConverter.ReadAsStringAsync(xmlFile);
                uploadedFile.FileRealName = xmlFile.FileName;
                //uploadedFile.Signer = await _userManager.GetUserAsync(User);
                /*using (FileStream fileStream = new FileStream(Path.Combine(uploadFolder, xmlFile.FileName), FileMode.Create))
                {
                    await xmlFile.CopyToAsync(fileStream);
                }*/
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
            _context.XmlFiles.Add(uploadedFile);
            await _context.SaveChangesAsync();
            return uploadedFile.Id;
        }

        // DELETE: api/XmlFiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<XmlFile>> DeleteXmlFile(long id)
        {
            var xmlFile = await _context.XmlFiles.FindAsync(id);
            if (xmlFile == null)
            {
                return NotFound();
            }

            _context.XmlFiles.Remove(xmlFile);
            await _context.SaveChangesAsync();

            return xmlFile;
        }

        private bool XmlFileExists(long id)
        {
            return _context.XmlFiles.Any(e => e.Id == id);
        }
    }
}
