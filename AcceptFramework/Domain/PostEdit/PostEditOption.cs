using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.PostEdit
{
    public class PostEditOption: DomainBase
    {
         public virtual string EditOption { get; set; }

         public PostEditOption()
        {
            this.EditOption = string.Empty;        
        }

         public PostEditOption(string option)
        {
            this.EditOption = option;            
        }
        
    
    }
}
