using System;
using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationProjectMap : ClassMap<EvaluationProject>
    {
        public EvaluationProjectMap()
        {
            Table("EvaluationProjects");

            Id(e => e.Id);
            Map(e => e.Name).Length(250);
            Map(e => e.Organization).Length(50);
            Map(e => e.Description).Length(250);
            Map(e => e.ApiKey).Length(250);
            Map(e => e.Domain).Length(250);
            Map(e => e.Type);
            Map(e => e.CreationDate);
            Map(e => e.Status).Column("Status").CustomType(typeof(Int32));
            Map(e => e.Mode).CustomType(typeof(Int32));

            Map(e => e.AdminToken).Column("AdminToken").Length(250);
            
            References(e => e.Creator).Not.LazyLoad().Column("CreatorID");

            HasManyToMany(e => e.Providers).
                Table("EvaluationProjectProviders").Not.LazyLoad().
                ParentKeyColumn("ProjectID").
                ChildKeyColumn("ProviderID");

            HasManyToMany(e => e.LanguagePairs).
                Table("EvaluationProjectLanguagePairs").Not.LazyLoad().
                ParentKeyColumn("ProjectID").
                ChildKeyColumn("LanguagePairID");

            HasManyToMany(e => e.Questions).
                Table("EvaluationProjectQuestions").Not.LazyLoad().
                ParentKeyColumn("ProjectID").
                ChildKeyColumn("QuestionID");

            HasManyToMany(e => e.ContentChunks).Cascade.DeleteOrphan().
               Table("EvaluationProjectChunk").Not.LazyLoad().
               ParentKeyColumn("ProjectID").
               ChildKeyColumn("ChunkID");

        }
    

        


    }
}
