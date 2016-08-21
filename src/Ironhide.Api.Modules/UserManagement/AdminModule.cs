using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AcklenAvenue.Commands;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IUserRepository<User> readOnlyRepository, IMapper mapper,
            ICommandDispatcher commandDispatcher, IUserSessionFactory userSessionFactory)
        {
            Get["/users", true] =
                async (a, c) =>
                      {
                          this.RequiresClaims(new[] {"Administrator"});
                          var request = this.Bind<AdminUsersRequest>();

                          ParameterExpression parameter = Expression.Parameter(typeof (User), "User");
                          Expression<Func<User, object>> mySortExpression =
                              Expression.Lambda<Func<User, object>>(Expression.Property(parameter, request.Field),
                                  parameter);

                          IQueryable<User> users =
                              (await readOnlyRepository.Query(x => x.Name != Context.CurrentUser.UserName))
                                  .AsQueryable();

                          IOrderedQueryable<User> orderedUsers = users.OrderBy(mySortExpression);

                          IQueryable<User> pagedUsers =
                              orderedUsers.Skip(request.PageSize*(request.PageNumber - 1)).Take(request.PageSize);

                          List<AdminUserResponse> usersList = mapper
                              .Map<IQueryable<User>, IEnumerable<AdminUserResponse>>(pagedUsers).ToList();

                          return usersList;
                      };

            Put["/users/{userId:guid}/enable", true] =
                async (p, c) =>
                      {
                          this.RequiresClaims(new[] {"Administrator"});
                          Guid userId = Guid.Parse((string) p.userId);
                          await
                              commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                                  new EnableUser(userId));
                          return null;
                      };

            Put["/users/{userId:guid}/disable", true] =
                async (p, c) =>
                      {
                          this.RequiresClaims(new[] {"Administrator"});
                          Guid userId = Guid.Parse((string) p.userId);
                          await
                              commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                                  new DisableUser(userId));
                          return null;
                      };

            Get["/users/{userId:guid}", true] =
                async (p, c) =>
                      {
                          this.RequiresClaims(new[] { "Administrator" });
                          Guid userId = Guid.Parse((string)p.userId);
                          User user = await readOnlyRepository.GetById(userId);
                          AdminUserResponse mappedUser = mapper
                              .Map<User, AdminUserResponse>(user);
                          return mappedUser;
                      };

            Put["/users/{userId:guid}", true] =
                async (p, c) =>
                      {
                          this.RequiresClaims(new[] { "Administrator" });
                          var request = this.Bind<AdminUpdateUserRequest>();
                          Guid userId = Guid.Parse((string) p.userId);
                          await
                              commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                                  new UpdateUserProfile(userId, request.Name, request.Email));
                          return null;
                      };
        }
    }
}