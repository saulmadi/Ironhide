﻿using System;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Infrastructure.RestExceptions;
using Ironhide.Login.Domain;
using Ironhide.Login.Domain.Entities;
using Ironhide.Login.Domain.Exceptions;
using Ironhide.Login.Domain.Services;
using Ironhide.Login.Domain.ValueObjects;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Ironhide.Login.Api.Modules.Login
{
    public class LoginModule : NancyModule
    {
        public LoginModule(IPasswordEncryptor passwordEncryptor, IUserRepository userRepo, ITokenFactory tokenFactory, IMenuProvider menuProvider)
        {
            Post["/login", true] =
                async (a, ct) =>
                      {
                          var loginInfo = this.Bind<LoginRequest>();
                          if (loginInfo.Email == null) throw new UserInputPropertyMissingException("Email");
                          if (loginInfo.Password == null) throw new UserInputPropertyMissingException("Password");

                          EncryptedPassword encryptedPassword = passwordEncryptor.Encrypt(loginInfo.Password);

                          try
                          {
                              UserEmailLogin user =
                                  await userRepo.First<UserEmailLogin>(
                                      x =>
                                          x.Email == loginInfo.Email &&
                                          x.EncryptedPassword == encryptedPassword.Password);

                              if (!user.IsActive) throw new DisableUserAccountException();
                              string jwtoken = tokenFactory.Create(user);

                              return new SuccessfulLoginResponse<string>(jwtoken);
                          }
                          catch (ItemNotFoundException<UserEmailLogin>)
                          {
                              throw new UnauthorizedAccessException(
                                  "Invalid email address or password. Please try again.");
                          }
                          catch (DisableUserAccountException)
                          {
                              throw new UnauthorizedAccessException(
                                  "Your account has been disabled. Please contact your administrator for help.");
                          }
                      };

            Post["/login/facebook", true] =
                async (a, ct) =>
                      {
                          var loginInfo = this.Bind<LoginSocialRequest>();
                          if (loginInfo.Email == null)
                              throw new UserInputPropertyMissingException("Email");
                          if (loginInfo.Id == null)
                              throw new UserInputPropertyMissingException("Social Id");

                          try
                          {
                              UserFacebookLogin user =
                                  await userRepo.First<UserFacebookLogin>(
                                      x =>
                                          x.Email == loginInfo.Email &&
                                          x.FacebookId == loginInfo.Id);

                              if (!user.IsActive) throw new DisableUserAccountException();

                              string jwtoken = tokenFactory.Create(user);

                              return new SuccessfulLoginResponse<string>(jwtoken);
                          }
                          catch (ItemNotFoundException<UserEmailLogin>)
                          {
                              throw new UnauthorizedAccessException(
                                  "Invalid facebook user, you need to register first.");
                          }
                          catch (DisableUserAccountException)
                          {
                              throw new UnauthorizedAccessException(
                                  "Your account has been disabled. Please contact your administrator for help.");
                          }
                      };
            Get["/roles"] =
                _ =>
                {
                    this.RequiresAuthentication();
                    return Response.AsJson(menuProvider.getAllFeatures());
                };


            Post["/login/google", true] =
                async (a, ct) =>
                      {
                          var loginInfo = this.Bind<LoginSocialRequest>();
                          if (loginInfo.Email == null)
                              throw new UserInputPropertyMissingException("Email");
                          if (loginInfo.Id == null)
                              throw new UserInputPropertyMissingException("Social Id");

                          try
                          {
                              UserGoogleLogin user =
                                  await userRepo.First<UserGoogleLogin>(
                                      x =>
                                          x.Email == loginInfo.Email &&
                                          x.GoogleId == loginInfo.Id);

                              if (!user.IsActive) throw new DisableUserAccountException();

                              string jwtoken = tokenFactory.Create(user);

                              return new SuccessfulLoginResponse<string>(jwtoken);
                          }
                          catch (ItemNotFoundException<UserEmailLogin>)
                          {
                              throw new UnauthorizedAccessException(
                                  "Invalid google user, you need to register first.");
                          }
                          catch (DisableUserAccountException)
                          {
                              throw new UnauthorizedAccessException(
                                  "Your account has been disabled. Please contact your administrator for help.");
                          }
                      };
        }
    }
}