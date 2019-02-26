using CompareHare.Domain.Entities;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CompareHare.Api.Extensions
{
    public static class DbContextExtension
    {
        public static bool AllMigrationsApplied(this CompareHareDbContext dbContext)
        {
            var applied = dbContext.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = dbContext.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this CompareHareDbContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!dbContext.Roles.Any()) {
                dbContext.Roles.Add(new Role {
                  Name = "Admin",
                  NormalizedName = "Admin",
                });

                dbContext.Roles.Add(new Role {
                  Name = "User",
                  NormalizedName = "User",
                });

                dbContext.SaveChanges();
            }

            var seedUsers = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(path: $"Seeds{Path.DirectorySeparatorChar}asp_net_users.json"));

            foreach(var user in seedUsers) {
                if (dbContext.Users.Any(x => x.Email == user.Email)) continue;

                dbContext.Add(user);

                dbContext.SaveChanges();

                userManager.AddToRoleAsync(user, "Admin").Wait();
                userManager.AddToRoleAsync(user, "User").Wait();
            }

            var stateUtilityIndices = JsonConvert.DeserializeObject<List<StateUtilityIndex>>(File.ReadAllText(path: $"Seeds{Path.DirectorySeparatorChar}state_utility_indices.json"));

            foreach(var index in stateUtilityIndices) {
              if (dbContext.StateUtilityIndices.Any(x => x.LoaderDataIdentifier == index.LoaderDataIdentifier
                && x.UtilityState == index.UtilityState
                && x.UtilityType == index.UtilityType)) continue;

              dbContext.StateUtilityIndices.Add(index);

              dbContext.SaveChanges();
            }
        }
    }
}
