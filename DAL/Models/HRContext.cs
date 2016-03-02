using System.Data.Entity;

namespace DAL.Models
{
    public class HRContext : DbContext
    {
        public HRContext()
            : base("name=MyTestDBEntities")
        {

        }
        public DbSet<DAL.Employee> Employees { get; set; }
    }
}
