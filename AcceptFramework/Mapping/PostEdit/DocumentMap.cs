using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class DocumentMap : ClassMap<Document>
    {

        public DocumentMap()
        {
            Table("Documents");

            Id(e => e.Id);
            Map(e => e.text_id).Length(500);
            Map(e => e.src_text).CustomSqlType("ntext").Length(1073741823);
            Map(e => e.tgt_text).CustomSqlType("ntext").Length(1073741823);

            HasManyToMany(e => e.tgt_sentences).Cascade.All().
            Not.LazyLoad().
            AsSet().
            Table("DocumentTargetSentence").
            ParentKeyColumn("DocumentID").
            ChildKeyColumn("TargetSentenceID");

            HasManyToMany(e => e.src_sentences).Cascade.All().
            Not.LazyLoad().
            AsSet().
            Table("DocumentSourceSentence").
            ParentKeyColumn("DocumentID").
            ChildKeyColumn("SourceSentenceID");

            HasManyToMany(e => e.tgt_templates).Cascade.All().
            Not.LazyLoad().
            AsSet().
            Table("DocumentTargetTemplate").
            ParentKeyColumn("DocumentID").
            ChildKeyColumn("TargetTemplateID");

            References(e => e.Project).Not.LazyLoad().Column("ProjectId");

            #region Metadata                      
            Map(e => e.Original).Length(500);
            Map(e => e.SourceLanguage).Length(50);
            Map(e => e.TargetLanguage).Length(50);
            Map(e => e.DataType).Length(250);
            Map(e => e.Category).Length(250);
            Map(e => e.ProductName).Length(250);
            Map(e => e.MtContactEmail).Length(500);
            Map(e => e.MtDate).Length(250);
            Map(e => e.MtTool).Length(2500);
            Map(e => e.MtToolId).Length(2500);

            Map(e => e.IsSingleRevision);
            Map(e => e.UniqueReviewerId).Length(500);
                           
            HasManyToMany(e => e.DocumentRevisions).Cascade.AllDeleteOrphan().
               LazyLoad().
               AsSet().
               Table("DocumentsRevisions").
               ParentKeyColumn("DocumentID").
               ChildKeyColumn("DocumentRevisionID");
            
            #endregion

        }
    
    }
}
