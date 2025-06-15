using LiteDB;
using LiteDbCRUDLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteDbCRUDLibrary.Context
{
    public class LiteDbContext : IDisposable
    {
        private readonly LiteDatabase _database;

        public LiteDbContext(string dbPath)
        {
            _database = new LiteDatabase(dbPath);
        }

        public ILiteCollection<T> Set<T>() where T : class
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute<LiteEntityAttribute>();

            if (attr == null)
                throw new InvalidOperationException($"Classe {type.Name} não possui o atributo [LiteEntity]");

            return _database.GetCollection<T>(attr.CollectionName);
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
