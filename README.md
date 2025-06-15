# LiteDbCRUDLibrary & Console Example

Este projeto demonstra uma biblioteca C# para CRUD com [LiteDB](https://www.litedb.org/) utilizando **anotações personalizadas** para mapear modelos às coleções. A biblioteca fornece um repositório genérico e um contexto que lê os atributos das classes de modelo para automatizar operações com o banco de dados.

---

## 📁 Estrutura do Projeto

```
├── LiteDbCRUDLibrary
│   ├── Attributes
│   ├── Context
│   ├── Controllers
│   ├── Models
│   ├── Repositories
│   └── LiteDbCRUDLibrary.csproj
│
├── LiteDbConsoleApp
│   ├── Program.cs
│   └── LiteDbConsoleApp.csproj
│
└── LiteDbWebApp
    ├── Controllers
    ├── Models
    ├── LiteDbWebApp.csproj
    ├── LiteDbWebApp.http
    ├── LiteDbWebApp.sln
    └── Program.cs
```

---

## 🌟 LiteDbCRUDLibrary
A Biblioteca que facilita na utilização nos CRUD

---

## 🥐 LiteDbConsoleApp
Só um exemplo de como funciona

---

## 📦 Dependência

* [.NET 9.0](https://dotnet.microsoft.com)
* [LiteDB 5.0.12](https://www.nuget.org/packages/LiteDB/)

---

## ✅ Como Funciona

### 1. Anotação Personalizada

As classes de modelo usam o atributo `[LiteEntity("nome_da_colecao")]` para mapear automaticamente a coleção no LiteDB:

```csharp
[LiteEntity("produtos")]
public class Produto
{
    [BsonId]
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}
```

### 2. Contexto LiteDb

A classe `LiteDbContext` localiza automaticamente o nome da coleção com base na anotação da classe.

### 3. Repositório Genérico

A classe `LiteRepository<T>` oferece métodos CRUD comuns:

```csharp
var repo = new LiteRepository<Produto>("dados.db");
repo.Create(new Produto { Nome = "Caneta", Preco = 1.99m });
```

---

## ▶️ Executar o Projeto Console

1. Compile a solução:

```bash
cd LiteDbConsoleApp
dotnet build
```

2. Execute o exemplo:

```bash
dotnet run
```

---

## ✅ Saída Esperada

```
==== Teste CRUD com LiteDB ====
Produto criado com ID: 1
Lido: Teclado Mecânico - R$299.90
Produto atualizado!

Lista de Produtos:
1: Teclado Mecânico - R$259.90
Produto deletado!
```

## 📄 Licença

Este projeto é livre para uso acadêmico e pessoal. Protegido pela GNU GENERAL PUBLIC LICENSE.

---

## 👨‍💻 Autor

Gabriel Batista & ChatGPT
