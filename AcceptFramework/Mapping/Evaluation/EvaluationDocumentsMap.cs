using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationDocumentsMap : ClassMap<EvaluationDocument>
    {
        public EvaluationDocumentsMap()
        {
            Table("EvaluationDocuments");

            Id(e => e.Id);
            Map(e => e.Name).Length(250);
            Map(e => e.IsFull);
            References(e => e.Project).Column("ProjectID");
            References(e => e.LanguagePair).Column("LanguagePairID");
            References(e => e.Provider).Column("ProviderID");            
            HasMany(e => e.UserStatuses).Table("DocumentUserStatuses").KeyColumn("DocumentID");
            HasMany(e => e.Paragraphs).Cascade.All().KeyColumn("DocumentID");
           
        }
    }
}