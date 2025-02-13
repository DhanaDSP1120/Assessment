using Assessment.Operations;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Serilog;
using NHibernate.Criterion;

namespace Assessment.Model
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual string ProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Category {  get; set; }

        public static Product GetById(string id)
        {
            return DBService.FetchByField<Product>(Restrictions.Eq(nameof(ProductId), id));
        }
        public static int Add(Product product)
        {
            try
            {
               return DBService.Add<Product>(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error($"Error in Adding Product -{ex.Message}");
                return -1;
            }
        }
        public static void Update(Product product)
        {
            try
            {
                DBService.Update<Product>(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error($"Error in Adding Product -{ex.Message}");
            }
        }

        public static IList<Product> GetAll()
        {
            return DBService.FetchAll<Product>();
        }
    }

    public class ProductMapping : ClassMapping<Product>
    {
        public ProductMapping()
        {
            Table("Products");
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(X => X.ProductId, m => m.Unique(true));
            Property(x => x.Name);
            Property(x => x.Category);
        }
    }

}
