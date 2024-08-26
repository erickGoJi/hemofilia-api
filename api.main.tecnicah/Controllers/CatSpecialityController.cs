using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Catalogos;
using AutoMapper;
using biz.main.tecnicah.Repository.Catalogos;
using Microsoft.AspNetCore.Mvc;
using biz.main.tecnicah.Services.Logger;


namespace api.main.tecnicah.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatSpecialityController : ControllerBase
    {
        private readonly ICatSpecialityRepository _catAreaRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;


        public CatSpecialityController(ICatSpecialityRepository catAreaRepository, IMapper mapper, ILoggerManager logger)
        {
            _catAreaRepository = catAreaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<CatSpecialityDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatSpecialityDto>>();

            try
            {
                response.Result = _mapper.Map<List<CatSpecialityDto>>(_catAreaRepository.GetAll());
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
