using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class QuestionReplyMap: ClassMap<QuestionReply>
    {

        public QuestionReplyMap()
        {

            Table("QuestionReply");

            Id(e => e.Id);
            Map(e => e.ReplyText).CustomSqlType("ntext").Length(1073741823);
            References(e => e.Question).Not.LazyLoad().Column("QuestionId");

        }
 
    }
}
