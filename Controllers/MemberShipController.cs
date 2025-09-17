using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class MemberShipController : Controller
    {
        //private readonly IMemberShipRepository _memberShipRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MemberShipController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: MemberShipController
        // GET: MemberShipController
        public async Task<IActionResult> Index()
        {
            var memberships = await _unitOfWork.MemberShipRepository.GetAllAsync(
                include: q => q
                    .Include(m => m.Users)           // include users
                        .ThenInclude(u => u.Borrowings) // include borrowings
            );

            // Project into a view model with book count
            var result = memberships.Select(m => new
            {
                MemberShip = m,
                BooksCount = m.Users?
                    .SelectMany(u => u.Borrowings)
                    .Count(b => b.Status != Enums.BorrowingStatus.Returned) ?? 0
            }).ToList();

            ViewBag.MemberShipBookCounts = result.ToDictionary(x => x.MemberShip.Id, x => x.BooksCount);

            return View(memberships);
        }


        // GET: MemberShipController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var memberShip = await _unitOfWork.MemberShipRepository.GetByIdAsync(id);
            if(memberShip == null)
            {
                return NotFound();
            }
            return View(memberShip);
        }

        // GET: MemberShipController/Create
        // GET: MemberShip/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberShip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberShip model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.MemberShipRepository.AddAsync(model);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: MemberShipController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var memberShip = await _unitOfWork.MemberShipRepository.GetByIdAsync(id);
            if(memberShip == null)
            {
                return NotFound();
            }
            return View(memberShip);
        }

        // POST: MemberShipController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberShip model)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var memberShip = await _unitOfWork.MemberShipRepository.GetByIdAsync(id);
            if (ModelState.IsValid)
            {
                memberShip.FinePerDay = model.FinePerDay;
                memberShip.ExtraBooks = model.ExtraBooks;
                memberShip.ExtraDays = model.ExtraDays;
                memberShip.ExtraPenaltys = model.ExtraPenaltys;

                await _unitOfWork.MemberShipRepository.UpdateAsync(memberShip);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: MemberShipController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var memberShip = await _unitOfWork.MemberShipRepository.GetByIdAsync(id);
            return View(memberShip);
        }

        // POST: MemberShipController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var memberShip = await _unitOfWork.MemberShipRepository.GetByIdAsync(id);
            if(memberShip == null)
            {
                return NotFound();
            }
            await _unitOfWork.MemberShipRepository.DeleteAsync(memberShip);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
