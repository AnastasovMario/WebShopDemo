using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShopDemo.Core.Data.Models;
using WebShopDemo.Core.Data.Models.Account;

namespace WebShopDemo.Core.Data
{
	//За да работи миграцията, трябва да сложим ApplicationUser-a
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);
		}

		//След като създадем Модела, трябва да го инициализираме в db context-a
		public DbSet<Product> Products { get; set; }
	}
}