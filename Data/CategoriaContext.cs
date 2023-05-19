using Microsoft.EntityFrameworkCore;

namespace Arvore.Data
{
    public class CategoriaContext : DbContext
    {
        public CategoriaContext(DbContextOptions<CategoriaContext> options) : base(options)
        {
            
        }
    }
}