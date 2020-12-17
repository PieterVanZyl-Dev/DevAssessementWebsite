using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevAssessementWebsite.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography.X509Certificates;

namespace DevAssessementWebsite.Controllers
{
    public class AccountsController : Controller
    {
        private readonly DevAssessmentContext _context;

        public AccountsController(DevAssessmentContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(InputModel Input)
        {

            //return Json(new { status = true, message = "Login Successfull!" });

            if (ModelState.IsValid)
            {
                // Use Input.Email and Input.Password to authenticate the user
                // with your custom authentication logic.
                //
                // For demonstration purposes, the sample validates the user
                // on the email address maria.rodriguez@contoso.com with 
                // any password that passes model validation.

                var user = await AuthenticateUser(Input.Name, Input.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("Surname", user.Surname)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    IsPersistent = Input.RememberMe,
                };


                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                user.LastLogin = DateTime.Now;

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }catch
                {
                    ModelState.AddModelError(string.Empty, "Error Updating Last Login");
                    return View();
                }




                return RedirectToAction("Edit", new { id = user.Id } );

            } else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }


        }

        // GET: Accounts/Register
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,PasswordHash,LastLogin")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformations.FindAsync(id);
            var user = await _context.Users.FindAsync(id);

            //if user doesn't exist and userinformation doesn't exist throw error.

            if (userInformation == null && user == null)
            {
                return NotFound();
            }

            //if userinformation doesn't exist, it means it hasn't been created yet !
            //so let's create a userinformation entry with just the id and default/null values for everything else.
            if (userInformation == null)
            {
                userInformation = new UserInformation();
                userInformation.PersonId = id ?? default(int);
                try
                {
                    _context.UserInformations.Add(userInformation);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }


            var UserandInformation = new ExtendedInformation();
            UserandInformation.user = user;
            UserandInformation.userInformation = userInformation;

               

            ViewData["PersonId"] = new SelectList(_context.Users, "Id", "Id", userInformation.PersonId);
            
            return View(UserandInformation);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EditResponse response)
        {
            var id = Int32.Parse(response.Id);
            var userinfoobj = await _context.UserInformations.FindAsync(id);
            var userobj = await _context.Users.FindAsync(id);
            userobj.Password = response.Password;
            if(response.PostalCode != "")
            {
                userinfoobj.PostalCode = Int32.Parse(response.PostalCode);
            }
            if(response.AddressCode != "")
            {
                userinfoobj.AddressCode = Int32.Parse(response.AddressCode);
            }
            if (response.LastLogin != "")
            {
                userobj.LastLogin = DateTime.Parse(response.LastLogin);
            }

            userinfoobj.AddressLine1 = response.AddressLine1;
            userinfoobj.AddressLine2 = response.AddressLine2;
            userinfoobj.AddressLine3 = response.AddressLine3;
            userinfoobj.PostalAddress1 = response.PostalAddress1;
            userinfoobj.PostalAddress2 = response.PostalAddress2;
            userinfoobj.TellNo = response.TellNo;
            userinfoobj.CellNo = response.CellNo;

            if (UserExists(8))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userinfoobj);
                    _context.Update(userobj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(8))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { status = true, message = "Completed Update to Database" });
            }
            return Json(new { status = false, message = "Whoops" });
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);

            return Json(new { status = true, message = "Logged out!" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private async Task<User> AuthenticateUser(string name, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == name);

            if (user != null)
            {
                if(user.Password == password)
                {
                    return user;
                }else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
    }
}
