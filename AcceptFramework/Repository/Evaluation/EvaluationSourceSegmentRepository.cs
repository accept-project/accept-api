using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Common;
using AcceptFramework.Domain.Evaluation;
using AcceptFramework.Repository;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationSourceSegmentRepository : RepositoryBase<EvaluationSourceSegment>
    {
        public static List<EvaluationSourceSegment> SelectByParagraphID(int paragraphID, string userName)
        {
            List<EvaluationSourceSegment> list = null;
            return list;

        }

        private EvaluationSourceSegmentScoring SelectUserScoringBySourceSegment(EvaluationSourceSegment segment, User user)
        {
          
            return new EvaluationSourceSegmentScoring();
        }

        private int FidelityScore(EvaluationSourceSegment segment, User user)
        {
            EvaluationSourceSegmentScoring score = SelectUserScoringBySourceSegment(segment, user);
            if (score != null)
                if (DateTime.Now.Subtract(score.ScoringDate).TotalHours < 24) return score.FidelityScore;
            return -1;
        }

        private int ComprehensibilityScore(EvaluationSourceSegment segment, User user)
        {
            EvaluationSourceSegmentScoring score = SelectUserScoringBySourceSegment(segment, user);
            if (score != null)
                if(DateTime.Now.Subtract(score.ScoringDate).TotalHours < 24) return score.ComprehensibilityScore;
            return 0;
        }

        private string SelectTargetStringBySourceSegmentID(EvaluationSourceSegment source)
        {
            //TargetSegment targetSegment = source.TargetSegments.FirstOrDefault();
            //if (targetSegment != null)
            //    return targetSegment.TargetString;
            return "";
        }
    }
}
