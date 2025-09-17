using Library.Enums;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library.Data;
using Library.Enums;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BorrowingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: BorrowingController
        public async Task<IActionResult> Index()
        {
            // Include User, MemberShip, and Book
            var borrowings = await _unitOfWork.BorrowingRepository.GetAllAsync(
                include: q => q
                    .Include(b => b.User)
                        .ThenInclude(u => u.MemberShip)
                    .Include(b => b.Book)
            );

            bool updated = false;

            foreach (var b in borrowings)
            {
                if (b.Status != BorrowingStatus.Returned && DateTime.Now > b.DueDate)
                {
                    b.Status = BorrowingStatus.Overdue;

                    if (b.User?.MemberShip != null)
                    {
                        int overdueDays = (DateTime.Now - b.DueDate).Days;
                        b.TotalFines = overdueDays * b.User.MemberShip.FinePerDay;
                        updated = true;
                    }
                }
            }

            // ✅ Save only if we updated something
            if (updated)
            {
                await _unitOfWork.BorrowingRepository.UpdateRangeAsync(borrowings);
            }

            return View(borrowings);
        }



        // GET: BorrowingController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var borrowing = await _unitOfWork.BorrowingRepository.GetAsync(
                b => b.Id == id,
                include: q => q
                    .Include(b => b.User)
                        .ThenInclude(u => u.MemberShip)
                    .Include(b => b.Book) // optional
            );

            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }


        // BorrowingController.cs
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _unitOfWork.UserRepository.GetByUserAndPassAsync(username, password);

            if (user != null)
            {
                // Save in Session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FullName);

                return RedirectToAction("Create");
            }

            // If not found → redirect to Register
            return RedirectToAction("Create", "User");
        }


        // GET: BorrowingController/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var books = await _unitOfWork.BookRepository.GetAllAsync();
            ViewBag.Books = new SelectList(books, "Id", "Title");
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(BorrowingStatus)));

            var borrowing = new Borrowing
            {
                UserId = userId.Value
            };

            return View(borrowing);
        }

        // POST: BorrowingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Borrowing borrowing)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = await _unitOfWork.UserRepository.GetAsync(
                u => u.Id == userId.Value,
                include: q => q.Include(u => u.MemberShip)
                               .Include(u => u.Borrowings)
            );

            if (user == null) return RedirectToAction("Login");

            // 🚨 Guard: No membership
            if (user.MemberShip == null)
            {
                ModelState.AddModelError("", "User does not have an active membership.");
                return RedirectToAction("Index", "MemberShip");
            }
            // Get all books
            var books = await _unitOfWork.BookRepository.GetAllAsync();

            // ✅ Check overdue books
            var hasOverdue = user.Borrowings.Any(b =>
                b.Status != BorrowingStatus.Returned &&
                b.DueDate < DateTime.Now);

            if (hasOverdue)
            {
                ModelState.AddModelError("", "You cannot borrow a new book until you return your overdue books.");
                ViewBag.Books = new SelectList(books, "Id", "Title", borrowing.BookId);
                return View(borrowing);
            }

            // ✅ Check book availability
            var book = await _unitOfWork.BookRepository.GetByIdAsync(borrowing.BookId.Value);
            if (book == null || book.AvailableCopies <= 0)
            {
                ModelState.AddModelError("", $"The book \"{book?.Title ?? "Unknown"}\" is not available.");
                ViewBag.Books = new SelectList(books, "Id", "Title", borrowing.BookId);
                return View(borrowing);
            }

            if (ModelState.IsValid)
            {
                borrowing.UserId = userId.Value;
                borrowing.BorrowDate = DateTime.Now;   // ✅ Set borrow date as now
                borrowing.ReturnDate = null;           // ✅ Only set on return

                // ✅ Set due date based on membership
                if (user.MemberShip?.MemberShipType == MemberShipType.Normal)
                {
                    borrowing.DueDate = DateTime.Now.AddDays(user.MemberShip.ExtraDays);
                }
                else if (user.MemberShip?.MemberShipType == MemberShipType.Premium)
                {
                    borrowing.DueDate = DateTime.Now.AddDays(user.MemberShip.ExtraDays);
                }
                else
                {
                    borrowing.DueDate = DateTime.Now.AddDays(user.MemberShip.ExtraDays);
                }

                borrowing.TotalDays = (borrowing.DueDate - borrowing.BorrowDate).Days;

                // ✅ Reduce copies and update book status
                book.AvailableCopies--;
                book.Status = book.AvailableCopies > 0 ? BookStatus.Available : BookStatus.Borrowed;

                borrowing.TotalFines = 0;

                await _unitOfWork.BorrowingRepository.AddAsync(borrowing);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Books = new SelectList(books, "Id", "Title", borrowing.BookId);
            return View(borrowing);
        }


        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            var borrowing = await _unitOfWork.BorrowingRepository.GetAsync(
                b => b.Id == id,
                include: q => q.Include(u => u.Book)
                               .Include(u => u.User)
            );
            if (borrowing == null)
            {
                return NotFound();
            }

            borrowing.ReturnDate = DateTime.Now;
            borrowing.Status = BorrowingStatus.Returned;


            //Return book copies
            if (borrowing.Book != null)
            {
                borrowing.Book.AvailableCopies++;
                borrowing.Book.Status = BookStatus.Available;
                borrowing.TotalFines = 0;
            }

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //GET: BorrowingController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var borrowing = await _unitOfWork.BorrowingRepository.GetByIdAsync(id);

            if (borrowing == null) return NotFound();

            var books = await _unitOfWork.BookRepository.GetAllAsync();
            ViewBag.Books = new SelectList(books, "Id", "Title", borrowing.BookId);
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(BorrowingStatus)), borrowing.Status);

            return View(borrowing);
        }

        // POST: BorrowingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Borrowing model)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var borrowing = await _unitOfWork.BorrowingRepository.GetByIdAsync(id);
                if (borrowing == null) return NotFound();

                // Update only editable fields
                borrowing.DueDate = model.DueDate;
                borrowing.ReturnDate = model.ReturnDate;
                borrowing.Status = model.Status;
                borrowing.BookId = model.BookId;

                // Recalculate total days if needed
                borrowing.TotalDays = (borrowing.DueDate - borrowing.BorrowDate).Days;

                await _unitOfWork.BorrowingRepository.UpdateAsync(borrowing);

                return RedirectToAction(nameof(Index));
            }

            var books = await _unitOfWork.BorrowingRepository.GetAllAsync();
            await _unitOfWork.SaveChangesAsync();
            ViewBag.Books = new SelectList(books, "Id", "Title", model.BookId);
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(BorrowingStatus)), model.Status);
            return View(model);
        }

        // GET: Borrowing/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var borrowing = await _unitOfWork.BorrowingRepository.GetByIdAsync(id);

            if (borrowing == null) return NotFound();

            return View(borrowing);
        }

        // POST: Borrowing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowing = await _unitOfWork.BorrowingRepository.GetByIdAsync(id);
            if (borrowing != null)
            {
                await _unitOfWork.BorrowingRepository.DeleteAsync(borrowing);
            }
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
