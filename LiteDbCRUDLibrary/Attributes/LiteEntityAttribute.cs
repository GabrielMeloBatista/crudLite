// Attributes/LiteEntityAttribute.cs
using System;

namespace LiteDbCRUDLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LiteEntityAttribute : Attribute
    {
        public string CollectionName { get; }

        public LiteEntityAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
