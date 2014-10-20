using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationSourceParagraphsMap : ClassMap<EvaluationSourceParagraph>
    {
        public EvaluationSourceParagraphsMap()
        {
            Table("EvaluationSourceParagraph");

            Id(e => e.Id);
            Map(e => e.ExternalPostID);           
            Map(e => e.Original);
            References(e => e.Document).Not.LazyLoad().Column("DocumentID");
            References(e => e.AuthorType).Not.LazyLoad().Column("AuthorTypeID");
            References(e => e.Category).Not.LazyLoad().Column("CategoryID");
            HasMany(x => x.TargetSegments).Cascade.All().KeyColumn("ParagraphID");
            HasMany(x => x.SourceSegments).Cascade.All().KeyColumn("ParagraphID");      
            Map(e => e.SourceHash);
            HasManyToMany(x => x.Scorings)
                .Table("ParagraphScoring")
                .ParentKeyColumn("ParagraphID")
                .ChildKeyColumn("ScoringID")
                .Cascade.SaveUpdate();
        }
    }
}
