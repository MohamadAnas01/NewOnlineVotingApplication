using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NewOnlineVotingApplication.Models;
using NewOnlineVotingApplication.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace NewOnlineVotingApplication.Controllers
{
    public class UserRegistrationsController : Controller
    {
        private readonly OnlineVotingAppContext _context;
        private readonly IWebHostEnvironment _environment;
       

        public UserRegistrationsController(OnlineVotingAppContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [Authorize(Roles = "Admin")]
        // GET: UserRegistrations/Index
        //View the User Details
        public async Task<IActionResult> Index()
        {
              return _context.UserRegistration != null ? 
                          View(await _context.UserRegistration.ToListAsync()) :
                          Problem("Entity set 'OnlineVotingAppContext.UserRegistration'  is null.");
        }

        // GET: UserRegistrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRegistration == null)
            {
                return NotFound();
            }

            var userRegistration = await _context.UserRegistration
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRegistration == null)
            {
                return NotFound();
            }

            return View(userRegistration);
        }

        // GET: UserRegistrations/Create
        public IActionResult Create()
        {
            return View();
        }
        // GET: UserRegistrations/RegistrationSuccess
        public IActionResult RegistrationSuccess()
        {
            return View();
        }


        // POST: UserRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to..
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public IActionResult Create(UserRegistrationViewModel vm,UserRegistration userRegistration)
        {
            string fileName = UploadFile(vm);
            var user = new UserRegistration
            {
               FirstName=vm.FirstName,
               LastName=vm.LastName,
               DateOfBirth=vm.DateOfBirth,
               Gender=vm.Gender,
               PhoneNumber=vm.PhoneNumber,
               Email=vm.Email,
               Password=vm.Password,
               ConfirmPassword=vm.ConfirmPassword,
               VoterId=vm.VoterId,
               IdProof=fileName,
                Voting_status = "Denied"
            };

            _context.UserRegistration.Add(user);
            _context.SaveChanges();

           

            return RedirectToAction("RegistrationSuccess");

        }
        //UploadFile method to Upload Photo
        private string UploadFile(UserRegistrationViewModel vm)
        {
            string fileName = "";

            if (vm.IdProof != null)
            {
                string uploadDir = Path.Combine(_environment.WebRootPath, "Images");
                fileName = vm.IdProof.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    vm.IdProof.CopyTo(filestream);
                }
            }
            return fileName;
        }


        // GET: UserRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRegistration == null)
            {
                return NotFound();
            }

            var userRegistration = await _context.UserRegistration.FindAsync(id);
            if (userRegistration == null)
            {
                return NotFound();
            }
            return View(userRegistration);
        }

        // POST: UserRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,DateOfBirth,Gender,PhoneNumber,Email,Password,ConfirmPassword,VoterId,IdProof,Voting_status")] UserRegistration userRegistration)
        {
            if (id != userRegistration.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRegistrationExists(userRegistration.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userRegistration);
        }

        //method to approve the status of the User
        public async Task<IActionResult> UpdateVoterStatus(int id, string value)
        {
            var userRegistration = await _context.UserRegistration.FindAsync(id);

            if (userRegistration == null)
            {
                return NotFound();
            }

            if (value == "Approved")
            {
                //Sent the mail if the user clicks the approved button
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse("vedhavikram3022@gmail.com");
                email.To.Add(MailboxAddress.Parse(userRegistration.Email));
                email.Subject = "User Verified";
                var builder = new BodyBuilder();
                builder.HtmlBody = userRegistration.FirstName+" "+userRegistration.LastName + ", Your Details has been verfied and you can now poll your Votes.";
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("vedhavikram3022@gmail.com", "rfjiqevpbnuyleyb");
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                userRegistration.Voting_status = "Approved";
            }
            else if (value == "Denied")
            {
                userRegistration.Voting_status = "Denied";
            }

            _context.Update(userRegistration);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: UserRegistrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRegistration == null)
            {
                return NotFound();
            }

            var userRegistration = await _context.UserRegistration
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRegistration == null)
            {
                return NotFound();
            }

            return View(userRegistration);
        }

        // POST: UserRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRegistration == null)
            {
                return Problem("Entity set 'OnlineVotingAppContext.UserRegistration'  is null.");
            }
            var userRegistration = await _context.UserRegistration.FindAsync(id);
            if (userRegistration != null)
            {
                _context.UserRegistration.Remove(userRegistration);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRegistrationExists(int id)
        {
          return (_context.UserRegistration?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        public IActionResult Login()
        {
            return View();
        }
        //VotePoll Page
        public IActionResult VotePoll()
        {
            return View();
        }
        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string password)
        {
            TempData["Email"] = Email;
            //Check the email and password matches with the User input
            var user = await _context.UserRegistration.FirstOrDefaultAsync(u => u.Email == Email && u.Password == password);
            var userStatus = "";
            var existingVote = _context.VoteResults.FirstOrDefault(v => v.Email == Email);
            //if the voter email already present,vote cannot be registered
            if (existingVote != null)
            {
                ModelState.AddModelError("", "Already Voted");
                return View("Login");
            }

              userStatus = user.Voting_status;
            
            //if the user value is empty, it displays the following message
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid User Login.");
                return View();
            }
            // if login is success
            if ( userStatus=="Approved")
            {
                HttpContext.Session.SetString("UserLoggedIn", "true");

                // Create and set authentication cookie
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User")
                };

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                HttpContext.SignInAsync(authScheme, new ClaimsPrincipal(new ClaimsIdentity(authClaims, authScheme)), authProperties);

                return RedirectToAction(nameof(VotePoll));
            }

            else
            {
                ModelState.AddModelError("", "Your are not approved by Admin..");
                return View();
            }
           
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Logout()
        {
            // Remove the "AdminLoggedIn" session variable
            HttpContext.Session.Remove("UserLoggedIn");

            // Sign out the authentication cookie
            var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            HttpContext.SignOutAsync(authScheme);

            return RedirectToAction(nameof(Login));
        }
       
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Vote(string value,VoteResults voteResults)
        {
            //Take the email value from the login action method
            var email = TempData["Email"] as string;
            var mime = new MimeMessage();

           
                //check for the following Email in userRegistration table
                var existingData = _context.UserRegistration.Where(u => u.Email == email).ToList();
                var newData = new List<VoteResults>();
                //assign the value to the VoterResults Table
                foreach (var item in existingData)
                {
                    newData.Add(new VoteResults
                    {
                        VoterName = item.FirstName + item.LastName,
                        Email = item.Email,
                        Vote = value
                    });
                }
                _context.VoteResults.AddRange(newData);
                _context.SaveChanges();
                mime.Sender = MailboxAddress.Parse("vedhavikram3022@gmail.com");
                mime.To.Add(MailboxAddress.Parse(email));
                mime.Subject = "Voted Successfully";
                var builder = new BodyBuilder();
                builder.HtmlBody = voteResults.VoterName + ", Your Vote has been successfully Registered!";
                mime.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("vedhavikram3022@gmail.com", "rfjiqevpbnuyleyb");
                await smtp.SendAsync(mime);
                smtp.Disconnect(true);
              

                return RedirectToAction("VoteSuccess");
            

        }

        public IActionResult VoteSuccess()
        {
            return View();
        }
    }
}
