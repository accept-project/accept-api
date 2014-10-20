using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class TargetTemplateMap : ClassMap<TargetTemplate>
    {
        public TargetTemplateMap()
        {
            Table("TargetTemplates");

            Id(e => e.Id);
            Map(e => e.markup).CustomSqlType("ntext").Length(1073741823);
               
        }
    
    
    }
}
