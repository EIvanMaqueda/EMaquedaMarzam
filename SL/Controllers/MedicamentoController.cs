using System.Web.Http;

namespace SL.Controllers
{
    public class MedicamentoController : ApiController
    {
        // GET: Usuario
        [HttpGet]
        [Route("Api/Medicamento/GetAll")]
        public IHttpActionResult GetAll()
        {
            ML.Result result = BL.Medicamento.GetAll();
            return Ok(result);
        }

        [HttpPost]
        [Route("Api/Medicamento/Add")]
        public IHttpActionResult Add([FromBody] ML.Medicamento medicamento)
        {
            ML.Result result = BL.Medicamento.Add(medicamento);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Route("Api/Medicamento/GeyById/{IdMedicamento}")]

        public IHttpActionResult GetById(int IdMedicamento) {

            ML.Result result = BL.Medicamento.GetById(IdMedicamento);
            if (result.Correct)
            {
                return Ok(result);
            }
            else { 
                return NotFound();
               
            }
        }

        [HttpPost]
        [Route("Api/Medicamento/Update")]
        public IHttpActionResult Update([FromBody] ML.Medicamento medicamento) { 
        
            ML.Result result=BL.Medicamento.Update(medicamento);
            if (result.Correct)
            {
                return Ok(result);
            }
            else {
                return NotFound();
            }
        }


        [HttpGet]
        [Route("Api/Medicamento/Delete/{IdMedicamento}")]

        public IHttpActionResult Delete(int IdMedicamento)
        {

            ML.Result result = BL.Medicamento.Delete(IdMedicamento);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();

            }
        }

    }
}