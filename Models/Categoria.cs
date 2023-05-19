namespace Arvore.Models
{
    public class Categoria
    {
        public Categoria()
        {
            CategoriasFilhas = new List<Categoria>();
        }

        public int IdCategoria { get; set; }
        public string NomeCategoria;
        public int? IdCategoriaPai { get; set; }
        public List<Categoria> CategoriasFilhas;

        public Categoria(int idCategoria, string nomeCategoria)
        {
            IdCategoria = idCategoria;
            NomeCategoria = nomeCategoria;
            CategoriasFilhas = new List<Categoria>();
        }
    }
}