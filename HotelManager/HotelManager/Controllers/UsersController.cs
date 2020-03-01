using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManager.Data;
using HotelManager.Models;
using HotelManager.Repository;

namespace HotelManager.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UsersRepository _usersRepository;

        public UsersController(ApplicationDbContext context, UsersRepository usersRepository)
        {
            _context = context;
            _usersRepository = usersRepository;
        }

        // GET: Users
        public async Task<IActionResult> ListAllAdmins()
        {
            return View(await _usersRepository.ListAllAdmins());
        }

        public async Task<IActionResult> ListAllClients()
        {
            return View(await _usersRepository.ListAllClients());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UsersTable
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult CreateAdmin()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(CreateAdminModel adminModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FirstName = adminModel.FirstName,
                    MiddleName = adminModel.MiddleName,
                    LastName = adminModel.LastName,
                    Email = adminModel.MiddleName,
                    PhoneNumber = adminModel.PhoneNumber,
                    EGN = adminModel.EGN
                };
                await _usersRepository.CreateAdmin(user);
                return RedirectToAction(nameof(ListAllAdmins));
            }
            return View(adminModel);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UsersTable.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,MiddleName,LastName,EGN,PhoneNumber,Email,IsActive,IsAdmin,DateHired,DateFired")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListAllClients));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(_context.UsersTable.Where(u => u.Id == id).First());
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _usersRepository.DeleteUser(id);
            return RedirectToAction(nameof(ListAllClients));
        }

        public async Task<IActionResult> DemoteAdminToUser(string id)
        {
            User user = await _usersRepository.FindUserById(id);
            await _usersRepository.DemoteAdminToClient(user);
            return RedirectToAction(nameof(ListAllAdmins));
        }

        public async Task<IActionResult> PromoteClientToAdmin(string id)
        {
            return View(await _usersRepository.FindUserById(id));
        }

        [HttpPost]
        public async Task<IActionResult> PromoteClientToAdmin(User user)
        {
            await _usersRepository.PromoteClientToAdmin(user);
            return RedirectToAction(nameof(ListAllAdmins));
        }

        private bool UserExists(string id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
