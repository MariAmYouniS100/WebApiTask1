using Microsoft.EntityFrameworkCore;
using WebApi_lab1.Models;

namespace WebApi_lab1.Context
{
    public class CompanyContext : DbContext
    {
        protected override  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-KQGM5HDP;Database=APIDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True");
        }
        public DbSet<Student> Students { get; set; }
    }
}
