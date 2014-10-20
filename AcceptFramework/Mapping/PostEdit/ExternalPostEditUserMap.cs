using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.PostEdit
{
    public class ExternalPostEditUserMap : ClassMap<ExternalPostEditUser>
    {
        public ExternalPostEditUserMap()
        {
            Table("ExternalPostEditUsers");
           
            Id(e => e.Id);
            Map(e => e.ExternalUserName).Length(320);           
            Map(e => e.CreationDate);
            Map(e => e.UniqueIdentifier).Length(320);           
            Map(e => e.isDeleted);
        }
    
    }
}
