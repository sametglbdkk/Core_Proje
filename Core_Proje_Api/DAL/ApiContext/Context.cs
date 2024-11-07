using Core_Proje_Api.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje_Api.DAL.ApiContext
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-QEKI6MG\\SQLEXPRESS;Database=CoreProjeDB2;Integrated Security=True;TrustServerCertificate=True;");



        }

        public DbSet<Category> Categories { get; set; }
    }
}


