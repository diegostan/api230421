using System.ComponentModel.DataAnnotations;


namespace Shop.Models
{
    public class Product
    {
        [Key]
        [DataType("int")]
        public int ID{get;set;}

        [Required(ErrorMessage="Esse campo é obrigatorio")]
        [MaxLength(60, ErrorMessage="O valor maximo e de 60 caracteres")]
        [MinLength(3, ErrorMessage="O valor minimo e de 3 caracteres")]
        [DataType("varchar")]
        public string Title{get;set;}

        [MaxLength(1024, ErrorMessage="A quantidade maxima de caracteres é 1024")]
        public string Description{get;set;}

        [Range(1, int.MaxValue, ErrorMessage="O preço compreende de 1 até 32767")]
        public decimal Price{get;set;}

        [Required(ErrorMessage="Esse campo é obrigatorio.")]
        [Range(1, int.MaxValue, ErrorMessage= "Categoria inválida")]        
        public int CategoryId{get;set;}

        public Category Category{get;set;}
    }
    
}