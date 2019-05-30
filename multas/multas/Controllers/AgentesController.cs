using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using multas.Models;

namespace multas.Controllers
{
    public class AgentesController : Controller
    {
        //cria uma variavel que representa a base de dados
        private ApplicationDbContext db = new ApplicationDbContext();
        private object fotografia;

        // GET: Agentes
        public ActionResult Index()
        {
            //Procura a totalidade dos agente na base de dados
            //Instrução feita em LINQ
            //SELECT * FROM Agentes ORDER BY nome
            var listaAgentes = db.Agentes.OrderBy(a=>a.Nome).ToList();

            return View(listaAgentes);
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// Mostra os dados de um Agente
        /// </summary>
        /// <param name="id">Identifica o Agente</param>
        /// <returns>View com os dados</returns>
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                //vamos alterar esta resposta por defeito
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer experiencias ( no url ex)
                return RedirectToAction("Index");
            }
            //SELECT * FROM Agentes WHERE Id=id
            Agentes agente = db.Agentes.Find(id);

            //O agente foi encontrado?
            if (agente == null)
            {
                //O agente nao foi encontrado, porque o utilizador está 'à pesca de erros'
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        /// <summary>
        /// criação de um novo agente
        /// </summary>
        /// <param name="agente">recolhe os dados do nome e da esquadra do agente</param>
        /// <returns>representa a fotogradia que identifica o agente</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agente, HttpPostedFileBase fotografia)
        {
            //precisamos de processar a fotografia
            //1º será que foi fornecido um ficheiro?
            //2º será do tipo correto?
            //3º se for do tipo correto, guarda-se
            //   ser não for, atribui-se um 'avatar genérico' ao utilizador

            string caminho = "";
            bool haficheiro = false;
            //há ficheiro?
            if (fotografia == null)
            {
                //não há ficheiro,
                //atribui-se-lhe o avatar
                agente.Fotografia = "noUser.png";
            }
            else
            {
                //há ficheiro
                //será correto?
                if(fotografia.ContentType =="image/jpeg" || fotografia.ContentType == "image/png")
                {
                    //estamos perante uma foto correta
                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();
                    Guid g;
                    g = Guid.NewGuid();
                    // nome do ficheiro 
                    string nome = g.ToString() + extensao;
                    // onde guardar o ficheiro
                    caminho = Path.Combine(Server.MapPath("~/imagens"), nome);
                    //atribuir ao agente o nome do ficheiro
                    agente.Fotografia = nome;
                }
            }
                
            if (ModelState.IsValid) //valida se os dados fornecidos com as regras definidas no Modelo
            {
                try
                {
                    db.Agentes.Add(agente);   //adiciona o novo Agente ao Modelo
                    db.SaveChanges();         //consolida os dados na bd
                    if(haficheiro)
                    //vou guardar o ficheiro no disco rigido
                    fotografia.SaveAs(caminho);
                    return RedirectToAction("Index"); //redireciona o utilizador para a pagina index
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a escrita dos dados do novo agente");
                }

            }

            //enviar para a View is dados do agente que foi procurado e encontrado
            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                //vamos alterar esta resposta por defeito
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer experiencias ( no url ex)
                return RedirectToAction("Index");
            }
            //SELECT * FROM Agentes WHERE Id=id
            Agentes agente = db.Agentes.Find(id);

            //O agente foi encontrado?
            if (agente == null)
            {
                //O agente nao foi encontrado, porque o utilizador está 'à pesca de erros'
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //vamos alterar esta resposta por defeito
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                //este erro ocorre porque o utilizador anda a fazer experiencias ( no url ex)
                return RedirectToAction("Index");
            }
            //SELECT * FROM Agentes WHERE Id=id
            Agentes agente = db.Agentes.Find(id);

            //O agente foi encontrado?
            if (agente == null)
            {
                //O agente nao foi encontrado, porque o utilizador está 'à pesca de erros'
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            //o agente foi encontrado
            //vou salvaguardar os dados para posterior validação
            //-guardar o ID do agente num cookie cifrado
            //-guardar o ID numa variável de sessão (se se usar o ASP.NET Core, esta ferramenta já não existe...)

            Session["Agente"] = agente.ID;

            //mostra na view os dados do agente
            return View(agente);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
                {
                    //tentar dar volta ao codigo
                    return RedirectToAction("Index");
                }

            //o ID não é null
            //será o ID o que eu espero?
            //vamos validar se o ID está correto
            if (id != (int)Session["Agente"])
            {
                //há aqui outro xico esperto
                return RedirectToAction("Index");
            }


            //procura o agente a remover
            Agentes agente = db.Agentes.Find(id);

                if(agente == null)
            {
                //ver se o agente é null ou com valor
                return RedirectToAction("Index");


            }
            try
            {

                //da ordem à remoção do agente
                db.Agentes.Remove(agente);
                //consolida a remoção
                db.SaveChanges();

            }
            catch (Exception) { 
            //devem ser escritas todas as instruções que sao consideradas necessarias
            //informar que houve um erro
            ModelState.AddModelError("", "Não é possível remover o Agente porque provavelmente" + agente.Nome + "." +
                "tem multas associadas a ele...");

            //Redirecionar para a pafina onde o erro foi gerado

            return View(agente);
            }

            return RedirectToAction("Index");

            }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
