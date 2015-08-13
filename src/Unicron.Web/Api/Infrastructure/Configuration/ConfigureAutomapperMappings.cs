using System;
using Autofac;
using AutoMapper;
using Ironhide.Users.Domain.Entities;
using Ironhide.Web.Api.Requests;
using Ironhide.Web.Api.Responses.Admin;

namespace Ironhide.Web.Api.Infrastructure.Configuration
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