using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Ligg.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("sys/[controller]/[action]")]
    [ApiController]
    [AuthorizationFilter]
    public class TenantController : BaseController
    {
        private readonly TenantService _service;
        public TenantController(TenantService tenantService)
        {
            _service = tenantService;
        }

        [HttpGet]
        public async Task<TResult<ShowTenantDto>> GetCurrentTenant(string code)
        {
            var currentTenant = _service.GetCurrentShowDtoAsync().Result;
            return new TResult<ShowTenantDto>(1, currentTenant);

        }

    }
}
