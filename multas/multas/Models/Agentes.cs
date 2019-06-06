using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace multas.Models
{
    public class Agentes
    {
        public Agentes()
        {
            ListaDasMultas = new HashSet<Multas>();
        }
        //id, nome, esquadra, foto
        public int ID { get; set; }

        [Required(ErrorMessage ="Por favor, escreva o nome do agente")]
        [RegularExpression("[A-ZÁÉÍÓÚa-záéíóúàèìòùäëïöüãõâêîôûçñ]+( |-|')?)+", ErrorMessage = "Só pode escrever letras no nome. Deve começar por uma maiúscula")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, escreva o nome da esquadra")]
        //[RegularExpression("Tomar|Ourém|Torres Novas|Lisboa|Leiria|Alcanena|Abrantes|Porto")]

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }
       
        //Objeto "lista" das multas (ICollection == Array)
        //Identifica as multas passadas pelos agentes
        //virtual - carrrega as multas quando criar um agente
        public virtual ICollection<Multas> ListaDasMultas { get; set; }

        //**************************************************************\\
        //adicionar uma 'chave forasteria' para uma tabela de autenticação
        //é nencessário para todos as aplicações 'a sério'
        //[Required]
        public string UserNameId { get; set; }
    }
}