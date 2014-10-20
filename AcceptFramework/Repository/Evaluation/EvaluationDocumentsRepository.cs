using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AcceptFramework.Domain.Evaluation;

namespace AcceptFramework.Repository.Evaluation
{
    [DataObject]
    public class EvaluationDocumentsRepository : RepositoryBase<EvaluationDocument>
    {
    
        /// <summary>
        /// Select docuemtns by project, language pair and provider
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<EvaluationDocument> SelectByProject(int projectID)
        {
            return Select(d => d.Project.Id == projectID);
        }

        /// <summary>
        /// Select docuemtns by project, language pair and provider
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<EvaluationDocument> Select(int projectID, int langPairID, int providerID)
        {
            return Select(d => 
                d.LanguagePair.Id == langPairID && 
                d.Provider.Id == providerID && 
                d.Project.Id == projectID);
        }

        /// <summary>
        /// Select docuemtns by user
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<EvaluationDocument> Select(int userID)
        {
            EvaluationDocumentUserStatusesRepository repository = new EvaluationDocumentUserStatusesRepository();
            List<EvaluationDocument> list = repository.Select(dus => dus.User.Id == userID).ToList().Select(d => d.Document).Distinct().ToList();
            List<EvaluationDocument> resultList = new List<EvaluationDocument>();
            foreach (EvaluationDocument document in list)
            {
                resultList.Add(new EvaluationDocument
                                   {
                                       Id = document.Id,
                                       Project = document.Project,
                                       Provider = document.Provider,
                                       LanguagePair = document.LanguagePair,
                                       CompletedDate = CheckCompletedDate(document, userID),
                                       CompletionTime = CalculateCompletionTime(document, userID)
                                   });
            }

            return resultList;
        }

        /// <summary>
        /// Select docuemtns by user
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<EvaluationDocumentStatistic> SelectStatictic(int userID)
        {
            var directions = new[] {Direction.Both, Direction.Source, Direction.Target};
            var repository = new EvaluationDocumentUserStatusesRepository();
            var list = repository.Select(dus => dus.User.Id == userID).ToList().Select(d => d.Document).Distinct().ToList();
            List<EvaluationDocumentStatistic> resultList = new List<EvaluationDocumentStatistic>();

            foreach (EvaluationDocument document in list)
            {
                if (document == null)
                    continue;
                double totalScore = 0;
                double paraFidelity = AvgFidelityScoreParagraph(document, userID);
                double segmentFidelity = AvgFidelityScoreSegment(document, userID);

                foreach (Direction direction in directions)
                {
                    double avgParaScore = AvgComprehensibilityScoreParagraph(document, direction, userID);
                    double avgSegmentScore = AvgComprehensibilityScoreSegment(document, direction, userID);
                    totalScore += avgParaScore;
                    if (avgParaScore == 0)
                    {
                        continue;
                    }

                    EvaluationDocumentStatistic stat = new EvaluationDocumentStatistic
                    {
                        DocumentID = document.Id,
                        ProjectName = document.Project.Name,
                        ProviderName = document.Provider.Name,
                        LanguagePair = document.LanguagePair,
                        Direction = direction,
                        CompletedDate = CheckCompletedDate(document, userID),
                        CompletionTime = CalculateCompletionTime(document, userID),
                        AvgParaCompScore = avgParaScore,
                        AvgSegmentCompScore = avgSegmentScore,
                        ParaFidelity = paraFidelity,
                        SegmentFidelity = segmentFidelity
                    };
                    GetStatisticPerDocument(stat, document);
                    resultList.Add(stat);
                }

                if (totalScore == 0)
                {
                    EvaluationDocumentStatistic stat = new EvaluationDocumentStatistic
                    {
                        DocumentID = document.Id,
                        ProjectName = document.Project.Name,
                        ProviderName = document.Provider.Name,
                        LanguagePair = document.LanguagePair,
                        CompletedDate = CheckCompletedDate(document, userID),
                        CompletionTime = CalculateCompletionTime(document, userID),
                        AvgParaCompScore = 0,
                        AvgSegmentCompScore = 0,
                        ParaFidelity = paraFidelity,
                        SegmentFidelity = segmentFidelity
                    };
                    GetStatisticPerDocument(stat, document);
                    resultList.Add(stat);
                }
            }

            return resultList;
        }

        private DateTime CheckCompletedDate(EvaluationDocument document, int userID)
        {
            EvaluationDocumentUserStatusesRepository repository = new EvaluationDocumentUserStatusesRepository();
            if (repository.Select(dus => dus.Document == document && dus.User.Id == userID && dus.Status == EvaluationStatus.InProgress).Count == 0)
                return repository.Select(dus => dus.Document == document && dus.User.Id == userID).Last().CompletionDate;
            return DateTime.MinValue;
        }

        private double CalculateCompletionTime(EvaluationDocument document, int userID)
        {
            EvaluationDocumentUserStatusesRepository repository = new EvaluationDocumentUserStatusesRepository();
            return repository.Select(dus => dus.Document == document && dus.User.Id == userID).Sum(d => d.CompletionTime);
        }

