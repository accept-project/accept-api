using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptApi.Areas.Api.Models.Core;

namespace AcceptApi.Areas.Api.Models.Interfaces
{
    public interface ICoreAuditManager
    {
        CoreApiResponse InsertPageVisitAudit(string userName, string type, string action, string description, string origin, string meta, string timeStamp, string language, string userAgent);
    }
}
