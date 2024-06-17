using Security.Domain.Contract.Commands;
using Security.Domain.Contract.Entities;
using Security.Domain.Util;
using Start;
using Start.Common.Utils;
using Start.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Security.Domain.Managers
{
    public class SecurityManager : BaseManager, ISecurityManager
    {
        private JwtTokenGenerator _jwtTokenGenerator;

        public SecurityManager(AppSettings appSettings)
            : base(appSettings)
        {
            _jwtTokenGenerator = new JwtTokenGenerator(appSettings);
        }

        public bool VarifyUserCredentials(CreateLoginSession command)
        {
            var user = GetUserByUserName(command.UserName);

            if (user == null)
            {
                throw new SecurityException("User not found.");
            }

            return HashGenerator.VerifyPassword(command.Password, user.Password);
        }

        public LoginSession GetAccessToken(string refreshToken, string ipAddess)
        {
            return ExecuteInTransactionWithResult((ctx) =>
            {
                var loginSession = GetLoginSessionAnonymous(refreshToken);
                var user = GetUser(loginSession.UserId);

                if (loginSession.IpAddress != ipAddess)
                {
                    _logger.Info("IpAddress missmatch");
                }

                if (loginSession.Expires < DateTime.UtcNow)
                {
                    _logger.Info("Session expired");

                    return null;
                }

                loginSession.AccessToken = _jwtTokenGenerator.CreateJwtToken(loginSession.UserId.ToString(), user.UserName, user.IsAdmin);

                return loginSession;
            }, MethodInfo.Create("GetAccressToken"));
        }

        public async Task<LoginSession> CreateLoginSession(CreateLoginSession command)
        {
            var user = GetUserByUserName(command.UserName);

            if (user == null)
            {
                throw new SecurityException("User not exists.");
            }

            var jwtToken = _jwtTokenGenerator.CreateJwtToken(user.Id.ToString(), user.UserName, user.IsAdmin);

            var session = new LoginSession
            {
                AccessToken = jwtToken,
                RefreshToken = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.Add(new TimeSpan(8, 0, 0)),
                IpAddress = command.IpAddress,
                UserId = user.Id
            };

            var sessionId = ExecuteWithResult((ctx) =>
            {
                return ctx.Insert<LoginSession>(session);
            }, MethodInfo.Create("CreateLoginSession", session.UserId));


            session.Id = (int)sessionId;
            return session;
        }

        public User GetUserByUserName(string userName)
        {
            return ExecuteWithResult((ctx) =>
            {
                return ctx.QueryFirstOrDefault<User>(
                    "Select * from [SecurityManagement].[User] where UserName = @userName",
                    new { userName });
            }, MethodInfo.Create("GetUserByUserName", userName));
        }

        public LoginSession GetLoginSessionAnonymous(string refreshToken)
        {
            return ExecuteWithResult((ctx) =>
            {
                return ctx.QueryFirstOrDefault<LoginSession>(
                    "Select * from [SecurityManagement].[LoginSession] where RefreshToken = @refreshToken",
                    new { refreshToken = refreshToken });
            }, MethodInfo.Create("GetLoginSessionAnonymous", refreshToken));
        }

        public bool UpdateUser(UpdateUserCommand command)
        {
            return ExecuteWithResult(ctx =>
            {
                var userToUpdate = ctx.Get<User>(command.Id);

                if (userToUpdate == null)
                {
                    throw new SecurityException("User not exists");
                }

                userToUpdate.Password = command.Password;
                userToUpdate.PhoneNumber = command.PhoneNumber;
                userToUpdate.LastName = command.LastName;
                userToUpdate.FirstName = command.FirstName;
                userToUpdate.IsAdmin = command.IsAdmin;

                return ctx.Update(userToUpdate);
            }, MethodInfo.Create("UpdateUser"));
        }

        public int CreateUser(AddUserCommand command)
        {
            return ExecuteWithResult(ctx =>
            {
                return (int)ctx.Insert<User>(new User
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    IsAdmin = false,
                    Password = command.Password,
                    PhoneNumber = command.PhoneNumber,
                    UserName = command.UserName,
                    IsConfirmed = false,
                });
            }, MethodInfo.Create("CreateUser"));
        }

        public User GetUser(int userId)
        {
            return ExecuteWithResult(ctx =>
            {
                return ctx.Get<User>(userId);
            }, MethodInfo.Create("GetUser"));
        }

        public List<Policy> GetUserPolicies(int userId)  
        {
        return ExecuteWithResult(ctx =>
        {
            return ctx.Query<Policy>(
            @"SELECT * 
                FROM [SecurityManagement].[Policy] P
                INNER JOIN [SecurityManagement].[UserGroupPolicy] UGP 
                ON UGP.PolicyId = P.Id
                INNER JOIN [SecurityManagement].[UserUserGroup] UUP  
                ON UUP.UserGroupId = UGP.UserGroupId
                WHERE UUP.UserId = @userId",
            new { userId = userId })
            .ToList();
        }, 
        MethodInfo.Create("GetUser"));
        }
    }
}
