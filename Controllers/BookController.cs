using Library.Data;
using Library.Enums;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        // GET: BookController
        // GET: BookController
        public async Task<IActionResult> Index()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync(
                include: q => q.Include(b => b.Category)
            );

            foreach (var book in books)
            {
                book.Status = book.AvailableCopies > 0 ? BookStatus.Available : BookStatus.Borrowed;
            }

            return View(books);
        }

        // GET: BookController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var book = await _unitOfWork.BookRepository.GetAsync(
                b => b.Id == id,
                include: q => q.Include(b => b.Category)
            );

            if (book == null) return NotFound();

            return View(book);
        }

        // GET: BookController/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // Handle Image Upload
                if (image != null && image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }

                var book = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    ISBN = model.ISBN,
                    PublishYear = model.PublishYear,
                    AvailableCopies = model.AvailableCopies,
                    ImageUrl = uniqueFileName != null ? "/images/" + uniqueFileName : null,
                    Status = model.AvailableCopies > 0 ? BookStatus.Available : BookStatus.Borrowed,
                    CategoryId = model.CategoryId
                };

                await _unitOfWork.BookRepository.AddAsync(book);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
            return View(model);
        }

        // GET: BookController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null) return NotFound();

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", book.CategoryId);

            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                string uniqueFileName = model.ImageUrl;

                // Update Image if uploaded
                if (model.Image != null && model.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }

                    uniqueFileName = "/images/" + uniqueFileName;
                }

                var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
                if (book == null) return NotFound();

                // Update fields
                book.Title = model.Title;
                book.Author = model.Author;
                book.PublishYear = model.PublishYear;
                book.AvailableCopies = model.AvailableCopies;
                book.Status = model.AvailableCopies > 0 ? BookStatus.Available : BookStatus.Borrowed;
                book.CategoryId = model.CategoryId;
                book.ImageUrl = uniqueFileName;

                await _unitOfWork.BookRepository.UpdateAsync(book);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
            return View(model);
        }

        // GET: BookController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null) return NotFound();

            await _unitOfWork.BookRepository.DeleteAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
