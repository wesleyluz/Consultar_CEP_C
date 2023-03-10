using Microsoft.EntityFrameworkCore;

namespace ConsultarCEP.Model.Context
{
    public class MSSQLContext : DbContext
    {
        public MSSQLContext() { }

        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options) { }

        public DbSet<CEP> CEPs { get; set; }
    }
}