        private void GetStatisticPerDocument(EvaluationDocumentStatistic stat, EvaluationDocument doc)
        {
            double totalComp = 0;
            double totalCompCount = 0;
            double totalFidelity = 0;
            double totalFidelityCount = 0;

            totalComp += doc.Paragraphs.Sum(
                p => p.Scorings.Where(s => s.ComprehensibilityScore > 0).Sum(sc => sc.ComprehensibilityScore));
            
            totalCompCount += doc.Paragraphs.
                Sum(p => p.Scorings.
                    Where(sc =>
                        sc.ComprehensibilityScore > 0).
                    Count());

            totalFidelity += doc.Paragraphs.Sum(
                p => p.Scorings.Where(s => s.FidelityScore > -1).Sum(sc => sc.FidelityScore));
            
            totalFidelityCount += doc.Paragraphs.
                Sum(p => p.Scorings.
                    Where(sc =>
                        sc.LanguagePair != null && sc.FidelityScore > -1).
                    Count());

            stat.DocumentComprehensibility = totalCompCount > 0 ? totalComp / totalCompCount : 0;
            stat.DocumentFidelity = totalFidelityCount > 0 ? totalFidelity / totalFidelityCount * 100 : 0;
        }

        #region Paragraph Scorings

        private double AvgFidelityScoreParagraph(EvaluationDocument doc, int userID)
        {
            double sum = 0;
            double count = 0;

            if (doc.Paragraphs != null)
            {
                foreach (EvaluationSourceParagraph paragraph in doc.Paragraphs)
                {
                    count += paragraph.Scorings.Where(s => s.User != null && s.UserID == userID && s.LanguagePair != null).Count();
                    sum += paragraph.Scorings.Where(s => s.User != null && s.UserID == userID && s.LanguagePair != null).Sum(s => s.FidelityScore);
                }
            }

            if (count == 0)
            {
                return 0;
            }

            return sum / count * 100;
        }

        private double AvgComprehensibilityScoreParagraph(EvaluationDocument doc, Direction direction, int userID)
        {
            double sum = 0;
            double count = 0;

            if (doc.Paragraphs != null)
            {
                foreach (EvaluationSourceParagraph paragraph in doc.Paragraphs)
                {
                    if (direction == Direction.Both)
                    {
                        count +=
                            paragraph.Scorings.Where(
                                s => s.User != null && s.User.Id == userID && s.LanguagePair != null).ToList().Count;
                        sum +=
                            paragraph.Scorings.Where(
                                s => s.User != null && s.User.Id == userID && s.LanguagePair != null).Sum(
                                    s => s.ComprehensibilityScore);
                    }
                    else if (direction == Direction.Source)
                    {
                        var list2 = paragraph.Scorings.Where(
                            s => s.User != null && s.User.Id == userID && s.SourceLanguage != null).ToList();

                        count +=
                            paragraph.Scorings.Where(
                                s => s.User != null && s.User.Id == userID && s.SourceLanguage != null).ToList().Count;
                        sum +=
                            paragraph.Scorings.Where(
                                s => s.User != null && s.User.Id == userID && s.SourceLanguage != null).Sum(
                                    s => s.ComprehensibilityScore);
                    }
                    else if (direction == Direction.Target)
                    {
                        count +=
                            paragraph.Scorings.Where(
                                s => s.User != null && s.User.Id == userID && s.TargetLanguage != null).ToList().Count;
                        sum +=
                            paragraph.Scorings.Where(
                                s => s.User != null && s.User.Id == userID && s.TargetLanguage != null).Sum(
                                    s => s.ComprehensibilityScore);
                    }
                }
            }

            if (count == 0)
                return 0;
            return sum/count;
        }

        #endregion

        #region Segment Scorings

        private double AvgComprehensibilityScoreSegment(EvaluationDocument doc, Direction direction, int userID)
        {
            double sum = 0;
            int count = 0;

            foreach (EvaluationSourceParagraph paragraph in doc.Paragraphs)
            {
                if (direction == Direction.Both)
                {
                    var scores = paragraph.SourceSegments.SelectMany(seg => seg.BilingualScorings.Where(s => s.User != null && s.UserID == userID && s.ComprehensibilityScore > 0)).ToList();
                    count += scores.Count();
                    sum += scores.Sum(s => s.ComprehensibilityScore);
                }
                else if (direction == Direction.Source)
                {
                    var scores = paragraph.SourceSegments.SelectMany(seg => seg.SourceScorings.Where(s => s.User != null && s.UserID == userID && s.ComprehensibilityScore > 0)).ToList();
                    count += scores.Count();
                    sum += scores.Sum(s => s.ComprehensibilityScore);
                }
                else if (direction == Direction.Target)
                {
                    var scores = paragraph.TargetSegments.SelectMany(seg => seg.TargetScorings.Where(s => s.User != null && s.UserID == userID && s.ComprehensibilityScore > 0)).ToList();
                    count += scores.Count();
                    sum += scores.Sum(s => s.ComprehensibilityScore);
                }
            }

            if (count == 0)
                return 0;
            return sum/count;
        }

        private double AvgFidelityScoreSegment(EvaluationDocument doc, int userID)
        {
            double sum = 0;
            int count = 0;

            foreach (EvaluationSourceParagraph paragraph in doc.Paragraphs)
            {
                var scores = paragraph.SourceSegments.SelectMany(seg => seg.BilingualScorings.Where(s => s.User != null && s.UserID == userID && s.FidelityScore > -1)).ToList();
                count += scores.Count();
                sum += scores.Sum(s => s.FidelityScore);
            }
            
            if (count == 0)
                return 0;
            return sum/count*100;
        }

        #endregion
    }
}