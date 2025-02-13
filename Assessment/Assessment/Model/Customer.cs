using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Assessment.Operations;
using Serilog;
using NHibernate.Criterion;

namespace Assessment.Model
{
    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual string CustomerId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Address { get; set; }

        
        public static Customer GetById(string id)
        {
            return DBService.FetchByField<Customer>(Restrictions.Eq(nameof(CustomerId), id));
        }
        public static int Add(Customer customer)
        {
            try
            {
               return DBService.Add<Customer>(customer);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Adding customer -{ex.Message}");
                return -1;
            }
        }
        public static void Update(Customer customer)
        {
            try
            {
                DBService.Update<Customer>(customer);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Updating customer -{ex.Message}");
            }
        }

        public static IList<Customer> GetAll()
        {
            return DBService.FetchAll<Customer>();
        }


    }
    public class CustomerMapping : ClassMapping<Customer>
    {
        public CustomerMapping()
        {
            Table("Customers");
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(X => X.CustomerId, m => m.Unique(true));
            Property(x => x.Name);
            Property(x => x.Email);
            Property(x => x.Address);
        }
    }
}
