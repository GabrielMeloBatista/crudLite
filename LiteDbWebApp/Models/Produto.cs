using LiteDB;
using LiteDbCRUDLibrary.Attributes;

namespace LiteDbWebApp.Models
{
    [LiteEntity("produtos")]
    public class Produto
    {
        [BsonId]
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}
