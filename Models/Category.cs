using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category
    {
        [Key]
        [DataType("int")]
        public int ID{get;set;}

        [Required(ErrorMessage="Esse campo é obrigatorio")]
        [MaxLength(60, ErrorMessage="O valor maximo e de 60 caracteres")]
        [MinLength(3, ErrorMessage="O valor minimo e de 3 caracteres")]
        [DataType("varchar")]
        public string Title{get;set;}
    }
}