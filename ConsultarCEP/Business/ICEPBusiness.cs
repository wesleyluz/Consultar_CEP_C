using ConsultarCEP.Model;
using ConsultarCEP.Utills;
using System.Collections.Generic;

namespace ConsultarCEP.Business
{
    public interface ICEPBusiness
    {
        public bool ValidateCEP(string cep);
        public CEP Create(string cepString);
        public CEP FindbyLogradouro(string logradouro);
        public PagedSearch PagedSearch(string uf, string sortDirection, int pageSize, int page);
        public List<CEP> FindALLbyUf(string uf);
        public bool Exist(CEP cep);

    }
}
