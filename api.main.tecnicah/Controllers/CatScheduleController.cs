﻿using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Catalogos;
using AutoMapper;
using biz.main.tecnicah.Repository.Catalogos;
using Microsoft.AspNetCore.Mvc;
using biz.main.tecnicah.Services.Logger;


namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatScheduleController : ControllerBase
    {
        private readonly ICatScheduleRepository _catScheduleRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CatScheduleController(ICatScheduleRepository catSchedule, IMapper mapper, ILoggerManager logger)
        {
            _catScheduleRepository = catSchedule;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public ActionResult<ApiResponse<List<CatScheduleDto>>> GetAll()
        {
            var response = new ApiResponse<List<CatScheduleDto>>();
            try
            {
                response.Result = _mapper.Map<List<CatScheduleDto>>(_catScheduleRepository.GetAll());
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
