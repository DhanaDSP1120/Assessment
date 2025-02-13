
using NHibernate.Criterion;
using Serilog;

namespace Assessment.Operations
{
    public class DBService
    {
        private static NHibernate.ISession _session;

        private static void BeginTransaction()
        {
            _session = NHibernateHelper.OpenSession();
            _session.BeginTransaction();
        }

        private static void CommitTransaction()
        {
            _session.Transaction.Commit();
        }

        private static void CloseSession()
        {
            _session.Close();
        }

        public static int Add<T>(T entity)
        {
            BeginTransaction();
            try
            {
               int id =(int) _session.Save(entity);
                CommitTransaction();
                return id;
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Adding record-{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }

        public static void Update<T>(T entity)
        {
            BeginTransaction();
            try
            {
                _session.Update(entity);
                CommitTransaction();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Updating record -{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }

        public static T Get<T>(object id)
        {
            BeginTransaction();
            try
            {
                return _session.Get<T>(id);                
            }
            catch (Exception ex)
            {
                Log.Error($"Error in Getting record -{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }
        public static T FetchByField<T>(params ICriterion[] restrictions) where T : class
        {
            BeginTransaction();
            try
            {
                var criteria = _session.CreateCriteria<T>();

                foreach(var condition in restrictions)
                {
                    criteria.Add(condition);
                    
                }

                return criteria.UniqueResult<T>();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in FetchByField record -{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }
        public static int FetchRowCount<T>(params ICriterion[] restrictions) where T : class
        {
            BeginTransaction();
            try
            {
                var criteria = _session.CreateCriteria<T>();

                foreach (var condition in restrictions)
                {
                    criteria.Add(condition);
                }
                criteria.SetProjection(Projections.RowCount());
                return criteria.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in FetchByField record -{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }
        public static decimal SumSingleCol<T>(string columnName ,params ICriterion[] restrictions) where T : class
        {
            BeginTransaction();
            try
            {
                var criteria = _session.CreateCriteria<T>();

                foreach (var condition in restrictions)
                {
                    criteria.Add(condition);
                }
                criteria.SetProjection(Projections.Sum(columnName));
                return criteria.UniqueResult<decimal>();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in sum record -{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }
        public static IList<T> FetchAll<T>()
        {
            BeginTransaction();
            try
            {
                return _session.Query<T>().ToList();
            } 
            catch (Exception ex)
            {
                Log.Error($"Error in fetching All record -{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }
        public static IList<T> FetchAllBycriteria<T>(params ICriterion[] restrictions) where T : class
        {
            BeginTransaction();
            try
            {
                var criteria = _session.CreateCriteria<T>();

                foreach (var condition in restrictions)
                {
                    criteria.Add(condition);

                }

                return criteria.List<T>();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in FetchByField record -{ex.Message}");
                throw;
            }
            finally
            {
                CloseSession();
            }
        }
    }
}
