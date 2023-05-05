using ML;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class MedicamentoController : Controller
    {
        // GET: Medicamento
        public ActionResult Index()
        {
            ML.Result result= new ML.Result();
            ML.Medicamento medicamento=new ML.Medicamento();
            try
            {
                using (var client = new HttpClient())
                {
                    string urlApi = "http://localhost:36977/";
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Api/Medicamento/GetAll");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();

                        readTask.Wait();
                        result.Objects = new List<object>();
                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Medicamento resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(resultItem.ToString());
                            
                            result.Objects.Add(resultItemList);
                        }
                    }
                     medicamento.Medicamentos= result.Objects;
                }
            }
            catch (Exception ex)
            {

                
            }

            return View(medicamento);
        }

        [HttpGet]

        public ActionResult Form(int? IdMedicamento) {
            ML.Result result = new ML.Result();
            ML.Medicamento medicamento = new ML.Medicamento();
            if (IdMedicamento == null)
            {
                medicamento = new ML.Medicamento();
                return View(medicamento);
            }
            else {
                try
                {
                    using (var client = new HttpClient())
                    {
                        string urlApi = "http://localhost:36977/";
                        client.BaseAddress = new Uri(urlApi);

                        var responseTask = client.GetAsync("Api/Medicamento/GeyById/"+IdMedicamento);
                        responseTask.Wait();

                        var resultServicio = responseTask.Result;

                        if (resultServicio.IsSuccessStatusCode)
                        {
                            var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();
                            ML.Medicamento resultItemList = new ML.Medicamento();
                            resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(readTask.Result.Object.ToString());
                            result.Object = resultItemList;
                            medicamento = (ML.Medicamento)result.Object;
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                        }

                    }
                }
                catch (Exception ex)
                {

                    result.Correct = false;
                    result.Message = ex.Message;
                }
                return View(medicamento);
            }
            
        }

        [HttpPost]

        public ActionResult Form(ML.Medicamento medicamento) {
            HttpPostedFileBase file = Request.Files["fuImage"];
            if (file != null)
            {
                byte[] imagen = ConvertToBytes(file);
                medicamento.Imagen = Convert.ToBase64String(imagen);
            }
            if (medicamento.IdMedicamento == 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:36977/");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Medicamento>("Api/Medicamento/Add", medicamento);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Medicamento Agregado correctamente";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "Error al agregar el medicamento";
                        return PartialView("Modal");
                    }
                }

            }
            else {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:36977/");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Medicamento>("Api/Medicamento/Update", medicamento);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Medicamento Actualizado correctamente";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "Error al Actualizar el medicamento";
                        return PartialView("Modal");
                    }
                }
            }
            
        }

        [HttpGet]

        public ActionResult Delete(int IdMedicamento) {
            ML.Result result=new ML.Result();
            try
            {
                using (var client = new HttpClient())
                {
                    string urlApi = "http://localhost:36977/";
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Api/Medicamento/Delete/" + IdMedicamento);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                        ViewBag.Message = "Medicamento Borrado Exitosamente";
                    }
                    else
                    {
                        result.Correct = false;
                        ViewBag.Message = "Error al eliminar el mensaje";
                    }

                }
            }
            catch (Exception ex)
            {

                result.Correct = false;
                result.Message = ex.Message;
            }
            return PartialView("Modal");
        }
        public byte[] ConvertToBytes(HttpPostedFileBase foto)
        {

            byte[] data = null;
            System.IO.BinaryReader reader = new System.IO.BinaryReader(foto.InputStream);
            data = reader.ReadBytes((int)foto.ContentLength);
            return data;

        }

        [HttpGet]

        public ActionResult Productos() {

            ML.Result result = BL.Medicamento.GetAll();
            ML.Medicamento medicamento= new ML.Medicamento();
            medicamento.Medicamentos = result.Objects;
            return View(medicamento);
        }

        [HttpGet]

        public ActionResult Carrito(int? IdMedicamento) {
            ML.Usuario usuario = new ML.Usuario();
            if (IdMedicamento != null)
            {
                ML.Result result = BL.Medicamento.GetById(IdMedicamento.Value);
                ML.Medicamento medicamento = (ML.Medicamento)result.Object;
                
                if (Session["Usuario"] != null)
                {
                    usuario = (ML.Usuario)Session["Usuario"];
                    if (usuario.Medicamentos != null)
                    {
                        usuario = BL.Medicamento.Carrito(usuario, medicamento);
                    }
                }
                else
                {
                    usuario.Nombre = Session["Nombre"].ToString();
                    usuario.IdUsuario = int.Parse(Session["IdUsuario"].ToString());
                    usuario.Medicamentos = new List<object>();
                    medicamento.Cantidad = 1;
                    usuario.Medicamentos.Add(medicamento);
                    Session["Usuario"] = usuario;

                }
            }
            else {
                if (Session["Usuario"]!=null)
                {
                    usuario = (ML.Usuario)Session["Usuario"];
                } 
                
                var nombre1= Session["Nombre"].ToString();
                usuario.Nombre= Session["Nombre"].ToString();
            }
            return View(usuario);
        }


    }
}