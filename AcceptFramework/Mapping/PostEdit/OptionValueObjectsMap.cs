using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class OptionValueObjectsMap : ClassMap<OptionValueObject>
    {
        public OptionValueObjectsMap()
        {

            Table("OptionValueObject");

            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            Map(x => x.phrase).Column("phrase").Length(1073741823);
            Map(x => x.start).Column("start");
            Map(x => x.end).Column("[end]");
            Map(x => x.fscore).Column("fscore");
                         
        }
    
    }
}
