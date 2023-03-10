using ConsultarCEP.Model;
using System.Collections.Generic;

namespace ConsultarCEP.Repository
{
    public interface IRepository
    {
        public CEP Create(CEP cep);
        public CEP FindbyLogradouro(string logradouro);
        public List<CEP> FindALLbyUf(string uf);
        public bool Exist(string query);
        public int GetCount(string query);
    }
}
