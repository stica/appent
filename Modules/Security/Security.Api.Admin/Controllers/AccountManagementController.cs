using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Api.Admin.Contract.Dtos;
using Security.Api.Admin.Contract.Requests;
using Security.Api.Admin.Contract.Responses;
using Security.Domain.Contract.Commands;
using Security.Domain.Managers;
using Start.Infrastructure;
using System.Collections.Generic;

namespace Security.Api.Admin.Controllers
{
    [Authorize]
    [Route("api/admin/account")]
    [ApiController]
    public class AccountManagementController : BaseController
    {
        private readonly ISecurityManager _securityManager;
        private readonly IMapper _mapper;

        public AccountManagementController(ISecurityManager securityManager, IMapper mapper)
        {
            _securityManager = securityManager;
            _mapper = mapper;
        }

        [Route("company")]
        [HttpPost]
        public IActionResult CreateCompany(AddCompanyRequest request)
        {
            var command = _mapper.Map<AddCompanyCommand>(request);
            var result = _securityManager.CreateCompany(command);
            return Ok(result);
        }

        [Route("company")]
        [HttpPut]
        public IActionResult UpdateCompany(UpdateCompanyRequest request)
        {
            var command = _mapper.Map<UpdateCompanyCommand>(request);
            var result = _securityManager.UpdateCompany(command);
            return Ok(result);
        }

        [Route("company/users")]
        [HttpGet]
        public IActionResult GetUsersForCompany()
        {
            var users = _securityManager.GetUsersForCompany(UserContext.Current.CompanyId);
            var result = _mapper.Map<List<UserResponse>>(users);
            return Ok(result);
        }

        [Route("company")]
        [HttpGet]
        public IActionResult GetCompanyData()
        {
            var users = _securityManager.GetCompanyView(UserContext.Current.CompanyId);
            var result = _mapper.Map<List<UserResponse>>(users);
            return Ok(result);
        }

        [Route("company/user/{userId}")]
        [HttpGet]
        public IActionResult GetUserData(int userId)
        {
            var users = _securityManager.GetUser(userId);
            var result = _mapper.Map<UserDto>(users);
            return Ok(result);
        }
    }
}
