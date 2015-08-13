using System;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using NHibernate;

namespace Ironhide.Web.Api.Infrastructure.Configuration
{
    public class DataBaseErrorInTransactionHandler
    {
        readonly ILifetimeScope _container;

        public DataBaseErrorInTransactionHandler(ILifetimeScope container)
        {
            _container = container;
        }

        public void Register(IPipelines pipelines)
        {
            pipelines.OnError.AddItemToStartOfPipeline(RollBackTransaction);
        }

        Response RollBackTransaction(NancyContext nancyContext, Exception exception)
        {
            var session = _container.Resolve<ISession>();

            if (session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
            }
            return nancyContext.Response;
        }
    }
}