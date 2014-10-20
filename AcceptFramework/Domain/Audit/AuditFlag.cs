using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AcceptFramework.Domain.Audit
{
    public class AuditFlag: DomainBase
    {
        public virtual string SessionCodeId { get; set; }
        public virtual string Flag { get; set; }
        public virtual string Action { get; set; }
        public virtual string ActionValue { get; set; }        
        public virtual string Ignored { get; set; }
        public virtual string Name { get; set; }
        public virtual string TextBefore { get; set; }
        public virtual string TextAfter { get; set; }
        public virtual DateTime? TimeStamp { get; set; }
        public virtual string RawValue { get; set; }
        public virtual string PrivateId { get; set; }

        public AuditFlag()
        {
            this.SessionCodeId = string.Empty;
            this.Flag = string.Empty;
            this.Action = string.Empty;
            this.ActionValue = string.Empty;          
            this.Ignored = string.Empty;
            this.Name = string.Empty;
            this.TextBefore = string.Empty;
            this.TextAfter = string.Empty;
            this.TimeStamp = null;
            this.RawValue = string.Empty;
            this.PrivateId = string.Empty;
        }

        public AuditFlag(string flag, string action, string actionValue, string sessionCodeId, string ignored, string name, string textBefore, string textAfter, DateTime timeStamp, string rawValue, string privateId)
        {
            this.SessionCodeId = sessionCodeId;
            this.Flag = flag;
            this.Action = action;
            this.ActionValue = actionValue;           
            this.Ignored = ignored;
            this.Name = name;
            this.TextBefore = textBefore;
            this.TextAfter = textAfter;
            this.TimeStamp = timeStamp;
            this.RawValue = rawValue;
            this.PrivateId = privateId;
        }

        public override void Validate()
        {
            try
            {
                if (this.Flag != null && this.Flag.Length == 0)
                    this.Flag = null;

                if (this.Action != null && this.Action.Length == 0)
                    this.Action = null;

                if (this.ActionValue != null && this.ActionValue.Length == 0)
                    this.ActionValue = null;

                if (this.Ignored != null && this.Ignored.Length == 0)
                    this.Ignored = null;

                if (this.Name != null && this.Name.Length == 0)
                    this.Name = null;

                if (this.TextBefore != null && this.TextBefore.Length == 0)
                    this.TextBefore = null;

                if (this.TextAfter != null && this.TextAfter.Length == 0)
                    this.TextAfter = null;

                if (this.RawValue != null && this.RawValue.Length == 0)
                    this.RawValue = null;
            }
            catch (Exception e)
            {
                throw (e);
            }

        }
    }
}


