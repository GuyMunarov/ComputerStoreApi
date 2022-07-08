using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseConfiguration
{
    public static class DbMigrationExtensions
    {
        public async static void MigrateDb(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {

                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();


                await dataContext.Database.MigrateAsync();


            }
        }
    }
}
