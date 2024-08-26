using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Catalogos;
using AutoMapper;
using biz.main.tecnicah.Repository.Catalogos;
using Microsoft.AspNetCore.Mvc;
using biz.main.tecnicah.Services.Logger;



namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatClaveDiagnosticController : ControllerBase
    {
        private readonly ICatClaveDiagnosticRepository _catClaveDiagnosticRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CatClaveDiagnosticController(ICatClaveDiagnosticRepository catClaveDiagnosticRepository, 
            IMapper mapper,
            ILoggerManager logger)
        {
            _catClaveDiagnosticRepository = catClaveDiagnosticRepository;
            _mapper = mapper;
            _logger = logger;

        }


        [HttpGet]
        public ActionResult<ApiResponse<List<CatClaveDiagnosticDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatClaveDiagnosticDto>>();

            try
            {
                response.Result = _mapper.Map<List<CatClaveDiagnosticDto>>(_catClaveDiagnosticRepository.GetAll());
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }
    }
}
