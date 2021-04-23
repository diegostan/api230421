using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        [Key]
        public int ID{get;set;}
        
        [Required(ErrorMessage="Esse campo é obrigatorio")]
        [MaxLength(32, ErrorMessage="O campo pode ter no maximo 32 caracteres")]
        [MinLength(3, ErrorMessage="O valor mínimo desse campo é de 3 caracteres")]
        public string UserName{get;set;}
        
        [Required(ErrorMessage="Esse campo é obrigatorio")]
        [MaxLength(32, ErrorMessage="O campo pode ter no maximo 32 caracteres")]
        [MinLength(3, ErrorMessage="O valor mínimo desse campo é de 3 caracteres")]
        public string Password{get;set;}

        public string Role{get;set;}
        
    }
}