using Arvore.Models;

namespace Arvore.Repository.Interfaces
{
    public interface ICategoriaRepository
    {
        Task CriarTabelaDeCategoriasEPreencherComDados();
        Task<IEnumerable<Categoria>> ObterCategoriasComCategoriasFilhasAsync();
    }
}