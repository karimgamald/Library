using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWrok)
        {
            _unitOfWork = unitOfWrok;
        }
        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
                include: q => q.Include(c => c.Books)
            );
            return View(categories);
        }


        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: CategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Description = model.Description
                };

                await _unitOfWork.CategoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: CategoryController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category); 
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if(id != model.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if(category == null)
                {
                    return NotFound();
                }
                category.Name = model.Name;
                category.Description = model.Description;

                // UpdateAsync() already has Calling savechanges()
                await _unitOfWork.CategoryRepository.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: CategoryController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            await _unitOfWork.CategoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
