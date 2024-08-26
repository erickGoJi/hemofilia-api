using api.main.tecnicah.ActionFilter;
using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.RequestSupport;
using AutoMapper;
using biz.main.tecnicah.Repository.RequestSupport;
using biz.main.tecnicah.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestSupportController : ControllerBase
    {
        private readonly IRequestSupportRepository _requestSupportRepository;
        private ILoggerManager _loggerManager;
        private IMapper _mapper;

        public RequestSupportController(IRequestSupportRepository requestSupportRepository, ILoggerManager loggerManager, IMapper mapper)
        {
            _requestSupportRepository = requestSupportRepository;
            _loggerManager = loggerManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<RequestSupportDto>>> AddRequestSupport([FromBody] RequestSupportAdd request)
        {
            var response =  new ApiResponse<RequestSupportDto>();
            try
            {
                var res = await _requestSupportRepository.AddAsyn(new biz.main.tecnicah.Entities.RequestSupport
                {
                    Id = new Guid(),
                    CreatedDate = DateTime.Now,
                    CreatedBy = request.UserId,
                    UserId = request.UserId,
                    EventType = request.EventType,
                });
                response.Success = true;
                response.Message = "Operation was success";
                response.Result = _mapper.Map<RequestSupportDto>(res);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }
            return StatusCode(201, response);
        }
    }
}
