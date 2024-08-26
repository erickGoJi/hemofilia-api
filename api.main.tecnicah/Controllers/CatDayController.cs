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
    public class CatDayController : ControllerBase
    {
        private readonly ICatDayRepository _catDayRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CatDayController(ICatDayRepository catDay, IMapper mapper, ILoggerManager logger)
        {
            _catDayRepository = catDay;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<CatDayDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatDayDto>>();
            try
            {
                response.Result = _mapper.Map<List<CatDayDto>>(_catDayRepository.GetAll());
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
