using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Domain.PostEditAudit
{
    public class QuestionReply: DomainBase
    {
        public virtual ProjectQuestion Question { get; set; }   
        public virtual string ReplyText { get; set; }


        public QuestionReply()
        {
            this.Question = new ProjectQuestion();         
            this.ReplyText = string.Empty;
        
        }

        public QuestionReply(ProjectQuestion question, string replyText)
        {
            this.Question = question;          
            this.ReplyText = replyText;
        }
    
    }
}
    