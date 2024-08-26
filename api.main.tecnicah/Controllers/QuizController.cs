using api.main.tecnicah.ActionFilter;
using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Calendario;
using api.main.tecnicah.Models.Diagnostico;
using api.main.tecnicah.Models.User;
using AutoMapper;
using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Repository.Calendario;
using biz.main.tecnicah.Repository.Diagnostico;
using biz.main.tecnicah.Repository.User;
using biz.main.tecnicah.Services.Logger;
using dal.main.tecnicah.Repository.Calendario;
using dal.main.tecnicah.Repository.User;
using dal.main.tecnicah.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using biz.main.tecnicah.Repository.Catalogos;
using api.main.tecnicah.Models.Catalogos;

namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IResultRepository _resultRepository;
        private readonly ICatClaveDiagnosticRepository _catClaveDiagnosticRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public QuizController(
           IQuizRepository quizRepository,
           IMapper mapper,
           ILoggerManager logger,
           IResultRepository resultRepository,
           ICatClaveDiagnosticRepository catClaveDiagnosticRepository
        )
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
            _logger = logger;
            _resultRepository = resultRepository;
            _catClaveDiagnosticRepository = catClaveDiagnosticRepository;
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<QuizDto>>> GetAll()
        {
            var response = new ApiResponse<List<QuizDto>>();
            try
            {
                var allquiz = _quizRepository.GetAll();
                response.Result = _mapper.Map<List<QuizDto>>(allquiz);
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


        [HttpGet("{id}")]
        public ActionResult<ApiResponse<QuizDto>> GetById(int id)
        {
            var response = new ApiResponse<QuizDto>();

            try
            {
                QuizDto allQuiz = new QuizDto();
                var _quiz = _mapper.Map<QuizDto>(_quizRepository.Find(c => c.Id == id));
                var _result = _mapper.Map<List<ResultDto>>(_resultRepository.FindAll(c => c.QuizId == id));
                allQuiz = _quiz;
                if (_result != null)
                    allQuiz.Results = _result;
                response.Result = _mapper.Map<QuizDto>(allQuiz);

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


        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<QuizDto>>> Post([FromBody] QuizDto item)
        {
            ApiResponse<QuizDto> response = new ApiResponse<QuizDto>();
            try
            {
                response.Result = _mapper.Map<QuizDto>(await _quizRepository.AddAsyn(_mapper.Map<Quiz>(item)));
                response.Success = true;
                response.Message = "Record was added success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Success = false;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
            return StatusCode(201, response);
        }

       
    }
}
