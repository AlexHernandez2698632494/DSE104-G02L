using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Encodings.Web;
namespace MVCPelicula.Controllers
{
    public class HelloWorldController : Controller
    {
        // GET:/HelloWorld/
        //public string Index()
        //{
        //    return "Esta es mi acción <b> predeterminada </b> ...";
        //}
        public ActionResult Index()
        {
            return View();
        }
        // GET: /HelloWorld/Welcome/
        //public string Welcome()
        //{
        //    return "Este es el método de acción Bienvenida...";
        //}
        //public string Welcome(string nombre, int numVeces = 1)
        //{
        //    //return "Este es el método de acción Bienvenida...";
        //    return HtmlEncoder.Default.Encode($"Hola {nombre}, NumVeces: {numVeces}");
        //}
        //public string Welcome(string nombre, int ID = 1)
        //{
        //    return HtmlEncoder.Default.Encode($"Hola {nombre}, ID: {ID}");
        //}
        //public ActionResult Welcome(string nombre, int numVeces = 1)
        //{
        //    ViewData["nombre"] = "Hola " + nombre;
        //    ViewData["numVeces"] = numVeces;
        //    return View();
        //}
        public ActionResult Welcome(string nombre, string apellido, int numveces = 1)
        {
            ViewData["nombreCompleto"] = "Hola " + nombre + " " + apellido;
            ViewData["numVeces"] = numveces;

            return View();
        }
    }
}