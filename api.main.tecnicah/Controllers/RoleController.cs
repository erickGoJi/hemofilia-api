using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Catalogos;
using AutoMapper;
using biz.main.tecnicah.Repository.Catalogos;
using biz.main.tecnicah.Services.Logger;
using dal.main.tecnicah.Repository.Catalogos;
using Microsoft.AspNetCore.Mvc;

namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ICatRoleRepository _carRoleRepository; 
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public RoleController(ICatRoleRepository carRoleRepository, IMapper mapper, ILoggerManager loggerManager)
        {
            _carRoleRepository = carRoleRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        /// <summary>
        /// Method: GET
        /// Returns role catalogs
        /// </summary>
        /// <returns>List Object with all roles</returns>
        [HttpGet]
        public ActionResult<ApiResponse<List<CatRoleDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatRoleDto>>();

            try
            {
                response.Result = _mapper.Map<List<CatRoleDto>>(_carRoleRepository.GetAll());
            }
            catch (Exception ex)
            {
                response.Result = new List<CatRoleDto>();
                response.Success = false;
                response.Message = ex.ToString();
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }
    }
}
