using api.main.tecnicah.Models.User;
using AutoMapper;
using biz.main.tecnicah.Entities;
using api.main.tecnicah.Models.Catalogos;
using api.main.tecnicah.Models.Calendario;
using api.main.tecnicah.Models.Diagnosis;
using api.main.tecnicah.Models.Diagnostico;
using biz.main.tecnicah.Models.Diagnosis;
using biz.main.tecnicah.Paged;
using api.main.tecnicah.Models.PagedList;
using api.main.tecnicah.Models.ObjectiveSignUp;
using api.main.tecnicah.Models.RequestSupport;

namespace api.main.tecnicah.Mapper
{
    /// <summary>
    /// Mapper de Models a Entities
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<PagedList<User>, PagedListDto<UserDto>>().ReverseMap();
            CreateMap<PagedListUserDto<User>, PagedListUserDto<UserDto>>().ReverseMap();
            CreateMap<Quiz, QuizDto>().ReverseMap();
            CreateMap<Result, ResultDto>().ReverseMap();

            CreateMap<CatConsultorio, CatConsultorioDto>().ReverseMap();
            CreateMap<CatSpeciality, CatSpecialityDto>().ReverseMap();
            CreateMap<Schedule, CatScheduleDto>().ReverseMap();
            CreateMap<Day, CatDayDto>().ReverseMap();
            CreateMap<Calendar, CalendarioDto>().ReverseMap();
            CreateMap<CatOptionDiagnostic, CatOptionDiagnosticDto>().ReverseMap();
            CreateMap<CatClaveDiagnostic, CatClaveDiagnosticDto>().ReverseMap();
            CreateMap<CatState, CatStateDto>().ReverseMap();
            CreateMap<CatRole, CatRoleDto>().ReverseMap();
            CreateMap<DiagnosisTop, DiagnosisTopDto>().ReverseMap();
            CreateMap<ObjectiveSignUp, ObjectiveSignUpDto>().ReverseMap();
            CreateMap<FidelityLevel, FidelityLevelDto>().ReverseMap();
            CreateMap<RequestSupport, RequestSupportDto>().ReverseMap();

        }
    }
}
