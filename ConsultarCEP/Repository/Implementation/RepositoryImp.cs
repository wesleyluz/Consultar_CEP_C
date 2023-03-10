using ConsultarCEP.Model;
using ConsultarCEP.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ConsultarCEP.Repository.Implementation
{
    public class RepositoryImp : IRepository
    {
        protected MSSQLContext _context;
        public RepositoryImp(MSSQLContext context) 
        {
            _context = context;
        }
        public CEP Create(CEP cep)
        {
            try
            {
                _context.CEPs.Add(cep);
                _context.SaveChanges();
               

            }
            catch (Exception)
            {

                throw;
            }
            return cep;
        }

        public bool Exist(string query)
        {
            var InDataBase = _context.CEPs.FromSqlRaw(query).ToList();
            if (InDataBase.Count > 0)
            {
                return true;
            }
            return false;
        }

        public List<CEP> FindALLbyUf(string query)
        {
            return _context.CEPs.FromSqlRaw(query).ToList();
        }

        public CEP FindbyLogradouro(string logradouro)
        {
            //return _context.CEPs.SingleOrDefault(C => C.Logradouro.Equals(logradouro));
            return _context.CEPs.SingleOrDefault(c => c.Logradouro.Equals(logradouro));
        }

        public int GetCount(string query)
        {
            var result = "";
            using (var connection = _context.Database.GetDbConnection()) 
            {
                connection.Open();
                using(var command = connection.CreateCommand()) 
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }
            return int.Parse(result);
        }
    }
}
