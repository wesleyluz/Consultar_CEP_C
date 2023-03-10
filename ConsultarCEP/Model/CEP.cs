using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultarCEP.Model
{
    [Table("CEP")]
    public class CEP
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("cep")]
        public string Cep { get; set; }

        [Column("logradouro")]
        public string Logradouro { get; set; }

        [Column("complemento")] 
        public string Complemento { get; set; }

        [Column("bairro")] 
        public string Bairro { get; set; }

        [Column("localidade")]
        public string Localidade { get; set; }

        [Column("uf")] 
        public string Uf { get; set; }

        [Column("unidade")]
        public long Unidade { get; set; }

        [Column("ibge")]
        public int Ibge { get; set; }

        [Column("gia")]
        public string Gia { get; set; }
    }
}
