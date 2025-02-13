using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace Assessment.Operations
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.DataBaseIntegration(db =>
                    {
                        db.ConnectionString = "Host=localhost;Port=5432;Database=Tasks;Username=postgres;Password=postgres";
                        db.Dialect<NHibernate.Dialect.PostgreSQLDialect>();
                        db.Driver<NHibernate.Driver.NpgsqlDriver>();
                    });

                    var mapper = new ModelMapper();
                    mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
                    configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

                    _sessionFactory = configuration.BuildSessionFactory();
                    var schemaExport = new SchemaExport(configuration);
                    schemaExport.Create(false, true);
                }
                return _sessionFactory;
            }
        }

        public static NHibernate.ISession OpenSession() => SessionFactory.OpenSession();
    }
}
