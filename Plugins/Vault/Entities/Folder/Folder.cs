using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using VaultServices.Entities.Base;

namespace VaultServices.Entities.Folder
{
    [DataServiceKey("Id")]
    [DataServiceEntity]
    public class Folder : BaseObject
    {
        public override long Id { get; set; }
        public override sealed string Type { get; set; }
        public override string Name { get; set; }
        public override string CreateUser { get; set; }
        public override DateTime CreateDate { get; set; }
        public override IEnumerable<Property.Property> Properties { get; set; }
        public override IEnumerable<Link.Link> Children { get; set; }
        public override IEnumerable<Link.Link> Parents { get; set; }
    }
}