using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace multas.Models
{
    public class Condutores
    {
        public Condutores()
        {
            ListasDasMultas = new HashSet<Multas>();
        }
        public int ID { get; set; }

        public string Nome { get; set; }

        public string BI { get; set; }

        public string Telemovel { get; set; }

        public DateTime DataNascimento { get; set; }

        public string NumCartaConducao { get; set; }

        public string LocalEmissao { get; set; }

        public DateTime DataValidadeCarta { get; set; }

        //Lista das multas associadas ao condutor
        public virtual ICollection<Multas> ListasDasMultas { get; set; }
    }
}