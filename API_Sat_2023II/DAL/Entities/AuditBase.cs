using System.ComponentModel.DataAnnotations;

namespace API_Sat_2023II.DAL.Entities
{
    public class AuditBase
    {
        /*Nota: como todo esto a ser heredable, es necesario usar el 'virtual'*/

        /*DataAnnotation/Decoradores: herramienta para definir propiedades de las tablas, se ponen antes
        de su respectiva propiedad */

        [Key] //DataAnnotation me sirve para definir que esta propiedad ID es una PK
        [Required] //Para campos obligatorios, o sea, que deben tener un valor (no permite nulos)
        public Guid Id { get; set; } //Será la PK de todas las tablas de mi B.D., Guid es para que sea difícil de hackear
        public DateTime? CreatedDate { get; set; } //Campos nuleables, notación Elvis (?)
        public DateTime? ModifiedDate { get; set; } //Campos nuleables, notación Elvis (?)
    }
}
