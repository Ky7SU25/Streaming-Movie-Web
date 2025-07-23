using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Application.Interfaces.ExternalServices.VNpay;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Web.Views.Payment.ViewModels;

namespace StreamingMovie.Web.Views.Payment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayservice;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly IPaymentService _paymentService;

        public PaymentController(IVnPayService vnPayservice, UserManager<User> userManager, IPaymentService paymentService, SignInManager<User> signInManager)
        {
            _vnPayservice = vnPayservice;
            _userManager = userManager;
            _signInManager = signInManager;
            _paymentService = paymentService;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> PaymentChoice()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);

            var user = await _userManager.FindByIdAsync(userIdString); 
            if (user == null)
            {
                return NotFound();
            }

            var paymentViewModel = new PaymentViewModel
            {
                UserID = userId,
                UserFullName = user.FullName,
                ExpDate = user.SubscriptionEndDate ?? DateTime.Now.AddDays(-1)
            };

            return View(paymentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentConfirmation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            
                

            
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = 50000,
                    CreatedDate = DateTime.Now,
                    Description = "thanh toan nang cap tai khoan",
                    FullName = user.FullName,
                    OrderID = new Random().Next(1000, 100000)
                };
                
                return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            
        
        [Authorize]
        public async Task<IActionResult> PaymentRollBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["error"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("Error404", "Home");
            }
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString); // hoặc dùng TryParse như trên

            var user = await _userManager.FindByIdAsync(userIdString); // Identity thường dùng string ID
            if (user == null)
            {
                return NotFound();
            }


            var payment = new StreamingMovie.Domain.Entities.Payment
                {
                    UserId = userId,
                    PaymentCreateDate = DateTime.Now,
                    PaymentExpDate = DateTime.Now.AddDays(30), 
                    Amount = 50000,
                    Status = "Success"

                };
            await _paymentService.AddAsync(payment);

            user.SubscriptionType = "Premium";
            user.SubscriptionStartDate = DateTime.Now;
            user.SubscriptionEndDate = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);



            return RedirectToAction("Index", "Home");


        }
        public IActionResult ErrorPayment()
        {
            return View();
        }
    }
}
