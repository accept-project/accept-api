using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class TargetSentenceOptionMap: ClassMap<TargetSentenceOption>
    {
        
        public TargetSentenceOptionMap()
        {
            Table("TargetSentenceOptions");
            Id(e => e.Id);

            Map(e => e.context).CustomSqlType("ntext").Length(1073741823);

            HasManyToMany(e => e.values).Cascade.All().
                Not.LazyLoad().
                AsSet().
                Table("TargetSentenceOptionValue").
                ParentKeyColumn("TargetSentenceOptionID").
                ChildKeyColumn("OptionValueID");
                  
            
        }

    }
}
