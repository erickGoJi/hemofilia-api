using api.main.tecnicah.ActionFilter;
using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.ObjectiveSignUp;
using api.main.tecnicah.Models.User;
using AutoMapper;
using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Repository.ObjectiveSignUp;
using biz.main.tecnicah.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectiveSignUpController : ControllerBase
    {
        private readonly IObjectiveSignUpRepository _objectiveSignUpRepository;
        private ILoggerManager _loggerManager;
        private IMapper _mapper;

        public ObjectiveSignUpController(IObjectiveSignUpRepository objectiveSignUpRepository, ILoggerManager loggerManager, IMapper mapper)
        {
            _objectiveSignUpRepository = objectiveSignUpRepository;
            _loggerManager = loggerManager;
            _mapper = mapper;
        }

        [HttpGet("", Name = "Get-Objective-Sign-Up")]
        public async Task<ActionResult<ApiResponse<ObjectiveSignUpDto>>> Get()
        {
            var response = new ApiResponse<ObjectiveSignUpDto>();
            try
            {
                response.Result = _mapper.Map<ObjectiveSignUpDto>(_objectiveSignUpRepository.GetAll().FirstOrDefault());
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Internal server error";
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpPut("", Name = "Put-Objective-Sign-Up")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public ActionResult<ApiResponse<ObjectiveSignUpDto>> Update(ObjectiveSignUpDto item)
        {
            var response = new ApiResponse<ObjectiveSignUpDto>();

            try
            {
                var objectiveSignUp = _objectiveSignUpRepository.Find(c => c.Id == item.Id);
                response.Success = true;
                if (objectiveSignUp == null)
                {
                    _objectiveSignUpRepository.Add(_mapper.Map<ObjectiveSignUp>(item));
                    _objectiveSignUpRepository.Save();
                    response.Message = "Record was added success";
                }

                var osuUpdate = _mapper.Map<ObjectiveSignUp>(item);
                _objectiveSignUpRepository.Update(osuUpdate, item.Id);
                _objectiveSignUpRepository.Save();
                response.Message = "Update record was success";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Internal server error";
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

    }
}
