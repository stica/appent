using Security.Domain.Contract.Commands;
using Security.Domain.Contract.Entities;
using Security.Domain.Contract.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Security.Domain.Managers
{
    public interface ISecurityManager
    {
        Task<LoginSession> CreateLoginSession(CreateLoginSession command);

        User GetUserByUserName(string userName);

        LoginSession GetLoginSessionAnonymous(string refreshToken);

        bool VarifyUserCredentials(CreateLoginSession command);

        bool UpdateUser(UpdateUserCommand user);

        int CreateUser(AddUserCommand command);

        int CreateCompany(AddCompanyCommand command);

        bool UpdateCompany(UpdateCompanyCommand command);

        List<CompanyView> GetCompanies();

        Company GetCompany(int companyId);

        CompanyView GetCompanyView(int companyId);

        List<UserView> GetUsersForCompany(int companyId);

        User GetUser(int userId);

        LoginSession GetAccessToken(string refreshToken, string ipAddess);

        List<Policy> GetUserPolicies(int userId);
    }
}
