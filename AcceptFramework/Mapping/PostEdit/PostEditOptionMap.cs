using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class PostEditOptionMap: ClassMap<PostEditOption>
    {
        public PostEditOptionMap()
        {
            Table("PostEditOption");
            Id(e => e.Id);
            Map(e => e.EditOption).Length(500);
        }       
    }
}
