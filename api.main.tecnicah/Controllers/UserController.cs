using api.main.tecnicah.ActionFilter;
using api.main.tecnicah.Models.ApiResponse;
using api.main.tecnicah.Models.Calendario;
using api.main.tecnicah.Models.User;
using AutoMapper;
using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Paged;
using biz.main.tecnicah.Repository.Calendario;
using biz.main.tecnicah.Repository.User;
using biz.main.tecnicah.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Calendar = biz.main.tecnicah.Entities.Calendar;
using static api.main.tecnicah.Extensions.StringExtensions;
using api.main.tecnicah.Models.PagedList;

namespace api.main.tecnicah.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IUserRepository _userRepository;
        private readonly ICalendarioRepository _calendarioRepository;

        public UserController(
            IUserRepository userRepository,
            IMapper mapper,
            ILoggerManager logger,
            ICalendarioRepository calendarioRepository
         )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _calendarioRepository = calendarioRepository;

        }
      
        [HttpGet]
        public ActionResult<ApiResponse<List<UserDto>>> GetAll()
        {
            var response = new ApiResponse<List<UserDto>>();

            try
            {
                var user = _userRepository.GetAll();

                response.Result = _mapper.Map<List<UserDto>>(user);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public ActionResult<ApiResponse<PagedListDto<UserDto>>> GetPaged(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<PagedListDto<UserDto>>();

            try
            {
                response.Result = _mapper.Map<PagedListDto<UserDto>>
                    (_userRepository.GetAllPaged(pageNumber, pageSize));
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


        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<ApiResponse<UserCalendarDto>> GetById(int id)
        {
            var response = new ApiResponse<UserCalendarDto>();

            try
            {

                UserCalendarDto allDataUser = new UserCalendarDto();
                var _user = _mapper.Map<UserDto>(_userRepository.Find(c => c.Id == id));
                var _calendar = _mapper.Map<List<CalendarioDto>>(_calendarioRepository.FindAll(c => c.UserId == id));
                allDataUser.User = _user;
                if (_calendar != null)
                    allDataUser.Calendario = _calendar;
                response.Result = _mapper.Map<UserCalendarDto>(allDataUser);

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
        public ActionResult<ApiResponse<UserDto>> CreateUser(UserDto item)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                if (_userRepository.Exists(c => c.Email == item.Email))
                {
                    response.Success = false;
                    response.Message = $"Email: {item.Email} Already Exists";
                    return BadRequest(response);
                }
                
                if (!item.Email.IsValidEmail())
                {
                    response.Success = false;
                    response.Message = $"Email: {item.Email} is not a email valid.";
                    return BadRequest(response);
                }
                User user = _userRepository.Add(_mapper.Map<User>(item));
                response.Success = true;
                response.Result = _mapper.Map<UserDto>(user);



            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return StatusCode(201, response);
        }

        [HttpPut("Recovery_password", Name = "Recovery_password")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Recovery_password(string email)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var _user = _mapper.Map<User>(_userRepository.Find(c => c.Email == email));

                if (_user != null)
                {

                    var guid = Guid.NewGuid().ToString().Substring(0, 7);
                    var password = _userRepository.HashPassword("$" + guid);

                    _user.Password = password;
                    _user.UpdatedBy = _user.Id;
                    _user.UpdatedDate = DateTime.Now;

                    _userRepository.Update(_mapper.Map<User>(_user), _user.Id);

                    StreamReader reader = new StreamReader(Path.GetFullPath("TemplateMail/Email.html"));
                    string body = string.Empty;
                    body = reader.ReadToEnd();
                    body = body.Replace("{user}", _user.Name);
                    body = body.Replace("{username}", $"{_user.Email}");
                    body = body.Replace("{pass}", "$" + guid);

                    _userRepository.SendMail(_user.Email, body, "Recovery password");

                    response.Result = _mapper.Map<UserDto>(_user);
                    response.Success = true;
                    response.Message = "success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Hubo un error";

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPut("Change_password", Name = "Change_password")]
        public ActionResult<ApiResponse<UserDto>> Change_password(string email, string password)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var _user = _mapper.Map<User>(_userRepository.Find(c => c.Email == email));

                if (_user != null)
                {
                    var guid = Guid.NewGuid().ToString().Substring(0, 7);
                    var passwordNew = _userRepository.HashPassword(password);

                    _user.Password = passwordNew;
                    _user.UpdatedBy = _user.Id;
                    _user.UpdatedDate = DateTime.Now;
                    _userRepository.Update(_mapper.Map<User>(_user), _user.Id);

                    response.Result = _mapper.Map<UserDto>(_user);
                    response.Result.Token = _userRepository.BuildToken(_user);
                    response.Success = true;
                    response.Message = "success";

                    StreamReader reader = new StreamReader(Path.GetFullPath("TemplateMail/Email.html"));
                    string body = string.Empty;
                    body = reader.ReadToEnd();
                    body = body.Replace("{user}", _user.Name);
                    body = body.Replace("{username}", $"{_user.Email}");
                    body = body.Replace("{pass}", password);
                    _userRepository.SendMail(_user.Email, body, "Change password");
                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuario y/o contraseña incorrecta";

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPost("Login", Name = "Login")]
        public async Task<ActionResult<ApiResponse<UserReturnDto>>> Login(string email, string password)
        {
            var response = new ApiResponse<UserReturnDto>();

            try
            {
                var _user = _mapper.Map<User>(_userRepository.Find(c => c.Email == email));
                if (_user != null)
                {
                    if (_userRepository.VerifyPassword(_user.Password, password))
                    {
                        if(_user.Status == null || _user.Status == false)
                        {
                            response.Result = null;
                            response.Success = false;
                            response.Message = "Usuario no activo";
                        }
                        else
                        {
                            var userData = _mapper.Map<UserDto>(_user);

                            UserReturnDto userReturnDto = new UserReturnDto()
                            {
                                Email = userData.Email,
                                Id = userData.Id,
                                Name = userData.Name,
                                RoleId = userData.RoleId,
                                Token = userData.Token
                            };
                            response.Result = userReturnDto;
                            response.Result.Token = _userRepository.BuildToken(_user);
                            response.Success = true;
                            response.Message = "Success";
                            await _userRepository.AddLogIn(_user.Id);
                        }
                       
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Usuario y/o contraseña incorrecta";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuario y/o contraseña incorrecta";

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPost("login-admin", Name = "login-admin")]
        public async Task<ActionResult<ApiResponse<UserReturnDto>>> LoginAdmin(string email, string password)
        {
            var response = new ApiResponse<UserReturnDto>();

            try
            {
                if (await _userRepository.ExistsAsync(e=>e.Email.Equals(email) && e.RoleId == 5))
                {
                    var _user = _mapper.Map<User>(await _userRepository.FindAsync(c => c.Email == email));
                    if (_userRepository.VerifyPassword(_user.Password, password))
                    {
                        if(_user.Status is null || _user.Status.Equals(false) )
                        {
                            response.Success = false;
                            response.Message = "Usuario no activo";
                        }
                        else
                        {              
                            response.Result =  new UserReturnDto()
                            {
                                Email = _user.Email,
                                Id = _user.Id,
                                Name = _user.Name,
                                RoleId = _user.RoleId,
                                Token = ""
                            };
                            response.Result.Token = _userRepository.BuildToken(_user);
                            response.Success = true;
                            response.Message = "Success";
                            await _userRepository.AddLogIn(_user.Id);
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Usuario y/o contraseña incorrecta";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuario y/o contraseña incorrecta";

                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Internal server error";
                _logger.LogError("Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        
        [HttpPut("Edit_user", Name = "Edit_user")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public ActionResult<ApiResponse<UserDto>> Update(UserDto item)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var user = _userRepository.Find(c => c.Id == item.Id);

                if (user == null)
                {
                    response.Message = $"User id { item.Id } Not Found";
                    return NotFound(response);
                }

                item.Password = user.Password;
                var userUpdate = _mapper.Map<User>(item);
                _userRepository.Update(userUpdate, item.Id);
                _userRepository.Save();
                response.Message = "Update record was success";
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<UserDto>> Delete(int id)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var user = _userRepository.Find(c => c.Id == id);

                if (user == null)
                {
                    response.Message = $"User id { id } Not Found";
                    return NotFound(response);
                }

                _userRepository.Delete(user);
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    var number = sqlException.Number;
                    if (number == 547)
                    {
                        response.Result = null;
                        response.Success = false;
                        response.Message = "Operation Not Permitted";
                        _logger.LogError("Operation Not Permitted");
                        return StatusCode(490, response);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }


        [HttpPost("Calendar/{idUser}", Name = "Calendar")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public ActionResult<ApiResponse<List<CalendarioDto>>> Paso6([FromBody] List<CalendarioDto> calendarios, int idUser)
        {
            var response = new ApiResponse<List<CalendarioDto>>();
            List<CalendarioDto> lista = new List<CalendarioDto>();
            try
            {

                foreach (var item in calendarios)
                {

                    var calendar = _calendarioRepository.Find(c => c.UserId == item.UserId && c.Id == item.Id);

                    if (calendar == null)
                    {

                        var Calendarios = _mapper.Map<Calendar>(item);
                        lista.Add(_mapper.Map<CalendarioDto>(_calendarioRepository.Add(Calendarios)));
                    }
                    else
                    {
                        var Calendarios = _mapper.Map<Calendar>(item);
                        _calendarioRepository.Update(Calendarios, item.Id);
                        _calendarioRepository.Save();
                    }

                }

                response.Result = lista;
                response.Message = "Success";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error";
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return StatusCode(201, response);
        }

        [HttpDelete("DeleteCalendario", Name = "DeleteCalendario")]
        public ActionResult<ApiResponse<CalendarioDto>> Delete_Calendario(int id)
        {
            var response = new ApiResponse<CalendarioDto>();

            try
            {
                var calendar = _calendarioRepository.Find(c => c.Id == id);

                if (calendar == null)
                {
                    response.Message = $"Calendario id {id} Not Found";
                    return NotFound(response);
                }

                _calendarioRepository.Delete(calendar);
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    var number = sqlException.Number;
                    if (number == 547)
                    {
                        response.Result = null;
                        response.Success = false;
                        response.Message = "Operation Not Permitted";
                        _logger.LogError("Operation Not Permitted");
                        return StatusCode(490, response);
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

        [HttpGet("all-by-speciality", Name = "all-by-speciality")]
        public ActionResult<ApiResponse<List<UserDto>>> GetAllBy([FromQuery] int speciality)
        {
            var response = new ApiResponse<List<UserDto>>();

            try
            {
                var user = _userRepository.GetAll().Where(x=>x.SpecialityId == speciality);

                response.Result = _mapper.Map<List<UserDto>>(user);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

    }
}
