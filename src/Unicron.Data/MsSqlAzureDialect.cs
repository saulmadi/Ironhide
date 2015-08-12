using NHibernate.Dialect;

namespace Unicron.Data
{
    public class MsSqlAzureDialect : MsSql2008Dialect
    {
        public override string PrimaryKeyString
        {
            get
            {
                return "primary key CLUSTERED";
            }
        }
    }
}