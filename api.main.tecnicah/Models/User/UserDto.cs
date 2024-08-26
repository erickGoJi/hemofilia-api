using api.main.tecnicah.Models.Calendario;
using biz.main.tecnicah.Entities;
namespace api.main.tecnicah.Models.User
{
   
    public class UserDto
    {

        public int Id { get; set; }
        /// <summary>
        /// Correo electronico
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Contraseña 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// TOKEN
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Nombre 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Celula profesional 
        /// </summary>
        public string ProfessionalLicense { get; set; }
        /// <summary>
        /// Especialidad 
        /// </summary>
        public int? SpecialityId { get; set; }
        /// <summary>
        /// Rol de usuario
        /// </summary>
        public bool ProfessionalLicenseProcedure { get; set; } 
        public int RoleId { get; set; }

        public bool Status { get; set; }

        public string Phone { get; set; }
        public DateTime DateBirth { get; set; }
        public string FirstName { get; set; }
        public int ConsultingType { get; set; }
        public int Sex { get; set; }

        public string Address { get; set; }
        public int Institution { get; set; }
        public int? StateId { get; set; }
        public string? Other { get; set; }
        /// <summary>

        /// <summary>
        /// Quien creo el registro
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación 
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Quien actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha de actualización de registro
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }

    public class UserCalendarDto
    {
        public UserDto User { get; set; }

        public ICollection<CalendarioDto> Calendario { get; set; }
    }
    public class UserReturnDto
    {
        /// <summary>
        /// ID de Usuario
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Correo
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Contraseña
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        public string Name { get; set; }

     
        /// <summary>
        /// Celula profesional 
        /// </summary>
        public string ProfessionalLicense { get; set; }
        /// <summary>
        /// Especialidad 
        /// </summary>
        public int? SpecialityId { get; set; }
        public bool ProfessionalLicenseProcedure { get; set; }
        public int RoleId { get; set; }

        public bool Status { get; set; }

        public string Phone { get; set; }
        public DateTime DateBirth { get; set; }
        public string FirstName { get; set; }
        public string ConsultingType { get; set; }
        public int Sex { get; set; }

        public string Address { get; set; }
        public string Institution { get; set; }
        /// <summary>
        /// Where is locacted?
        /// </summary>
        public int? StateId { get; set; }
        public string Other { get; set; }
        /// <summary>
        /// Quien creo el registro
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación 
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Quien actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha de actualización de registro
        /// </summary>
        public DateTime? UpdatedDate { get; set; }


    }
}
