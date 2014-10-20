using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class ProjectInvitationMap: ClassMap<ProjectInvitation>
    {
        public ProjectInvitationMap()
        {
            Table("ProjectInvitation");
            Id(e => e.Id);
            Map(e => e.ProjectId);
            Map(e => e.UserName);
            Map(e => e.InvitationDate);
            Map(e => e.ConfirmationCode);
            Map(e => e.ConfirmationDate);

            Map(e => e.Type);        
        }
        
    
    }
}
