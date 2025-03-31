using DeviceStore.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DeviceStore.Models;
namespace DeviceStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Device> Devices { get; set; }
    }
}
