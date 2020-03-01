using HotelManager.Data;
using HotelManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class UsersRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersRepository(UserManager<IdentityUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        async public Task<bool> RemoveUser(User user)
        {
            try
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> PromoteClientToAdmin(User user, string EGN, string middleName, string phoneNumber)
        {
            try
            {
                user.EGN = EGN;
                user.MiddleName = middleName;
                user.IsAdmin = true;
                user.DateHired = DateTime.Now;
                await AssignUserToRole(user, "Admin");

                _context.UsersTable.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> PromoteClientToAdmin(User user)
        {
            try
            {
                IdentityUser userFind = await _userManager.FindByEmailAsync(user.Email);
                User promotedUser = new User
                {
                    UserName = userFind.UserName,
                    Email = user.Email,
                    Id = userFind.Id,
                    PasswordHash = user.PasswordHash,
                    EmailConfirmed = userFind.EmailConfirmed,
                    TwoFactorEnabled = userFind.TwoFactorEnabled
                };
                promotedUser.IsAdmin = true;
                promotedUser.DateHired = DateTime.Now;
                promotedUser.PhoneNumber = user.PhoneNumber;
                promotedUser.EGN = user.EGN;
                promotedUser.FirstName = user.FirstName;
                promotedUser.MiddleName = user.MiddleName;
                promotedUser.LastName = user.LastName;

                //var updateResult = await _userManager.UpdateAsync(promotedUser);
                var deleteResult = await _userManager.DeleteAsync(userFind);
                var createResult = await _userManager.CreateAsync(promotedUser);

                var roleResult = await AssignUserToRole(promotedUser, "Admin");
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        async public Task<bool> CreateAdmin(User user)
        {
            try
            {
                user.DateHired = DateTime.Now;
                user.IsAdmin = true;
                _context.UsersTable.Add(user);
                await _context.SaveChangesAsync();
                await AssignUserToRole(user, "Admin");
                return true;
            }
            catch
            {
                return false;
            }
        }

        //using UserManager
        async public Task<bool> CreateAdmin(User user, string password)
        {
            try
            {
                user.DateHired = DateTime.Now;
                user.IsAdmin = true;
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, password);
                bool roleResult = await AssignUserToRole(user, "Admin");
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> DemoteAdminToClient(User user)
        {
            try
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                await AssignUserToRole(user, "Client");
                user.IsAdmin = false;
                user.DateFired = DateTime.Now;
                _context.UsersTable.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<User> FindUserById(string id)
        {
            return _context.UsersTable.Where(user => user.Id == id).First();
        }

        async public Task<bool> DeleteUser(User user)
        {
            try
            {
                _context.UsersTable.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        async public Task<bool> DeleteUser(string id)
        {
            try
            {
                User userToDelete = _context.UsersTable.Where(user => user.Id == id).First();
                _context.UsersTable.Remove(userToDelete);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async public Task<IEnumerable<User>> ListAllAdmins()
        {
            return await _context.UsersTable.Where(user => user.IsAdmin == true).ToListAsync();
        }

        async public Task<IEnumerable<User>> ListAllClients()
        {
            return await _context.UsersTable.Where(user => user.IsAdmin == false).ToListAsync();
        }

        async private Task<bool> AssignUserToRole(User user, string role)
        {
            var roleCheck = await _roleManager.RoleExistsAsync(role);
            if (!roleCheck)
            {
                //here in this line we are creating admin role and seed it to the database
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            //here we are assigning the Admin role to the User that we have registered above
            await _userManager.AddToRoleAsync(user, role);

            return true;
        }
    }
}
