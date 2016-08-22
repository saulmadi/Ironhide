using System;
using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Modules.UserAccounts.Facebook;
using Ironhide.Api.Modules.UserAccounts.Google;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Nancy;
using Nancy.ModelBinding;

namespace Ironhide.Api.Modules.UserAccounts
{
    public class UserAccountModule : NancyModule
    {
        public UserAccountModule(IUserAbilityRepository abilityReadRepo, ICommandDispatcher commandDispatcher,
            IPasswordEncryptor passwordEncryptor, IMapper mapper, IUserSessionFactory userSessionFactory)
        {
            Post["/register", true] =
                async (a, ct) =>
                      {
                          var req = this.Bind<NewUserRequest>();
                          IEnumerable<UserAbility> abilities =
                              mapper.Map<IEnumerable<UserAbilityRequest>, IEnumerable<UserAbility>>(req.Abilities);
                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                              new CreateEmailLoginUser(req.Email, passwordEncryptor.Encrypt(req.Password), req.Name,
                                  req.PhoneNumber, abilities));
                          return null;
                      };


            Post["/register/facebook", true] =
                async (a, ct) =>
                      {
                          var req = this.Bind<FacebookRegisterRequest>();
                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                              new CreateFacebookLoginUser(req.id, req.email, req.first_name, req.last_name, req.link,
                                  req.name,
                                  req.url_image));
                          return null;
                      };

            Post["/register/google", true] =
                async (a, ct) =>
                      {
                          var req = this.Bind<GoogleRegisterRequest>();
                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                              new CreateGoogleLoginUser(req.id, req.email, req.name.givenName, req.name.familyName,
                                  req.url,
                                  req.displayName, req.image.url));
                          return null;
                      };

            Post["/password/requestReset", true] =
                async (a, ct) =>
                      {
                          var req = this.Bind<ResetPasswordRequest>();
                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                              new StartPasswordResetProcess(req.Email));
                          return null;
                      };

            Put["/password/reset/{token}", true] =
                async (a, ct) =>
                      {
                          var newPasswordRequest = this.Bind<NewPasswordRequest>();
                          Guid token = Guid.Parse((string) a.token);
                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                              new ResetPassword(token, passwordEncryptor.Encrypt(newPasswordRequest.Password)));
                          return null;
                      };

            Post["/user/abilites", true] =
                async (a, ct) =>
                      {
                          var requestAbilites = this.Bind<UserAbilitiesRequest>();
                          await commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                              new AddAbilitiesToUser(requestAbilites.UserId,
                                  requestAbilites.Abilities.Select(x => x.Id)));

                          return null;
                      };

            Get["/abilities", true] = async (_, c) =>
                                            {
                                                IEnumerable<UserAbility> abilites =
                                                    await abilityReadRepo.GetAll();

                                                IEnumerable<UserAbilityRequest> mappedAbilites =
                                                    mapper
                                                        .Map<IEnumerable<UserAbility>, IEnumerable<UserAbilityRequest>>(
                                                            abilites);

                                                return mappedAbilites;
                                            };
        }
    }
}