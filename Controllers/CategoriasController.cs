using System.Text.Json;
using System.Text.Json.Serialization;
using Arvore.Models;
using Arvore.Repository;
using Arvore.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Arvore.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _repository;

    public CategoriasController(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("obter-categorias-com-hierarquia")]
    public async Task<string> ObterCategoriasComHierarquiaAsync()
    {
        await _repository.CriarTabelaDeCategoriasEPreencherComDados();

        return ConverterDadosDoBDParaArvore(await _repository.ObterCategoriasComCategoriasFilhasAsync());
    }

    private string ConverterDadosDoBDParaArvore(IEnumerable<Categoria> categorias)
    {
        Dictionary<int, Categoria> categoriaHash = new Dictionary<int, Categoria>();

        foreach (Categoria categoria in categorias)
        {
            categoriaHash[categoria.IdCategoria] = categoria;
        }

        Categoria categoriaPaiDeTodas = null;

        foreach (Categoria categoria in categorias)
        {
            if (categoria.IdCategoriaPai == null || categoria.IdCategoriaPai == 0)
            {
                categoriaPaiDeTodas = categoria;
            }
            else
            {
                Categoria categoriaPai = categoriaHash[categoria.IdCategoriaPai.Value];

                categoriaPai.CategoriasFilhas.Add(categoria);
            }
        }

        Func<Categoria, object> FuncConverteParaObjetoAnonimo = null;

        FuncConverteParaObjetoAnonimo = o =>
        {
            dynamic novoObjetoAnonimo = new
            {
                IdCategoria = o.IdCategoria,
                NomeCategoria = o.NomeCategoria,
                CategoriasFilhas = o.CategoriasFilhas.Select(e => FuncConverteParaObjetoAnonimo(e))
            };
            
            return novoObjetoAnonimo;
        };

        return JsonConvert.SerializeObject(FuncConverteParaObjetoAnonimo(categoriaPaiDeTodas), Formatting.Indented);
    }
}
