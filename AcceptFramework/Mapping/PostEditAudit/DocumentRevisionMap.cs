using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEditAudit;

namespace AcceptFramework.Mapping.PostEditAudit
{
    public class DocumentRevisionMap:ClassMap<DocumentRevision>
    {
        public DocumentRevisionMap()
        {
            Table("DocumentRevision");
            Id(e => e.Id);
            Map(e => e.DocumentId).Length(500);
            Map(e => e.UserIdentifier).Length(500);
            Map(e => e.Status);
            Map(e => e.RevisionHash);
            Map(e => e.DateCreated);
            Map(e => e.DateLastUpdate);
            Map(e => e.CompleteDate);
            
            Map(e => e.IsLocked);
            Map(e => e.LockedBy);
            Map(e => e.LockedDate);   
            
            HasManyToMany(e => e.TranslationUnits).Cascade.All().
                Not.LazyLoad().
                AsSet().
                Table("DocumentRevisionTranslationRevision").
                ParentKeyColumn("DocumentRevisionID").
                ChildKeyColumn("TranslationRevisionID");

            HasManyToMany(e => e.QuestionsReplied).Cascade.All().
              Not.LazyLoad().
              AsSet().
              Table("DocumentRevisionQuestionReplied").
              ParentKeyColumn("DocumentRevisionID").
              ChildKeyColumn("QuestionReplyId");           
        }        
    }
}
