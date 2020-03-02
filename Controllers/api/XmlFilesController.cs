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

        // GET: api/XmlFiles/token/9
        [Authorize]
        [HttpGet("token/{id}")]
        public async Task<ActionResult<string>> GetXmlFileDownloadToken(long id)
        {
            //Should add token verification
            XmlFile xmlFile = await _context.XmlFiles.FindAsync(id);
            if (xmlFile == null)
            {
                return BadRequest("File Not Found");
            }
            DownloadUploadToken dut = new DownloadUploadToken("Reason", TableName.LeaveApplication, id);
            dut.Signer = await _userManager.GetUserAsync(User);
            /*if (xmlFile.DownloadUploadTokens == null)
            {
                xmlFile.DownloadUploadTokens = new List<DownloadUploadToken>();
            }
            xmlFile.DownloadUploadTokens.Add(dut);*/
            _context.DownloadUploadToken.Add(dut);
            _context.XmlFiles.Update(xmlFile);
            await _context.SaveChangesAsync();

            return dut.Token;
        }

        // GET: api/XmlFiles/asdasd234/9
        [HttpGet("{token}/{id}")]
        public async Task<IActionResult> DownloadXmlFile(long id, string token)
        {
            DownloadUploadToken dut = await _context.DownloadUploadToken
                                                .Where(d => d.IsUsed == false)
                                                .Where(d => d.TableName == TableName.LeaveApplication)
                                                .Where(d => d.ExpirityTime >= DateTime.UtcNow)
                                                .Where(d => d.DbEntryId == id)
                                                .Where(d => d.Token.Equals(token))
                                                .FirstOrDefaultAsync();

            if (dut == null)
            {
                return BadRequest("Token Not Found");
            }
            XmlFile xmlFile = await _context.XmlFiles.FindAsync(id);
            if (xmlFile == null)
            {
                return NoContent();
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(xmlFile.FileContent);
            MemoryStream fileStream = new MemoryStream(byteArray);
            return File(fileStream, "application/ocet-stream", xmlFile.FileRealName);
        }

        // POST: api/XmlFiles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<long>> UploadXmlFile([FromForm]IFormFile xmlFile, [FromForm]long? previousFileId, [FromForm]string token)    //XmlFile xmlFile
        {
            XmlFile uploadedFile = new XmlFile();
            DownloadUploadToken dut = await _context.DownloadUploadToken
                                            .Where(d => d.IsUsed == false)
                                            .Where(d => d.TableName == TableName.LeaveApplication)
                                            .Where(d => d.ExpirityTime >= DateTime.UtcNow)
                                            .Where(d => d.Token.Equals(token))
                                            //.Where(d => d.Signer == user)
                                            .FirstOrDefaultAsync();
            if (dut == null)
            {
                return BadRequest("Token Not Found");
            }
            dut.MarkAsUsed();
            _context.DownloadUploadToken.Update(dut);

            uploadedFile.PreviousSignedFile = await _context.XmlFiles.FindAsync(previousFileId);
            if(uploadedFile.PreviousSignedFile == null)
            {
                return BadRequest("Valid Token and previous file ID don't match");
            }
            if (xmlFile.Length > 0)
            {
                uploadedFile.Signer = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == dut.SignerId);
                uploadedFile.FileContent = await Adapter.ReadAsStringAsync(xmlFile);
                uploadedFile.FileRealName = xmlFile.FileName;
                uploadedFile.DbEntryId = uploadedFile.PreviousSignedFile.DbEntryId;
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
            _context.XmlFiles.Add(uploadedFile);
            LeaveApplication leaveApp = await _context.LeaveApplication.FindAsync(uploadedFile.DbEntryId);
            if(leaveApp == null)
            {
                return BadRequest("A file Should be Uploaded");
            }
            await _context.SaveChangesAsync();
            leaveApp.LastSignedId = uploadedFile.Id;
            leaveApp.PreviousSignedFile = uploadedFile;
            _context.LeaveApplication.Update(leaveApp);
            await _context.SaveChangesAsync();
            return uploadedFile.Id;
        }

        // POST: api/XmlFiles
        [HttpPost, Route("api/controller/search")]
        public Task<ActionResult<bool>> UpdateApplicationStatus([FromForm]string LeaveFormId, [FromForm]ApplicationStatus Status)
        {
            if(ApplicationStatus.Approved == Status)
            {
                return null;
            }
            return null;
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

        // POST: api/XmlFiles/verify
        [HttpPost("verify")]
        public async Task<ActionResult<List<X509Certificate2>>> VerifyXmlFile([FromForm]IFormFile xmlFile)    //XmlFile xmlFile
        {
            List<X509Certificate2> signerCertificateList = new List<X509Certificate2>();

            if (xmlFile.Length > 0)
            {
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(await Adapter.ReadAsStringAsync(xmlFile));
                    if (XmlSign.CheckIfDocumentPreviouslySigned(xmlDocument))
                    {
                        var a =   XmlSign.GetAllSign(xmlDocument);
                        return XmlSign.VerifyAllSign(xmlDocument);
                    }
                    else
                    {
                        return BadRequest("Uploaded file has no sign");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("The file is compromised - " + ex.ToString());
                }
            }
            else
            {
                return BadRequest("A file Should be Uploaded");
            }
        }
    }
}
