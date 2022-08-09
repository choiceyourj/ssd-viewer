using AnnotationWebApp.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace AnnotationWebApp.Data
{
    public class InitialDbSeed
    {
        public async static Task SeedUserData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddIdentity<SsdUser, SsdRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<AppDbContext>();

                    context.Database.EnsureCreated();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<SsdUser>>();
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<SsdRole>>();

                    // First, We should create Role

                    #region ==================== Create Initial Role ================================

                    // Admin Role
                    var isAdminRoleExist = await roleMgr.RoleExistsAsync(SsdUserType.ADMIN.ToString());
                    if (!isAdminRoleExist)
                    {
                        var role = new SsdRole();
                        role.Name = SsdUserType.ADMIN.ToString();
                        role.RoleDescription = "Admin Role";
                        role.CreationDate = DateTime.Now;

                        var result = await roleMgr.CreateAsync(role);
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Debug.WriteLine("Administrator Role is created");
                    }

                    // Staff Role
                    var isClientExist = await roleMgr.RoleExistsAsync(SsdUserType.STAFF.ToString());
                    if (!isClientExist)
                    {
                        var role = new SsdRole();
                        role.RoleDescription = "Staff Role";
                        role.CreationDate = DateTime.Now;

                        role.Name = SsdUserType.STAFF.ToString();

                        var result = await roleMgr.CreateAsync(role);
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Debug.WriteLine("Client Role is created");
                    }
                    #endregion

                    // Now We can create User with Role

                    // Admin User
                    var adminUser = userMgr.FindByNameAsync("admin@clew.tech").Result;
                    if (adminUser == null)
                    {
                        adminUser = new SsdUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = "admin@clew.tech",
                            Email = "admin@clew.tech",
                            UserDisplayName = "Admin User Id",
                            PhoneNumber = "01012345678",
                        };

                        var result = userMgr.CreateAsync(adminUser, "ssdSSD1234!!").Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Debug.WriteLine("Admin User Id is created");
                        result = await userMgr.AddToRoleAsync(adminUser, SsdUserType.ADMIN.ToString());
                    }
                    else
                    {
                        throw new Exception("Admin User Id already exists");
                    }



                    #region Create UserId for DevTest. DO NOT DELETE IT

                    // Ghost User for Development And Test
                    var ghostUser = userMgr.FindByNameAsync("support@clew.tech").Result;
                    if (ghostUser == null)
                    {
                        ghostUser = new SsdUser
                        {
                            Id = "f23f250a-df1f-460f-99cc-6beb034327e0",
                            UserName = "support@clew.tech",
                            Email = "support@clew.tech",
                            UserDisplayName = "Ghost staff for DevTest",
                            PhoneNumber = "01012345678",
                        };

                        var result = userMgr.CreateAsync(ghostUser, "ssdSSD1234!!").Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Debug.WriteLine("Ghost staff Id is created");
                        result = await userMgr.AddToRoleAsync(ghostUser, SsdUserType.STAFF.ToString());
                    }
                    else
                    {
                        throw new Exception("Ghost staff Id already exists");
                    }

                    #endregion
                }
            }
        }
    }
}
