
using Assessment.Operations;
using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode.Conformist;
using Serilog;

namespace Assessment.Model
{
    public class ReleationTable
    {
        public virtual int OrderId { get; set; }
        public virtual int ProductId { get; set; }

        public virtual int customerId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is ReleationTable other)
            {
                return OrderId == other.OrderId &&
                       ProductId == other.ProductId &&
                       customerId == other.customerId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderId, ProductId, customerId);
        }

        public static void Add(ReleationTable releation)
        {
            try
            {
                DBService.Add<ReleationTable>(releation);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Adding releation -{ex.Message}");
            }
        }
    }
    public class ReleationTableMap : ClassMap<ReleationTable>
    {
        public ReleationTableMap()
        {
            Table("Releationtable");

            CompositeId().KeyProperty(x => x.customerId)
                        .KeyProperty(x => x.OrderId)
                        .KeyProperty(x => x.ProductId);
        }
    }
}
