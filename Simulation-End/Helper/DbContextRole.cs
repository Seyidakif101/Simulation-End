using Microsoft.AspNetCore.Identity;
using Simulation_End.Enums;
using Simulation_End.Models;
using Simulation_End.ViewModels.AccountViewModels;

namespace Simulation_End.Helper
{
    public class DbContextRole
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AdminVM _admin;

        public DbContextRole(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _admin = _configuration.GetSection("AdminSettings").Get<AdminVM>() ?? new();
        }
        public async Task DbRole()
        {
            await CreateRole();
            await CreateAdmin();
        }
        private async Task CreateAdmin()
        {
            AppUser user = new()
            {
                UserName = _admin.Username,
                Email=_admin.Email
            };
            var result = await _userManager.CreateAsync(user, _admin.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleEnum.Admin.ToString());
            }
        }
        private async Task CreateRole()
        {
            foreach(var role in Enum.GetNames(typeof(RoleEnum)))
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name=role
                });
            }
        }
    }
}
