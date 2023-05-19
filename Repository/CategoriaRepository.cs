using Arvore.Data;
using Arvore.Models;
using Arvore.Repository.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Arvore.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly CategoriaContext _context;
        
        public CategoriaRepository(CategoriaContext context)
        {
            _context = context;
        }

        public async Task CriarTabelaDeCategoriasEPreencherComDados()
        {
            await _context.Database.GetDbConnection().ExecuteAsync(
                @"
                    CREATE TABLE Categorias (
                        IdCategoria INT PRIMARY KEY,
                        NomeCategoria VARCHAR(50),
                        IdCategoriaPai INT
                    );
                "
            );

            await _context.Database.GetDbConnection().ExecuteAsync(
                @"
                    INSERT INTO Categorias (IdCategoria, NomeCategoria, IdCategoriaPai)
                    VALUES
                    (1, 'Eletrônicos', NULL),
                    (2, 'Televisores', 1),
                    (3, 'Celulares', 1),
                    (4, 'Smartphones', 3),
                    (5, 'Computadores', 1),
                    (6, 'Desktops', 5),
                    (7, 'Notebooks', 5),
                    (8, 'Tablets', 1);
                "
            );
        }

        public async Task<IEnumerable<Categoria>> ObterCategoriasComCategoriasFilhasAsync()
        {
            return await _context.Database.GetDbConnection().QueryAsync<Categoria>(
                @"
                    WITH CTECategoriasComRecursao (IdCategoria, NomeCategoria, IdCategoriaPai)
                        AS (
                            SELECT IdCategoria, NomeCategoria, IdCategoriaPai
                            FROM Categorias
                            WHERE IdCategoriaPai IS NULL
                            UNION ALL
                            SELECT c.IdCategoria, c.NomeCategoria, c.IdCategoriaPai
                            FROM Categorias c
                            INNER JOIN CTECategoriasComRecursao rc ON c.IdCategoriaPai = rc.IdCategoria
                        )
                    SELECT IdCategoria, NomeCategoria, IdCategoriaPai
                    FROM CTECategoriasComRecursao
                    ORDER BY NomeCategoria;
                ")
            ;
        }
    }
}