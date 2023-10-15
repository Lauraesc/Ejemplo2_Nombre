using System.ComponentModel.DataAnnotations;

namespace API_Sat_2023II.DAL.Entities
{
    public class Country : AuditBase //cada que se cree una nueva entidad, se le hará la herencia con AuditBase
    {
        [Display(Name = "País")] //Para pintar el nombre bonito en el front-end, en vez mostrar "name", mostrará "país"
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")] //Longitud de carácteres máxima
        [Required(ErrorMessage = "¡El campo {0} es obligatorio!")] //0 = "País", 1 = '50'
        public string Name { get; set; } //varchar(50)
    }
}
