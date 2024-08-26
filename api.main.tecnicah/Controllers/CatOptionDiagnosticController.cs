using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Catalogos;
using AutoMapper;
using biz.main.tecnicah.Repository.Catalogos;
using Microsoft.AspNetCore.Mvc;
using biz.main.tecnicah.Services.Logger;
using dal.main.tecnicah.Repository.Catalogos;


namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatOptionDiagnosticController : ControllerBase
    {
        private readonly ICatOptionDiagnosticRepository _catOptionDiagnosticRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CatOptionDiagnosticController(ICatOptionDiagnosticRepository catOptionDiagnosticController,
            IMapper mapper,
            ILoggerManager logger)
        {
            _catOptionDiagnosticRepository = catOptionDiagnosticController;
            _mapper = mapper;
            _logger = logger;

        }


        [HttpGet]
        public ActionResult<ApiResponse<List<CatOptionDiagnosticDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatOptionDiagnosticDto>>();

            try
            {
                response.Result = _mapper.Map<List<CatOptionDiagnosticDto>>(_catOptionDiagnosticRepository.GetAll());
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
