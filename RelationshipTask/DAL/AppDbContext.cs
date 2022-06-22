using Microsoft.EntityFrameworkCore;

namespace RelationshipTask.DAL
{
    public class AppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


    }
}
