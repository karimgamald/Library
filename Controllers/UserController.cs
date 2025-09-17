using Library.Data;
using Library.Enums;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync(
                include: q => q
                    .Include(u => u.Borrowings)
                        .ThenInclude(b => b.Book)
                    .Include(u => u.MemberShip)
            );

            bool updated = false;

            foreach (var user in users)
            {
                // A user is active if they have any borrowing that is not returned
                bool isActive = user.Borrowings.Any(b => b.Status != BorrowingStatus.Returned);

                if (user.Status != isActive)
                {
                    user.Status = isActive;
                    updated = true;
                }
            }

            if (updated)
            {
                await _unitOfWork.UserRepository.UpdateRangeAsync(users);
                await _unitOfWork.SaveChangesAsync();
            }

            return View(users);
        }


        // GET: User/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(
                u => u.Id == id,
                include: q => q
                    .Include(u => u.MemberShip)
                    .Include(u => u.Borrowings)
                        .ThenInclude(b => b.Book)
            );

            if (user == null) return NotFound();

            return View(user);
        }

        // GET: User/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.MemberShips = await _unitOfWork.MemberShipRepository.GetAllAsync();
            var user = new User
            {
                Status = true,
                MemberShipId = 1 // default Normal
            };
            return View(user);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User model)
        {
            if (ModelState.IsValid)
            {
                // ✅ Handle photo upload
                if (model.Photo != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await model.Photo.CopyToAsync(stream);

                    model.PhotoPath = "/images/users/" + fileName;
                }

                await _unitOfWork.UserRepository.AddAsync(model);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberShips = await _unitOfWork.MemberShipRepository.GetAllAsync();
            return View(model);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.MemberShips = await _unitOfWork.MemberShipRepository.GetAllAsync();
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User model, IFormFile? Photo)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null) return NotFound();

                // Update fields
                user.FullName = model.FullName;
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.ConfirmPassword = model.ConfirmPassword;
                user.Address = model.Address;
                user.Phone = model.Phone;
                user.Email = model.Email;
                user.Status = model.Status;
                user.BirthDate = model.BirthDate;
                user.MemberShipId = model.MemberShipId == 0 ? 1 : model.MemberShipId;

                // ✅ Handle photo re-upload
                if (Photo != null && Photo.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);
                    var filePath = Path.Combine("wwwroot/images/users", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Photo.CopyToAsync(stream);
                    }

                    user.PhotoPath = "/images/users/" + fileName;
                }

                await _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberShips = await _unitOfWork.MemberShipRepository.GetAllAsync();
            return View(model);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.PhotoPath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                await _unitOfWork.UserRepository.DeleteAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
