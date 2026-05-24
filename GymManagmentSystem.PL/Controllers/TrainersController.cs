using GymManagmentSystem.BLL.ViewModels.TrainerViewModels;
using GymManagmentSystem.DAL.DbContexts;
using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly GymDbContext _context;

        public TrainersController(GymDbContext context)
        {
            _context = context;
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers
                .Select(t => new TrainerViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    Phone = t.Phone,
                    DateOfBirth = t.DateOfBirth,
                    Address = $"{t.Address.BuildingNumber}, {t.Address.Street}, {t.Address.City}",
                    Specialties = t.Speciality
                })
                .ToListAsync();

            return View(trainers);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _context.Trainers
                .Where(t => t.Id == id)
                .Select(t => new TrainerViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Email = t.Email,
                    Phone = t.Phone,
                    DateOfBirth = t.DateOfBirth,
                    Address = $"{t.Address.BuildingNumber}, {t.Address.Street}, {t.Address.City}",
                    Specialties = t.Speciality
                })
                .FirstOrDefaultAsync();

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Speciality = model.Specialties,

                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                }
            };

            await _context.Trainers.AddAsync(trainer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Trainer created successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            var model = new TrainerToUpdateViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Speciality
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
                return RedirectToAction(nameof(Index));

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Speciality = model.Specialties;

            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;

            _context.Trainers.Update(trainer);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Trainer updated successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            _context.Trainers.Remove(trainer);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Trainer deleted successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
