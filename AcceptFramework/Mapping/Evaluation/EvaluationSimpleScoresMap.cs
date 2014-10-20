using AcceptFramework.Domain.Evaluation;
using FluentNHibernate.Mapping;

namespace AcceptFramework.Mapping.Evaluation
{
    public class EvaluationSimpleScoresMap : ClassMap<EvaluationSimpleScore>
    {
        public EvaluationSimpleScoresMap()
        {
            Table("EvaluationSimpleScores");

            Id(e => e.Id).Column("ID");
            Map(e => e.ProjectID).Column("ProjectID").Not.Nullable();
            Map(e => e.TimeStamp).Column("TimeStamp").Not.Nullable();
            Map(e => e.Domain).Column("DomainName");

            Map(e => e.QuestionId).Not.Nullable();
            Map(e => e.Question).Not.Nullable();
            Map(e => e.QuestionCategoryId).Not.Nullable();
            Map(e => e.QuestionCategory).Not.Nullable();
            Map(e => e.AnswerId).Not.Nullable();
            Map(e => e.AnswerValue).Not.Nullable();
            Map(e => e.Answer).Not.Nullable();
            Map(e => e.Language).Not.Nullable();

            Map(e => e.Var1).Length(25000);
            Map(e => e.Var2).Length(25000);
            Map(e => e.Var3).Length(2500);
            Map(e => e.Var4).Length(250);
            Map(e => e.Var5).Length(250);
            Map(e => e.Var6).Length(250);
            Map(e => e.Var7).Length(250);
            Map(e => e.Var8).Length(250);
            Map(e => e.Var9).Length(250);
            Map(e => e.Var10).Length(250);
            
        }
    }
}
