using AspCore_Identity.Model;
using AspCore_Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore_Identity.Controllers
{
    //oasisoft.net
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityController(UserManager<IdentityUser> userManager,IEmailSender emailSender,SignInManager<IdentityUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public  IActionResult SignUp()
        {
            var model = new SignupVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignupVM model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(model.UserName)==null)
                {
                    var user = new IdentityUser
                    {
                        Email = model.UserName,
                        UserName = model.UserName
                    };

                   var result= await _userManager.CreateAsync(user, model.Password);


                  
                    if (result.Succeeded)
                    {
                        //get it back from database to get generated  id

                        user = await _userManager.FindByEmailAsync(model.UserName);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var ConfirmationLink = Url.Action("ConfirmEmail", "Identity", new { userId = user.Id, @Token = token });
                        
                        await emailSender.SendEmailAddress("info@test.com", user.Email, "Confirm your Email Address", ConfirmationLink);

                      return  RedirectToAction("SignIn");
                    }

                    ModelState.AddModelError("SignUp", string.Join("", result.Errors.Select(z => z.Description)));
                }
            }
            return View(model);
        }









        public async Task<IActionResult> ConfirmEmail(string userId,string Token)
        {

            var user =await _userManager.FindByIdAsync(userId);

            var result = _userManager.ConfirmEmailAsync(user, Token);
            if (result.Result.Succeeded)
            {
                return RedirectToAction("SignIn");
            }
            return new NotFoundResult ();
        }



        public IActionResult SignUpWithoutEmail()
        {
            var model = new SignupNoEmailVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SignUpWithoutEmail(SignupNoEmailVM model)
        {
            if (ModelState.IsValid)
            {

                //check if role is exit and add if not 

                if (!(await roleManager.RoleExistsAsync(model.Role) ))
                {
                    var role = new IdentityRole { Name = model.Role };
                    var roleResult = roleManager.CreateAsync(role);

                    if (!roleResult.Result.Succeeded)
                    {
                        var errors = roleResult.Result.Errors.Select(a => a.Description);
                        ModelState.AddModelError("Role", string.Join(" ", errors));
                        return View(model);

                    }
                }



                if (await _userManager.FindByEmailAsync(model.UserName) == null)
                {
                    var user = new IdentityUser
                    {
                        Email = model.UserName,
                        UserName = model.UserName
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);



                    if (result.Succeeded)
                    {
                        //add user to role

                      await  _userManager.AddToRoleAsync(user, model.Role);
           

                        return RedirectToAction("SignIn");
                    }

                    ModelState.AddModelError("SignUp", string.Join("", result.Errors.Select(z => z.Description)));
                }
            }
            return View(model);
        }

















        public async Task<IActionResult> SignIn()
        {
           
            return View();
          
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM model)
        {
            if (ModelState.IsValid)
            {
               var result= await signInManager.PasswordSignInAsync(model.UserName, model.Password,model.RememberMe,false);


                if (result.Succeeded)
                {
                    //indentify user and and send user to page pased on his role

                    var user = await _userManager.FindByEmailAsync(model.UserName);

                    if (await _userManager.IsInRoleAsync(user,"Member"))
                    {
                        return RedirectToAction("Index");
                    }


                    return RedirectToAction(actionName: "AccessDenied", controllerName: "Home");
                }
            }
            else
            {
                ModelState.AddModelError("Login", "Cannot Login");
            }
            return View(model);

        }
        public async Task<IActionResult> AccessDenied()
        {

            return View();
        }
    }
}
