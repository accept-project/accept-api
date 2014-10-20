using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class TargetSentenceMap: ClassMap<TargetSentence>
    {

        public TargetSentenceMap()
        {
            Table("TargetSentences");

            Id(e => e.Id);
            Map(e => e.text).CustomSqlType("ntext").Length(1073741823);
                                   
            HasManyToMany(e => e.options).Cascade.All().
                Not.LazyLoad().
                AsSet().
                Table("TargetSentenceOption").
                ParentKeyColumn("TargetSentenceID").
                ChildKeyColumn("OptionID");
                  
        }
    
    }
}
