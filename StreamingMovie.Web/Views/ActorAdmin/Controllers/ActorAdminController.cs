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

        public ActorAdminController(SignInManager<User> signInManager, UserManager<User> userManager, IActorService actorServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _actorService = actorServices;
        }
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
        }
}
