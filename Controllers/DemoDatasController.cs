using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XmlSigner.Data;
using XmlSigner.Data.Models;
using XmlSigner.Library;

namespace XmlSigner.Controllers
{
    public class DemoDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser<long>> _userManager;

        public DemoDatasController(ApplicationDbContext context, UserManager<IdentityUser<long>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DemoDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.DemoData.ToListAsync());
        }

        // GET: DemoDatas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demoData = await _context.DemoData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demoData == null)
            {
                return NotFound();
            }

            return View(demoData);
        }

        // GET: DemoDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DemoDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Organization,Department,Address,Phone,NID,Id,CreateTime,LastUpdateTime,DeletionTime")] DemoData demoData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(demoData);
                await _context.SaveChangesAsync();
                //Save the XML
                XmlFile convertedXmlFile = new XmlFile();
                convertedXmlFile.FileContent = Adapter.SerializeToXml(demoData).OuterXml;
                convertedXmlFile.FileRealName = Guid.NewGuid() + ".xml";
                convertedXmlFile.Signer = await _userManager.GetUserAsync(User);
                _context.XmlFiles.Add(convertedXmlFile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(demoData);
        }

        // GET: DemoDatas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demoData = await _context.DemoData.FindAsync(id);
            if (demoData == null)
            {
                return NotFound();
            }
            return View(demoData);
        }

        // POST: DemoDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Organization,Department,Address,Phone,NID,Id,CreateTime,LastUpdateTime,DeletionTime")] DemoData demoData)
        {
            if (id != demoData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demoData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemoDataExists(demoData.Id))
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
            return View(demoData);
        }

        // GET: DemoDatas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demoData = await _context.DemoData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demoData == null)
            {
                return NotFound();
            }

            return View(demoData);
        }

        // POST: DemoDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var demoData = await _context.DemoData.FindAsync(id);
            _context.DemoData.Remove(demoData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemoDataExists(long id)
        {
            return _context.DemoData.Any(e => e.Id == id);
        }
    }
}
