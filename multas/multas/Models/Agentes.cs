using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multas.Models
{
    public class Agentes
    {
        //id, nome, esquadra, foto
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }

        //Objeto "lista" das multas (ICollection == Array)
        //Identifica as multas passadas pelos agentes
        public ICollection<Multas> ListaDasMultas { get; set; }
    }
}