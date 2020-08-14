using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XmlSigner.Data;
using XmlSigner.Data.Models;
using XmlSigner.ViewModels;
using XmlSigner.ViewModels.Mappers;

namespace XmlSigner.Controllers
{
    public class XmlFilesController : Controller
    {
        private UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public XmlFilesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: XmlFiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.XmlFiles
                                            .Where(ic => ic.DeletionTime == null)
                                            .Include(x => x.PreviousSignedFile).Include(x => x.Signer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: XmlFiles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            XmlFile xmlFile = await _context.XmlFiles
                            .Include(x => x.PreviousSignedFile)
                            .Include(x => x.Signer)
                            .FirstOrDefaultAsync(m => m.Id == id);
            if (xmlFile == null)
            {
                return NotFound();
            }

            return View(xmlFile);
        }

        // GET: XmlFiles/Create
        public IActionResult Create()
        {
            ViewData["PreviousFileId"] = new SelectList(_context.XmlFiles.Where(ic => ic.DeletionTime == null), "Id", "FileRealName",null);
            //ViewData["SignerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: XmlFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(XmlFileAddViewModel xmlFileAddViewModel)
        {
            if (ModelState.IsValid)
            {
                //Convert to xmlfiile model with mapper, then save the model to db
                XmlFile xmlFile = await XmlFileMapper.MapFromXmlFileAddViewModelAsync(xmlFileAddViewModel);
                if(xmlFile.PreviousFileId!=null)
                {
                    xmlFile.PreviousSignedFile = await _context.XmlFiles.FindAsync(xmlFile.PreviousFileId);
                }
                xmlFile.Signer = await _userManager.GetUserAsync(User);
                _context.XmlFiles.Add(xmlFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PreviousFileId"] = new SelectList(_context.XmlFiles, "Id", "FileContent", xmlFileAddViewModel.PreviousFileId);
            return View(xmlFileAddViewModel);
        }

        // GET: XmlFiles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmlFile = await _context.XmlFiles.FindAsync(id);
            if (xmlFile == null)
            {
                return NotFound();
            }
            ViewData["PreviousFileId"] = new SelectList(_context.XmlFiles, "Id", "FileRealName", xmlFile.PreviousFileId);
            ViewData["SignerId"] = new SelectList(_context.Users, "Id", "UserName", xmlFile.SignerId);
            return View(xmlFile);
        }

        // POST: XmlFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("FileContent,FileRealName,SignerId,PreviousFileId,Id,CreateTime,LastUpdateTime,DeletionTime")]
        public async Task<IActionResult> Edit(long id, [Bind("FileRealName,SignerId,PreviousFileId,Id,CreateTime,LastUpdateTime,DeletionTime")] XmlFile xmlFile)
        {
            if (id != xmlFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xmlFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!XmlFileExists(xmlFile.Id))
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
            ViewData["PreviousFileId"] = new SelectList(_context.XmlFiles, "Id", "FileContent", xmlFile.PreviousFileId);
            ViewData["SignerId"] = new SelectList(_context.Users, "Id", "Id", xmlFile.SignerId);
            return View(xmlFile);
        }

        // GET: XmlFiles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmlFile = await _context.XmlFiles
                .Include(x => x.PreviousSignedFile)
                .Include(x => x.Signer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xmlFile == null)
            {
                return NotFound();
            }

            return View(xmlFile);
        }

        // POST: XmlFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var xmlFile = await _context.XmlFiles.FindAsync(id);
            //_context.XmlFiles.Remove(xmlFile);
            xmlFile.SoftDelete();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool XmlFileExists(long id)
        {
            return _context.XmlFiles.Any(e => e.Id == id);
        }
    }
}
