using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;


namespace AcceptFramework.Mapping.Evaluation
{
    #region EvaluationInternal
    public class EvaluationUserProjectRoleMap : ClassMap<EvaluationUserProjectRole>
    {
        public EvaluationUserProjectRoleMap()
        {
            Table("EvaluationUserProjectRole");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.User).Column("UserID").Not.LazyLoad();
            References(x => x.Role).Column("RoleID").Not.LazyLoad();
            References(x => x.Project).Column("ProjectID").Not.LazyLoad();
        }
    }
    #endregion
}
