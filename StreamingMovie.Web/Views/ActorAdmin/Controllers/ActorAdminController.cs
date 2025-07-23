using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Services;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Web.Views.ActorAdmin.ViewModels;

namespace StreamingMovie.Web.Views.ActorAdmin.Controllers
{
    public class ActorAdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IActorService _actorService;
        private readonly ICategoryService _categoryService;
        private readonly IDirectorService _directorService;

        public ActorAdminController(SignInManager<User> signInManager, UserManager<User> userManager, IActorService actorServices, ICategoryService categoryServices, IDirectorService directorService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _actorService = actorServices;
            _categoryService = categoryServices;
            _directorService = directorService; 
        }

        #region Actor Management
        public async Task<IActionResult> ActorList()
        {
            var actors = await _actorService.GetAllAsync();
            if (actors == null || !actors.Any())
            {
                return NotFound("No actors found.");
            }
            return View(actors);
        }
        [HttpGet]
        public async Task<IActionResult> EditActor(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            if (actor == null)
            {
                return NotFound($"Actor with ID {id} not found.");
            }
            var model = new ActorDto
            {
                Id = actor.Id,
                Fullname = actor.Name,
                Biography = actor.Biography,
                Dob = actor.DateOfBirth ?? DateTime.Now,
                Nationality = actor.Nationality
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditActor(ActorDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Trả lại view nếu model sai
            }

            // Tìm actor hiện tại từ DB
            var existingActor = await _actorService.GetByIdAsync(model.Id);
            if (existingActor == null)
            {
                return NotFound();
            }

            // Gán lại thông tin từ DTO sang Entity
            existingActor.Name = model.Fullname;
            existingActor.Biography = model.Biography;
            existingActor.DateOfBirth = model.Dob;
            existingActor.Nationality = model.Nationality;

            if (model.ImgProfile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImgProfile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImgProfile.CopyToAsync(stream);
                }

                existingActor.AvatarUrl = "/images/" + fileName; // Lưu URL tương đối vào DB
            }


            await _actorService.UpdateAsync(existingActor);

            return RedirectToAction("ActorList");
        }
        [HttpGet]
        public IActionResult AddActor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddActor(ActorDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newActor = new Actor
            {
                Name = model.Fullname,
                Biography = model.Biography,
                DateOfBirth = model.Dob,
                Nationality = model.Nationality
            };

            // Nếu có ảnh thì lưu ảnh
            if (model.ImgProfile != null)
            {
                using var memoryStream = new MemoryStream();
                await model.ImgProfile.CopyToAsync(memoryStream);
                newActor.AvatarUrl = Convert.ToBase64String(memoryStream.ToArray()); // hoặc lưu lên host và lấy link
            }

            await _actorService.AddAsync(newActor); // đảm bảo service đã có AddAsync
            return RedirectToAction("ActorList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            if (actor == null)
            {
                return NotFound($"Actor with ID {id} not found.");
            }
            var result = await _actorService.DeleteAsync(actor.Id);
            if (!result)
            {
                return BadRequest("Failed to delete actor.");
            }
            return RedirectToAction("ActorList");
        }
        #endregion

        #region Category Management
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _categoryService.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            var model = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Trả lại view nếu model sai
            }
            // Tìm category hiện tại từ DB
            var existingCategory = await _categoryService.GetByIdAsync(model.Id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            // Gán lại thông tin từ DTO sang Entity
            existingCategory.Name = model.Name;
            existingCategory.Slug = model.Slug;
            existingCategory.Description = model.Description;
            existingCategory.CreatedAt = DateTime.Now; 
            await _categoryService.UpdateAsync(existingCategory);
            return RedirectToAction("CategoryList");
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newCategory = new Domain.Entities.Category
            // Add the correct using directive for the Category entity at the top of the file

            {
                Name = model.Name,
                Slug = model.Slug,
                Description = model.Description
            };
            await _categoryService.AddAsync(newCategory); // đảm bảo service đã có AddAsync
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            var result = await _categoryService.DeleteAsync(category.Id);
            if (!result)
            {
                return BadRequest("Failed to delete category.");
            }
            return RedirectToAction("CategoryList");
        }
        #endregion

        #region Director Management
        public async Task<IActionResult> DirectorList()
        {
            var directors = await _directorService.GetAllAsync();
            if (directors == null || !directors.Any())
            {
                return NotFound("No directors found.");
            }
            return View(directors);
        }
        [HttpGet]
        public async Task<IActionResult> EditDirector(int id)
        {
            var directors = await _directorService.GetByIdAsync(id);
            if (directors == null)
            {
                return NotFound($"Director with ID {id} not found.");
            }
            var model = new DirectorDto
            {
                Id = directors.Id,
                Fullname = directors.Name,
                Biography = directors.Biography,
                Dob = directors.DateOfBirth ?? DateTime.Now,
                Nationality = directors.Nationality
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditDirector(DirectorDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Trả lại view nếu model sai
            }
            // Tìm director hiện tại từ DB
            var existingDirector = await _directorService.GetByIdAsync(model.Id);
            if (existingDirector == null)
            {
                return NotFound();
            }
            // Gán lại thông tin từ DTO sang Entity
            existingDirector.Name = model.Fullname;
            existingDirector.Biography = model.Biography;
            existingDirector.DateOfBirth = model.Dob;
            existingDirector.Nationality = model.Nationality;
            if (model.ImgProfile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImgProfile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImgProfile.CopyToAsync(stream);
                }
                existingDirector.AvatarUrl = "/images/" + fileName; // Lưu URL tương đối vào DB
            }
            await _directorService.UpdateAsync(existingDirector);
            return RedirectToAction("DirectorList");
        }
            
        [HttpGet]
        public IActionResult AddDirector()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDirector(DirectorDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newdirector = new Director
            {
                Name = model.Fullname,
                Biography = model.Biography,
                DateOfBirth = model.Dob,
                Nationality = model.Nationality
            };

            // Nếu có ảnh thì lưu ảnh
            if (model.ImgProfile != null)
            {
                using var memoryStream = new MemoryStream();
                await model.ImgProfile.CopyToAsync(memoryStream);
                newdirector.AvatarUrl = Convert.ToBase64String(memoryStream.ToArray()); // hoặc lưu lên host và lấy link
            }

            await _directorService.AddAsync(newdirector); // đảm bảo service đã có AddAsync
            return RedirectToAction("DirectorList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var actor = await _directorService.GetByIdAsync(id);
            if (actor == null)
            {
                return NotFound($"Director with ID {id} not found.");
            }
            var result = await _directorService.DeleteAsync(actor.Id);
            if (!result)
            {
                return BadRequest("Failed to delete director.");
            }
            return RedirectToAction("DirectorList");
        }
        #endregion  

    }
}
