using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XmlSigner.Data;
using XmlSigner.Data.Models;

namespace XmlSigner.Controllers.api
{
    //[Authorize]
    //[ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    [ApiController]
    public class DatatablesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DatatablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Datatables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveApplication>> GetLeaveApplication(long id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return leaveApplication;
        }

        // POST: api/Datatables/LeaveApplications
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Consumes("application/json")]
        [HttpPost("LeaveApplications")]
        public async Task<ActionResult<string>> PostLeaveApplication(FormCollection collection) //Task<ActionResult<string>>
        {
            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body, Encoding.UTF8))
            {
                body = await stream.ReadToEndAsync();
            }
            return body;
        }
    }
}
