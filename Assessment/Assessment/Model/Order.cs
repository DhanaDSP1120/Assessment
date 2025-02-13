using Assessment.Operations;
using NHibernate.Criterion;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Serilog;

namespace Assessment.Model
{
    public class Order
    {
        public virtual int Id { get; set; }
        public virtual string OrderId { get; set; }
        public virtual string Region { get; set; }
        public virtual DateTime DateOfSale { get; set; }
        public virtual int QuantitySold { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal SellingPrice { get; set; }
        public virtual int PaymentMode {  get; set; }

        public static Order GetById(string id)
        {
            return DBService.FetchByField<Order>(Restrictions.Eq(nameof(OrderId), id));
        }
        public static int Add(Order order)
        {
            try
            {
               return DBService.Add<Order>(order);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Adding order -{ex.Message}");
                return -1;
            }
        }
        public static void Update(Order order)
        {
            try
            {
                DBService.Update<Order>(order);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Updating order -{ex.Message}");
            }
        }

        public static IList<Order> GetAll()
        {
            return DBService.FetchAll<Order>();
        }
    }

    public class OrderMapping : ClassMapping<Order>
    {
        public OrderMapping()
        {
            Table("Orders");
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(X => X.OrderId, m => m.Unique(true));
            Property(x => x.Region);
            Property(x => x.DateOfSale);
            Property(x => x.QuantitySold);
            Property(x => x.UnitPrice);
            Property(x => x.Discount);
            Property(x => x.SellingPrice);
            Property(x => x.PaymentMode);
        }
    }
}
