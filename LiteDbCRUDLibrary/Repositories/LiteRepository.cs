// Repositories/LiteRepository.cs
using LiteDB; // Necessário para ILiteQueryable, ILiteCollection e IQueryable
using LiteDbCRUDLibrary.Context;
using System;
using System.Collections.Generic;
using System.Linq; // Necessário para ToList(), Skip(), Take(), OrderBy(), OrderByDescending()
using System.Linq.Expressions;

namespace LiteDbCRUDLibrary.Repositories
{
    /// <summary>
    /// Define a ordem de classificação para os resultados paginados.
    /// </summary>
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    /// <summary>
    /// Representa o resultado de uma consulta paginada, contendo os itens da página atual
    /// e metadados de paginação.
    /// </summary>
    /// <typeparam name="T">O tipo dos itens na coleção.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// A coleção de itens para a página atual.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// A contagem total de itens que correspondem aos critérios de filtragem,
        /// independentemente da paginação.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// O número da página atual (baseado em 1).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// O número máximo de itens por página.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// O número total de páginas disponíveis.
        /// </summary>
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    /// <summary>
    /// Um repositório genérico para operações CRUD com LiteDB,
    /// agora com suporte a paginação, filtragem e classificação.
    /// </summary>
    /// <typeparam name="T">O tipo da entidade a ser gerenciada pelo repositório.</typeparam>
    public class LiteRepository<T> where T : class
    {
        private readonly string _dbPath;

        /// <summary>
        /// Inicializa uma nova instância do repositório LiteRepository.
        /// </summary>
        /// <param name="dbPath">O caminho completo para o arquivo do banco de dados LiteDB.</param>
        public LiteRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        /// <summary>
        /// Cria e insere uma nova entidade no banco de dados.
        /// </summary>
        /// <param name="entity">A entidade a ser inserida.</param>
        /// <returns>O ID da entidade inserida.</returns>
        public int Create(T entity)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Insert(entity);
        }

        /// <summary>
        /// Lê (recupera) uma entidade pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da entidade a ser recuperada.</param>
        /// <returns>A entidade encontrada ou null se não for encontrada.</returns>
        public T? Read(int id)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().FindById(id);
        }

        /// <summary>
        /// Lê (recupera) todas as entidades da coleção.
        /// </summary>
        /// <returns>Uma coleção de todas as entidades.</returns>
        public IEnumerable<T> ReadAll()
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().FindAll().ToList();
        }

        /// <summary>
        /// Atualiza uma entidade existente no banco de dados.
        /// </summary>
        /// <param name="entity">A entidade com as informações atualizadas.</param>
        /// <returns>True se a entidade foi atualizada com sucesso, caso contrário, false.</returns>
        public bool Update(T entity)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Update(entity);
        }

        /// <summary>
        /// Exclui uma entidade do banco de dados pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da entidade a ser excluída.</param>
        /// <returns>True se a entidade foi excluída com sucesso, caso contrário, false.</returns>
        public bool Delete(int id)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Delete(id);
        }

        /// <summary>
        /// Executa uma consulta básica com base em um predicado (filtro).
        /// </summary>
        /// <param name="predicate">Uma expressão para filtrar os resultados.</param>
        /// <returns>Uma coleção de entidades que correspondem ao predicado.</returns>
        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            using var context = new LiteDbContext(_dbPath);
            return context.Set<T>().Find(predicate);
        }

        /// <summary>
        /// Recupera uma lista paginada de entidades com opções de filtragem e classificação.
        /// </summary>
        /// <param name="pageNumber">O número da página a ser recuperada (baseado em 1). Padrão é 1.</param>
        /// <param name="pageSize">O número de itens por página. Padrão é 10.</param>
        /// <param name="predicate">Opcional: Uma expressão para filtrar os resultados.</param>
        /// <param name="orderBy">Opcional: Uma expressão para especificar a propriedade para classificação.</param>
        /// <param name="sortOrder">A ordem de classificação (Ascending ou Descending). Padrão é Ascending.</param>
        /// <returns>Um objeto PagedResult<T> contendo os itens da página solicitada e a contagem total.</returns>
        public PagedResult<T> GetPaged(
            int pageNumber = 1,
            int pageSize = 10,
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>? orderBy = null,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            // Garante que o número da página e o tamanho da página são válidos
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10; // Garante que o pageSize seja no mínimo 1

            using var context = new LiteDbContext(_dbPath);
            var collection = context.Set<T>();

            // Inicia a consulta usando Query() para obter um ILiteQueryable
            ILiteQueryable<T> query = collection.Query();

            // 1. Aplica a filtragem se um predicado for fornecido
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // Obtém a contagem total de itens ANTES de aplicar a paginação
            // Isso é crucial para calcular o TotalPages corretamente.
            var totalCount = query.Count();

            // 2. Aplica a classificação se uma expressão orderBy for fornecida
            if (orderBy != null)
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }

            // 3. Aplica a paginação
            var skip = (pageNumber - 1) * pageSize;

            // Executa a consulta, aplica paginação e converte os resultados para uma lista
            var items = query.Skip(skip).Limit(pageSize).ToList();

            // Retorna o objeto PagedResult
            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
