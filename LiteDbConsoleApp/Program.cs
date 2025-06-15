using LiteDbCRUDLibrary.Repositories;
using Models;
using System;

Console.WriteLine("==== Teste CRUD com LiteDB ====");

var repo = new LiteRepository<Produto>("dados.db");

// CREATE
var novoProduto = new Produto { Nome = "Teclado Mecânico", Preco = 299.90m };
var id = repo.Create(novoProduto);
Console.WriteLine($"Produto criado com ID: {id}");

// READ
var produto = repo.Read(id);
if (produto != null)
    Console.WriteLine($"Lido: {produto.Nome} - R${produto.Preco}");

// UPDATE
if (produto != null)
{
    produto.Preco = 259.90m;
    repo.Update(produto);
    Console.WriteLine("Produto atualizado!");
}

// READ ALL
var todos = repo.ReadAll();
Console.WriteLine("\nLista de Produtos:");
foreach (var p in todos)
    Console.WriteLine($"{p.Id}: {p.Nome} - R${p.Preco}");

// DELETE
repo.Delete(id);
Console.WriteLine("Produto deletado!");
