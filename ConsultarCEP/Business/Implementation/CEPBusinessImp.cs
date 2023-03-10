using ConsultarCEP.Model;
using ConsultarCEP.Repository;
using ConsultarCEP.Utills;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace ConsultarCEP.Business.Implementation
{
    public class CEPBusinessImp : ICEPBusiness
    {
        private readonly IRepository _repository;

        public CEPBusinessImp(IRepository repository) 
        {
            _repository = repository;
        }
        public CEP Create(string cepString)
        {
            string result = string.Empty;
            cepString = Regex.Replace(cepString, "[^0-9a-zA-Z]+", "");

            var valid = ValidateCEP(cepString);
            if (valid)
            {
                string viaCEPUrl = "https://viacep.com.br/ws/" + cepString + "/json/";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient();
                result = client.DownloadString(viaCEPUrl);

            }
            else
            {
                return null;
            }
            JObject cepJson = JsonConvert.DeserializeObject<JObject>(result);
            if (cepJson["erro"] != null) return null;

            var cep = new CEP
            {
                Cep = Regex.Replace(cepJson["cep"].ToString(), "[^0-9a-zA-Z]+", ""),
                Logradouro = RemoverAcentos(cepJson["logradouro"].ToString()),
                Complemento = RemoverAcentos(cepJson["complemento"].ToString()),
                Bairro = RemoverAcentos(cepJson["bairro"].ToString()),
                Localidade = RemoverAcentos(cepJson["localidade"].ToString()),
                Uf = RemoverAcentos(cepJson["uf"].ToString()),
                //Unidade foi removida do json logo não será atualizado
                //Unidade = RemoverAcentos(cepJson["unidade"].ToString()),
                Ibge = int.Parse(cepJson["ibge"].ToString()),
                Gia = RemoverAcentos(cepJson["gia"].ToString())

            };
            if (!Exist(cep))
            {
                _repository.Create(cep);
            }
            else 
            {
                return _repository.FindbyLogradouro(cep.Logradouro);
            }
            return cep;
        }
        public string RemoverAcentos(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            else
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(input);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }
        public bool Exist(CEP cep)
        {
            string query = @"select * from [dbo].[CEP] c ";
            query += $"where c.cep = {cep.Cep} and c.logradouro like '%{cep.Logradouro}%'";
            return _repository.Exist(query);
        }

        public List<CEP> FindALLbyUf(string query)
        {
            return _repository.FindALLbyUf(query);
        }

        public CEP FindbyLogradouro(string logradouro)
        {
            return _repository.FindbyLogradouro(logradouro);
        }
        public bool ValidateCEP(string cep)
        {
            Regex Rgx = new Regex(@"^\d{8}");
            return Rgx.IsMatch(cep);
        }

        public PagedSearch PagedSearch(string uf, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc")) ? "asc":"desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = (page > 0) ? (page * size) : 0;
            string query = $"SELECT * from [dbo].[CEP] c where c.uf = '{uf}' ORDER BY c.id {sort} OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY";
            string countQuery = $"select count(*) from [dbo].[CEP] c where c.uf = '{uf}'"; 
           
            var CEPs = FindALLbyUf(query);
            int totalResults = _repository.GetCount(countQuery);
            var result = Decimal.Divide(totalResults, size);
            int totalPages = (int)Math.Ceiling(result);

            return new PagedSearch
            {
                CurrentPage = page,
                List = CEPs,
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults,
                TotalPages = totalPages
            };
        }
    }
}
