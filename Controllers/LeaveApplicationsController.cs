using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XmlSigner.Data;
using XmlSigner.Data.Models;
using XmlSigner.Library;
using XmlSigner.ViewModels;

namespace XmlSigner.Controllers
{
    [Authorize]
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser<long>> _userManager;

        public LeaveApplicationsController(ApplicationDbContext context, UserManager<IdentityUser<long>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeaveApplications.ToListAsync());
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                                    .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Designation,LeaveStart,LeaveEnd,LeaveType,PurposeOfLeave,AddressDuringLeave,PhoneNoDuringLeave")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                IdentityUser<long> currentUser = await _userManager.GetUserAsync(User);
                if(currentUser != null)
                {
                    leaveApplication.ApplicantId = currentUser.Id;
                }
                _context.LeaveApplications.Add(leaveApplication);
                await _context.SaveChangesAsync();
                XmlFile convertedXmlFile = new XmlFile(
                                                        Adapter.SerializeToXml(leaveApplication).OuterXml,
                                                        Guid.NewGuid() + ".xml",
                                                        leaveApplication.Id,
                                                        TableName.LeaveApplication
                                                      );
                var user = await _userManager.GetUserAsync(User);
                if(user != null)
                {
                    convertedXmlFile.Signer = user;
                    convertedXmlFile.SignerId = user.Id;
                }
                _context.XmlFiles.Add(convertedXmlFile);
                await _context.SaveChangesAsync();
                leaveApplication.PreviousSignedFile = convertedXmlFile;
                leaveApplication.LastSignedId = convertedXmlFile.Id;
                _context.LeaveApplications.Update(leaveApplication);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Sign), "LeaveApplications"
                                            , new {
                                                    id = leaveApplication.Id
                                            });
            }
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Sign/5
        public async Task<IActionResult> Sign(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            LeaveApplication leaveApp = await _context.LeaveApplications.FindAsync(id);
            XmlFile xmlFile = await _context.XmlFiles.FindAsync(leaveApp.LastSignedId);
            if (xmlFile == null)
            {
                return NotFound();
            }
            ApplicationSignViewModel asv = new ApplicationSignViewModel(xmlFile);
            await asv.UpdateStatusFromDatabase(_context);
            return View(asv);
        }

        // GET: LeaveApplications/ApproveList
        public async Task<IActionResult> ApproveList()
        {
            return View(
                    await _context.LeaveApplications
                            .Where(d => d.ApplicationStatus == ApplicationStatus.Applied)
                            .OrderBy(d=> d.Id)
                            .ToListAsync()
                );
        }

        // GET: LeaveApplications/ApproveList
        public async Task<IActionResult> Approved()
        {
            return View(
                    await _context.LeaveApplications
                            .Where(d => d.ApplicationStatus == ApplicationStatus.Approved)
                            .OrderBy(d => d.Id)
                            .ToListAsync()
                );
        }

        // GET: LeaveApplications/Sign/5
        public async Task<IActionResult> Approve(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            LeaveApplication leaveApp = await _context.LeaveApplications.FindAsync(id);
            XmlFile xmlFile = await _context.XmlFiles.FindAsync(leaveApp.LastSignedId);
            if (xmlFile == null)
            {
                return NotFound();
            }
            try
            {
                ApplicationSignViewModel asv = new ApplicationSignViewModel(xmlFile);
                await asv.UpdateStatusFromDatabase(_context);
                if(asv.CertificateList == null)
                {
                    return View();
                }
                return View(asv);
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            /*
            var a = Adapter.SerializeToXml(leaveApplication);
            var b = Adapter.DeSerializeFromXml<LeaveApplication>(a);
            return View(b);
            */
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ApplicationStatus")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            _context.LeaveApplications.Remove(leaveApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(long id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }
    }
}
