using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XmlSigner.Data;
using XmlSigner.Data.Models;
using XmlSigner.Library;

namespace XmlSigner.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class XmlFilesController : ControllerBase
    {
        private UserManager<IdentityUser<long>> _userManager;
        private readonly ApplicationDbContext _context;

        public XmlFilesController(ApplicationDbContext context, UserManager<IdentityUser<long>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/XmlFiles/verify
        [HttpPost("verify")]
        public async Task<ActionResult<List<X509Certificate2>>> VerifyXmlFile([FromForm]IFormFile xmlFile)    //XmlFile xmlFile
        {
            List<X509Certificate2> signerCertificateList = new List<X509Certificate2>();

            if (xmlFile.Length > 0)
            {
                XmlDocument xmlDocument = new XmlDocument();
                try {
                    xmlDocument.LoadXml(await Adapter.ReadAsStringAsync(xmlFile));
                    if (XmlSign.CheckIfDocumentPreviouslySigned(xmlDocument))
                    {
                        return XmlSign.VerifyAllSign(xmlDocument);
                    }
                    else
                    {
                        return BadRequest("Uploaded file has no sign");
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest("The file is compromised");
                }
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
        }

        // POST: api/XmlFiles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<long>> UploadXmlFile([FromForm]IFormFile xmlFile, [FromForm]long previousFileId, [FromForm]string token)    //XmlFile xmlFile
        {
            XmlFile uploadedFile = new XmlFile();
            if (previousFileId == 0)
            {
                return BadRequest("Previous File ID not Given");
            }
            else
            {
                DownloadUploadToken downloadUploadToken = await _context.DownloadUploadTokens
                                                    .Where(dut => dut.IsUsed == false)
                                                    .Where(xml => xml.Token == token)
                                                    .Where(xml => xml.ExpirityTime >= DateTime.UtcNow)
                                                    .Where(xml => xml.TableName >= TableName.XmlFile)
                                                    .FirstOrDefaultAsync();
                if (downloadUploadToken == null)
                {
                    return BadRequest("Token Invalid");
                }
                uploadedFile.PreviousSignedFile = await _context.XmlFiles.FindAsync(downloadUploadToken.DbEntryId);
                if(uploadedFile.PreviousSignedFile == null)
                {
                    return BadRequest("File Not Exists!!");
                }
                downloadUploadToken.MarkAsUsed();
                _context.DownloadUploadTokens.Update(downloadUploadToken);
            }

            if (xmlFile.Length > 0)
            {
                uploadedFile.FileContent = await Adapter.ReadAsStringAsync(xmlFile);
                uploadedFile.FileRealName = xmlFile.FileName;
                uploadedFile.Signer = await _userManager.GetUserAsync(User);
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
            _context.XmlFiles.Add(uploadedFile);
            await _context.SaveChangesAsync();
            return uploadedFile.Id;
        }

        // GET: api/XmlFiles/token/9
        [Authorize]
        [HttpPost("token")]
        public async Task<ActionResult<string>> GetXmlFileDownloadToken(string signReason, long verificationStep, long id)
        {
            //Should add token verification
            XmlFile xmlFile = await _context.XmlFiles.FindAsync(id);
            if (xmlFile == null)
            {
                return BadRequest("File Not Exists");
            }
            DownloadUploadToken downloadUploadToken = new DownloadUploadToken(signReason, verificationStep, TableName.XmlFile, id);

            _context.DownloadUploadTokens.Add(downloadUploadToken);
            await _context.SaveChangesAsync();

            return downloadUploadToken.Token;
        }

        // GET: api/XmlFiles/asdasd234/9
        [HttpGet("{token}/{id}")]
        public async Task<IActionResult> DownloadXmlFile(long id, string token)
        {
            DownloadUploadToken downloadUploadToken = await _context.DownloadUploadTokens
                                                    .Where(dut => dut.IsUsed == false)
                                                    .Where(xml => xml.Token == token)
                                                    .Where(xml => xml.ExpirityTime >= DateTime.UtcNow)
                                                    .Where(xml => xml.TableName >= TableName.XmlFile)
                                                    .Where(dut => dut.DbEntryId == id)
                                                    .FirstOrDefaultAsync();
            if (downloadUploadToken == null)
            {
                return BadRequest("Token Invalid");
            }
            XmlFile xmlFile = await _context.XmlFiles.FindAsync(downloadUploadToken.DbEntryId);
            if (xmlFile == null)
            {
                return NoContent();
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(xmlFile.FileContent);
            MemoryStream fileStream = new MemoryStream(byteArray);
            return File(fileStream, "application/ocet-stream", xmlFile.FileRealName);
        }

        // GET: api/XmlFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<XmlFile>>> GetXmlFiles()
        {
            //Should use automapper - https://code-maze.com/automapper-net-core/
            return await _context.XmlFiles.ToListAsync();
        }

        // Post: api/XmlFiles/5
        [HttpPost("{id}")]
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
