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
    public class CatStateController : ControllerBase
    {
        private readonly ICatStateRepository _catStateRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        public CatStateController(
            ICatStateRepository catStateRepository,
            IMapper mapper,
            ILoggerManager loggerManager)
        {
            _catStateRepository = catStateRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }
        /// <summary>
        /// Method: GET
        /// Returns catalogue state
        /// </summary>
        /// <returns>List Object with all states of the republic</returns>
        [HttpGet]
        public ActionResult<ApiResponse<List<CatStateDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatStateDto>>();

            try
            {
                response.Result = _mapper.Map<List<CatStateDto>>(_catStateRepository.GetAll());
            }
            catch (Exception ex)
            {
                response.Result = new List<CatStateDto>();
                response.Success = false;
                response.Message = ex.ToString();
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }
    }
}
