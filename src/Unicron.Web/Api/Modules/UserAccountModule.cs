using System;
using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using AutoMapper;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Ironhide.Web.Api.Infrastructure;
using Ironhide.Web.Api.Requests;
using Ironhide.Web.Api.Requests.Facebook;
using Ironhide.Web.Api.Requests.Google;
using Nancy;
using Nancy.ModelBinding;

namespace Ironhide.Web.Api.Modules
{

    public class UserAccountModule : NancyModule
    {        
        public UserAccountModule(IReadOnlyRepository readOnlyRepository,ICommandDispatcher commandDispatcher, IPasswordEncryptor passwordEncryptor, IMappingEngine mappingEngine)
        {
            Post["/register"] =
                _ =>
                    {
                        var req = this.Bind<NewUserRequest>();
                        var abilities = mappingEngine.Map<IEnumerable<UserAbilityRequest>, IEnumerable<UserAbility>>(req.Abilities);
                        commandDispatcher.Dispatch(this.UserSession(),
                                                   new CreateEmailLoginUser(req.Email, passwordEncryptor.Encrypt(req.Password), req.Name, req.PhoneNumber, abilities));
                        return null;
                    };


            Post["/register/facebook"] =
                _ =>
                    {
                        var req = this.Bind<FacebookRegisterRequest>();
                        commandDispatcher.Dispatch(this.UserSession(), new CreateFacebookLoginUser(req.id,req.email, req.first_name, req.last_name,req.link,req.name,req.url_image));
                        return null;
                    };

            Post["/register/google"] =
                _ =>
                    {
                        var req = this.Bind<GoogleRegisterRequest>();
                        commandDispatcher.Dispatch(this.UserSession(), new CreateGoogleLoginUser(req.id,req.email,req.name.givenName,req.name.familyName,req.url,req.displayName,req.image.url));
                        return null;
                    };

            Post["/password/requestReset"] =
                _ =>
                {
                    var req = this.Bind<ResetPasswordRequest>();
                    commandDispatcher.Dispatch(this.UserSession(),
                                               new CreatePasswordResetToken(req.Email) );
                    return null;
                };

            Put["/password/reset/{token}"] =
                p =>
                {
                    var newPasswordRequest = this.Bind<NewPasswordRequest>();
                    var token = Guid.Parse((string)p.token);
                    commandDispatcher.Dispatch(this.UserSession(),
                                               new ResetPassword(token, passwordEncryptor.Encrypt(newPasswordRequest.Password)));
                    return null;
                };

            Post["/user/abilites"] = p =>
            {

                var requestAbilites = this.Bind<UserAbilitiesRequest>();
                commandDispatcher.Dispatch(this.UserSession(), new AddAbilitiesToUser(requestAbilites.UserId, requestAbilites.Abilities.Select(x => x.Id)));

                return null;


            };

            Get["/abilities"] = _ =>
            {
                var abilites = readOnlyRepository.GetAll<UserAbility>();

                var mappedAbilites = mappingEngine.Map<IEnumerable<UserAbility>, IEnumerable<UserAbilityRequest>>(abilites);

                return mappedAbilites;
            };
        }
    }
}