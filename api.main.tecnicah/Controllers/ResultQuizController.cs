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
    /// <summary>
    /// Respuesta de Quiz
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResultQuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IResultRepository _resultRepository;
        private readonly ICatClaveDiagnosticRepository _catClaveDiagnosticRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public ResultQuizController(
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
        
        // Trae Coincidencia de diagnostico
        // GET api/<ResultQuizController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<CatClaveDiagnosticDto>> GetById(int id)
        {
            var response = new ApiResponse<CatClaveDiagnosticDto>();
            try
            {

                if (_quizRepository.Exists(c => c.Id == id))
                {
                    var keys = _catClaveDiagnosticRepository.GetAll().Select(c => new { 
                        chars = c.KeyCode.ToCharArray(),
                        c.Id }
                    ).ToList();
                    var _quiz = _mapper.Map<QuizDto>(_quizRepository.Find(c => c.Id == id));
                    var keyValue = _quiz.Name.ToCharArray();
                    foreach (var key in keys) 
                    {
                        Array.Sort(key.chars);
                        Array.Sort(keyValue);
                        if (key.chars.SequenceEqual(keyValue))
                        {
                            response.Result = _mapper.Map<CatClaveDiagnosticDto>(
                                _catClaveDiagnosticRepository.Find(f => f.Id.Equals(key.Id))
                                );
                        }
                    }
                }

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
