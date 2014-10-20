using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.PostEdit
{
    public class ProjectQuestionMap: ClassMap<ProjectQuestion>
    {

        public ProjectQuestionMap()
        {
            Table("ProjectQuestion");
            Id(e => e.Id);
            Map(e => e.Question).Length(500);
        }                 
    }
}
