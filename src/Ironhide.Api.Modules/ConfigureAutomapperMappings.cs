using System;
using Autofac;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Modules.UserAccounts;
using Ironhide.Api.Modules.UserManagement;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Api.Modules
{
    public class ConfigureAutomapperMappings : IBootstrapperTask<ContainerBuilder>
    {
        private readonly IMapper _mapper;

        #region IBootstrapperTask<ContainerBuilder> Members
        
        public Action<ContainerBuilder> Task
        {
            get
            {
                return container =>
                    {
                        Mapper.Initialize(cfg =>
                        {
                            cfg.CreateMap<User, AdminUserResponse>();
                            cfg.CreateMap<UserAbility, UserAbilityRequest>().ReverseMap();
                        });

                        container.RegisterInstance(Mapper.Configuration.CreateMapper()).As<IMapper>();
                    };
            }
        }

        #endregion
    }
}