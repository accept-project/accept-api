using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.PostEdit
{
    public class TaskStatusRequestObject
    {        
            public string TextId { get; set; }
            public string UserId { get; set; }
            public int Status { get; set; }
            public string LastUpdate { get; set; }
           
            public TaskStatusRequestObject()
            {
                
            }

            public TaskStatusRequestObject(string textId, string userId, int status, string lastUpdate)
            {
                this.TextId = textId;
                this.UserId = userId;
                this.Status = status;
                this.LastUpdate = lastUpdate;

            }                       
    }

    public class UserTaskStatus
    {
        public string UserName { get; set; }
        public string Status { get; set; }
    }
}