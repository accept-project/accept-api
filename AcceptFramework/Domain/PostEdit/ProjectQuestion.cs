using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEdit
{
    public class ProjectQuestion: DomainBase
    {

        public virtual string Question { get; set; }

        public ProjectQuestion()
        {
            this.Question = string.Empty;        
        }

        public ProjectQuestion(string question)
        {
            this.Question = question;            
        }

    
    }
}
