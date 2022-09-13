using System;
using System.Collections.Generic;

namespace VaultServices.Entities.Base
{
    public interface IBaseObject
    {
        long Id { get; set; }
        string Type { get; set; }
        string Name { get; set; }
        string CreateUser { get; set; }
        DateTime CreateDate { get; set; }
        IEnumerable<Property.Property> Properties { get; set; }
        IEnumerable<Link.Link> Children { get; set; }
        IEnumerable<Link.Link> Parents { get; set; }
    }

    public abstract class BaseObject : IBaseObject
    {
        public abstract long Id { get; set; }
        public abstract string Type { get; set; }
        public abstract string Name { get; set; }
        public abstract string CreateUser { get; set; }
        public abstract DateTime CreateDate { get; set; }
        public abstract IEnumerable<Property.Property> Properties { get; set; }
        public abstract IEnumerable<Link.Link> Children { get; set; }
        public abstract IEnumerable<Link.Link> Parents { get; set; }

        protected BaseObject()
        {
            Children = new List<Link.Link>();
            Parents = new List<Link.Link>();
            Properties = new List<Property.Property>();
            Type = GetType().Name;
        }
    }
}
