namespace GestionOrdinateurs.API.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ParcInformatiqueContextFactory : IDesignTimeDbContextFactory<ParcInformatiqueContext>
{
    public ParcInformatiqueContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ParcInformatiqueContext>();
        optionsBuilder.UseSqlite("Data Source=parcinfo.db");

        return new ParcInformatiqueContext(optionsBuilder.Options);
    }
}
