using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Mapping.Common
{
    public class EngineMap: ClassMap<Engine>
    {
        public EngineMap()
        {
            Table("Engine");

            Id(e => e.Id)
             .Column("ID");

            Map(e => e.EngineName)
                .Column("EngineName").Length(250);                

            Map(e => e.Status)
                  .Column("Status");

            Map(e => e.Enable)
                      .Column("Enable");
            
            Map(e => e.Address)
                 .Column("Address").Length(250);      
        
        }
    
    }
}
