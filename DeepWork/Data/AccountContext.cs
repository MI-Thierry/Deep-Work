using DeepWork.Models;
using Microsoft.EntityFrameworkCore;

namespace DeepWork.Data;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
	public DbSet<Account> Accounts => Set<Account>();
}
