using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Api.Public.Contract.Dtos;
using Security.Api.Public.Contract.Requests;
using Security.Api.Public.Contract.Responses;
using Security.Domain.Contract.Commands;
using Security.Domain.Managers;
using Start.Infrastructure;
using System.Collections.Generic;

namespace Security.Api.Public.Controllers
{
    [Authorize]
    [Route("api/public/account")]
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

        [Route("user")]
        [HttpPost]
        public IActionResult CreateUser(AddUserRequest request)
        {
            var command = _mapper.Map<AddUserCommand>(request);
            var result = _securityManager.CreateUser(command);
            return Ok(result);
        }

        [Route("user")]
        [HttpPut]
        public IActionResult UpdateUser(UpdateUserRequest request)
        {
            var command = _mapper.Map<UpdateUserCommand>(request);
            var result = _securityManager.UpdateUser(command);
            return Ok(result);
        }

        [Route("companies")]
        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _securityManager.GetCompanies();
            var result = _mapper.Map<List<CompanyResponse>>(companies);
            return Ok(result);
        }

        [Route("company/{companyId}/users")]
        [HttpGet]
        public IActionResult GetUsersForCompany(int companyId)
        {
            var users = _securityManager.GetUsersForCompany(companyId);
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
