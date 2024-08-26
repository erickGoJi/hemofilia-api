using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Dashboard;
using api.main.tecnicah.Models.Diagnosis;
using api.main.tecnicah.Models.State;
using api.main.tecnicah.Models.User;
using api.rebel_wings.Extensions;
using AutoMapper;
using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Models.Diagnosis;
using biz.main.tecnicah.Repository.Catalogos;
using biz.main.tecnicah.Repository.Diagnostico;
using biz.main.tecnicah.Repository.ObjectiveSignUp;
using biz.main.tecnicah.Repository.RequestSupport;
using biz.main.tecnicah.Repository.User;
using biz.main.tecnicah.Services.Logger;
using dal.main.tecnicah.Repository.Diagnostico;
using dal.main.tecnicah.Services.Logger;
using Microsoft.AspNetCore.Mvc;


namespace api.main.tecnicah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private ILoggerManager _loggerManager;
        private IMapper _mapper;
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICatStateRepository _catStateRepository;
        private readonly IObjectiveSignUpRepository _objectiveSignUpRepository;
        private readonly ICatSpecialityRepository _catSpecialityRepository;
        private readonly ICatConsultorioRepository _catConsultorioResporitory;
        private readonly IRequestSupportRepository _requestSupportRepository;
        //private readonly I

        public DashboardController(IQuizRepository quizRepository, IMapper mapper, ILoggerManager loggerManager, IUserRepository userRepository,
            ICatStateRepository catStateRepository, IObjectiveSignUpRepository objectiveSignUpRepository, ICatConsultorioRepository catConsultorioResporitory, ICatSpecialityRepository catSpecialityRepository, IRequestSupportRepository requestSupportRepository)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _userRepository = userRepository;
            _catStateRepository = catStateRepository;
            _objectiveSignUpRepository = objectiveSignUpRepository;
            _catConsultorioResporitory = catConsultorioResporitory;
            _catSpecialityRepository = catSpecialityRepository;
            _requestSupportRepository = requestSupportRepository;
        }
        // GET: api/<DashboardController>
        [HttpGet("main", Name = "main")]
        public async Task<ActionResult<ApiResponse<DashboardMain>>> GetMain([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var response = new ApiResponse<DashboardMain>();
            try
            {
                var users = _userRepository.GetAllIncluding(i => i.Speciality, c => c.Consultorio);
                var offices = _catConsultorioResporitory.GetAll();
                var states = _catStateRepository.GetAllIncluding(i => i.Users);

                response.Result = new DashboardMain();
                
                response.Result.Goal = _objectiveSignUpRepository.GetAll().First().ObjectiveSignUp1;
                response.Result.SignUpTotal = await _userRepository.CountByAsync(c => c.CreatedDate >= start && c.CreatedDate <= end);
                response.Result.SignUpMonth = await _userRepository.CountByAsync(c => c.CreatedDate!.Value.Month == end.Month && c.CreatedDate.Value.Year == end.Year);

                response.Result.GoalPercentage = Math.Abs(Decimal.Divide(response.Result.SignUpTotal * 100,response.Result.Goal));

                response.Result.diagnosisTops = _mapper.Map<List<DiagnosisTopDto>>(await _quizRepository.DiagnosisTop(start, end));
                response.Result.topInstitutions = users.All(a => a.ConsultingType.Length > 0)
                    ? users
                        .Where(c => c.CreatedDate >= start && c.CreatedDate <= end)
                        .GroupBy(g => g.ConsultingType).Select(s =>
                    new TopInstitution
                    {
                        Name = offices.FirstOrDefault(f => f.Id.Equals(Convert.ToInt32(s.First().ConsultingType)))!.Consultorio,
                        Value = s.Count()
                    })
                        .OrderByDescending(o => o.Value)
                        .Take(5)
                        .ToList()
                    : new List<TopInstitution>();
                response.Result.topStates = states.Select(s => new MainStates
                    {
                        Name = s.Name,
                        Value = s.Users.Count()
                    })
                    .OrderByDescending(o => o.Value)
                    .Take(5)
                    .ToList();
                response.Result.TopSpecialities = users.All(a => a.SpecialityId.HasValue)
                    ? users
                        .Where(c => c.CreatedDate >= start && c.CreatedDate <= end)
                        .GroupBy(g => g.SpecialityId!.Value).Select(s =>
                        new TopSpeciality
                        {
                            Name = s.FirstOrDefault()!.Speciality.Speciality,
                            Value = s.Count()
                        })
                        .OrderByDescending(o => o.Value)
                        .Take(3)
                        .ToList()
                    : new List<TopSpeciality>();
                // Fidelity
                response.Result.FidelityLevel = new List<FidelityLevelDto>();
                response.Result.FidelityLevel = _mapper.Map<List<FidelityLevelDto>>(await _userRepository.GetFidelitiesAsync(start, end));
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }
        
        // GET: api/<DashboardController>
        [HttpGet("diagnosis-generate" ,Name = "diagnosis-generate" )]
        public async Task<ActionResult<ApiResponse<DiagnosisGenerate>>> GetDiagnosisGenerate([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var response = new ApiResponse<DiagnosisGenerate>();

            try
            {
                response.Result = new DiagnosisGenerate();
                var keys = _mapper.Map<List<DiagnosisTopDto>>(await _quizRepository.DiagnosisTop(start, end));
                response.Result.Top = keys;
                var diagnosisFound = await _quizRepository.DiagnosisFound(start, end);
                response.Result.Found = diagnosisFound.Count(c => c.Equals(true));
                response.Result.NoResult = diagnosisFound.Count(c => c.Equals(false));
                response.Result.Made = diagnosisFound.Count;
                response.Result.FoundPercentage = Math.Abs(Decimal.Divide(response.Result.Found * 100, response.Result.Made));
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("medic-sign-up", Name = "medic-sign-up")]
        public async Task<ActionResult<ApiResponse<MedicSignUpDashboardDto>>> GetMedicSignUp([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var response = new ApiResponse<MedicSignUpDashboardDto>();
            try
            {
                response.Result = new MedicSignUpDashboardDto();
                // Goal Sign-Up
                response.Result.ObjectiveSignUp = _objectiveSignUpRepository.GetAll().First().ObjectiveSignUp1;
                // Medic Sign-Up
                response.Result.MedicSignUp =
                    await _userRepository.CountByAsync(c => 
                        c.RoleId.Equals(1) || c.RoleId.Equals(4)
                        && c.CreatedDate >= start && c.CreatedDate <= end);
                // Medic Active
                response.Result.MedicActive =
                    await _userRepository.GetMedicActive(start, end);
                // Medic Inactive
                response.Result.MedicInactive =
                    await _userRepository.GetMedicInactive(start, end);
                // Medic Sign-Up Percentage
                response.Result.MedicSignUpPercentage =
                    Decimal.Divide(response.Result.MedicSignUp * 100, response.Result.ObjectiveSignUp);
                // Request Support
                var supports = await _requestSupportRepository.GetRequestSupportByBetweenDate(start, end.AbsoluteEnd());
                var requestList =new List<MedicRequestSupport>();
                requestList.Add(new MedicRequestSupport { Name = "Consulta a especialista", Value = Decimal.Divide(supports * 100, response.Result.MedicActive) });
                requestList.Add(new MedicRequestSupport { Name = "Sin consulta a especialista", Value = Math.Abs(Decimal.Divide((supports - response.Result.MedicActive) * 100, response.Result.MedicActive)) });
                response.Result.MedicRequestSupports = requestList.ToArray();
                // Fidelity
                response.Result.FidelityLevel = new List<FidelityLevelDto>();
                response.Result.FidelityLevel = _mapper.Map<List<FidelityLevelDto>>(await _userRepository.GetFidelitiesAsync(start, end));
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                _loggerManager.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("specialities-office", Name = "specialities-office")]
        public async Task<ActionResult<ApiResponse<SpecialityOffice>>> GetSpecialitiesOffice([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var response = new ApiResponse<SpecialityOffice>();
            try
            {
                response.Result = new SpecialityOffice();
                var users = _userRepository.GetAllIncluding(i=>i.Speciality, c=> c.Consultorio);
                var offices = _catConsultorioResporitory.GetAll();
                
                response.Result.Institution = await _catConsultorioResporitory.CountAsync();
                response.Result.Speciality = await _catSpecialityRepository.CountAsync();

                response.Result.InstitutionNoSignUp = await _catConsultorioResporitory.CountByAsync(c=> c.Users.Any());
                response.Result.SpecialityNoSignUp = await _catSpecialityRepository.CountByAsync(c => c.Users.Any());

                response.Result.InstitutionNoSignUpPercentage = Math.Abs(Decimal.Divide(response.Result.InstitutionNoSignUp * 100, response.Result.Institution));
                
                response.Result.InstitutionsCollection = users.All(a=>a.ConsultingType.Length > 0) 
                    ? users
                        .Where(c => c.CreatedDate >= start && c.CreatedDate <= end)
                        .GroupBy(g => g.ConsultingType).Select(s =>
                    new TopInstitution
                    {
                        Name = offices.FirstOrDefault(f=>f.Id.Equals(Convert.ToInt32(s.First().ConsultingType) ) )!.Consultorio,
                        Value = s.Count()
                    })
                        .OrderByDescending(o=>o.Value)
                        .Take(5)
                        .ToList()
                    : new List<TopInstitution>();
                response.Result.TopSpecialities = new List<TopSpeciality>();
                response.Result.TopSpecialities = users.All(a => a.SpecialityId.HasValue) 
                    ? users
                        .Where(c => c.CreatedDate >= start && c.CreatedDate <= end)
                        .GroupBy(g => g.SpecialityId!.Value).Select(s =>
                        new TopSpeciality
                        {
                            Name = s.FirstOrDefault()!.Speciality.Speciality,
                            Value = s.Count()
                        })
                        .OrderByDescending(o=>o.Value)
                        .Take(5)
                        .ToList() 
                    : new List<TopSpeciality>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return Ok(response);
        }

        [HttpGet("states", Name = "states")]
        public async Task<ActionResult<ApiResponse<StateDashboardDto>>> GetStates([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var response = new ApiResponse<StateDashboardDto>();
            try
            {
                response.Result = new StateDashboardDto();
                var states = _catStateRepository.GetAllIncluding(i => i.Users);
                response.Result.States = await _catStateRepository.CountAsync();
                response.Result.StateMostSignUp = states.OrderByDescending(o => o.Users.Count).First().Name;
                response.Result.StateMinusSignUp = states.OrderBy(o => o.Users.Count).First().Name;
                response.Result.StatesNoSignUp = states.Count(c=> !c.Users.Any() );
                response.Result.StatesNoSignUpPercentage = Math.Abs(Decimal.Divide(response.Result.StatesNoSignUp * 100, response.Result.States)); 
                response.Result.MainStates = states.Select(s => new MainStates
                {
                    Name = s.Name,
                    Value = s.Users.Count()
                })
                .OrderByDescending(o=>o.Value)
                .Take(5)
                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return Ok(response);
        }

    }
}
