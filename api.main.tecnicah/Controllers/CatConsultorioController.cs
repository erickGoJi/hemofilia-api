using api.main.tecnicah.ActionFilter;
using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Catalogos;
using AutoMapper;
using biz.main.tecnicah.Repository.Catalogos;
using biz.main.tecnicah.Services.Logger;
using Microsoft.AspNetCore.Mvc;


namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatConsultorioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly ICatConsultorioRepository _catConsultorioRepository;

        public CatConsultorioController(ICatConsultorioRepository catConsultorio,
            IMapper mapper,
            ILoggerManager logger)
        {
            _catConsultorioRepository = catConsultorio;
            _mapper = mapper;
            _logger = logger;

        }

        [HttpGet]
        public ActionResult<ApiResponse<List<CatConsultorioDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatConsultorioDto>>();

            try
            {
                response.Result = _mapper.Map<List<CatConsultorioDto>>(_catConsultorioRepository.GetAll());
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }
    }
}
