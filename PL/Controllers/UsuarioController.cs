using ML;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Nombre,string Password)
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = new ML.Result();
            try
            {
                using (var client = new HttpClient())
                {
                    string urlApi = "http://localhost:36977/";
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Api/Usuario/Login/"+Nombre+"/"+Password);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Usuario resultItemList = new ML.Usuario();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;
                        usuario = (ML.Usuario)result.Object;
                        Session["Nombre"] =  usuario.Nombre + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno;
                        Session["IdUsuario"] = usuario.IdUsuario;
                        ViewBag.Message = "Bienvenido " + Session["Nombre"] + " !";
                        ViewBag.Bandera = true;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        ViewBag.message = "Usuario o contraseña incorrecta";
                        ViewBag.Bandera = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Bandera = false;
                ViewBag.message = "Usuario o contraseña incorrecta";
                result.Correct = false;
                result.Message = ex.Message;
            }
            
           
            return PartialView("Modal");
        }

       
    }
}