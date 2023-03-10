using ConsultarCEP.Business;
using ConsultarCEP.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Net;

namespace ConsultarCEP.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CEPController : ControllerBase
    {
        private ICEPBusiness _cepBusiness;

        public CEPController(ICEPBusiness cepBusiness)
        {
            _cepBusiness = cepBusiness;
        }

        [HttpGet("{cep}")]
        [ProducesResponseType((200), Type = typeof(CEP))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Get(string cep) 
        {
            var ValidCep = _cepBusiness.Create(cep);
            if(ValidCep != null) 
            {
                return Ok(ValidCep);
            }
            return BadRequest();
        }

        [HttpGet("logradouro/{logradouro}")]
        [ProducesResponseType((200), Type = typeof(CEP))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult GetLogradouro(string logradouro)
        {
            var ValidLog = _cepBusiness.FindbyLogradouro(logradouro);
            if (ValidLog == null) return BadRequest();
            return Ok(ValidLog);
        }

        [HttpGet()]
        [ProducesResponseType((200), Type = typeof(CEP))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Get(
            [FromQuery] string uf,
            [FromQuery] string sortDirection,
            [FromQuery]int pageSize, 
            [FromQuery]int page)
        {
            return Ok(_cepBusiness.PagedSearch(uf,sortDirection,pageSize,page));
        }



    }
}
