using System;
using Autofac;
using AutoMapper;
using Unicron.Users.Domain.Entities;
using Unicron.Web.Api.Requests;
using Unicron.Web.Api.Responses.Admin;

namespace Unicron.Web.Api.Infrastructure.Configuration
{
    public class ConfigureAutomapperMappings : IBootstrapperTask<ContainerBuilder>
    {
        #region IBootstrapperTask<ContainerBuilder> Members

        public Action<ContainerBuilder> Task
        {
            get
            {
                return container =>
                    {
                        Mapper.CreateMap<User, AdminUserResponse>();
                        Mapper.CreateMap<UserAbility, UserAbilityRequest>().ReverseMap();
                    };
            }
        }

        #endregion
    }
}