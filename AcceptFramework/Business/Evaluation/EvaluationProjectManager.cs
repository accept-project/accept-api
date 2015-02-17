using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AcceptFramework.Business.Utils;
using AcceptFramework.Domain.Evaluation;
using AcceptFramework.Interfaces.Evaluation;
using AcceptFramework.Repository.Common;
using AcceptFramework.Repository.Evaluation;
using AcceptPortal.Models.Evaluation;
using System.Web;
using EP.Business;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Business.Evaluation
{
    internal class EvaluationProjectManager : IEvaluationProjectManager
    {
        private string space;
        private EvaluationDocument resultDoc;
        private EvaluationSourceParagraph paragraph;
        private EvaluationTargetSegment targetSegment;
        private EvaluationSourceSegment sourceSegment;
        private EvaluationReferenceSegment referenceSegment;

        public bool GenerateLanguages()
        {

            if (EvaluationLanguagesRepository.GetAll().ToList().Count == 0)
            {
                EvaluationLanguage english = new EvaluationLanguage("English", "en_us");
                EvaluationLanguage french = new EvaluationLanguage("French", "fr_fr");
                EvaluationLanguage german = new EvaluationLanguage("German", "de_de");
                EvaluationLanguage chinese = new EvaluationLanguage("Chinese", "zh_cn");
                EvaluationLanguage spanish = new EvaluationLanguage("Spanish", "es_es");
                EvaluationLanguage other = new EvaluationLanguage("Other", string.Empty);
                EvaluationLanguagesRepository.Insert(english);
                EvaluationLanguagesRepository.Insert(french);
                EvaluationLanguagesRepository.Insert(german);
                EvaluationLanguagesRepository.Insert(chinese);
                EvaluationLanguagesRepository.Insert(spanish);
                EvaluationLanguagesRepository.Insert(other);

                return true;
            }

            return false;
        }

        public EvaluationSimpleScoreResponse SimpleScore(int Id, string key, string answerId, string domain, string var1, string var2, string var3, string var4, string var5, string var6, string var7, string var8, string var9, string var10)
        {
            int AnswerId = 0;
            var score = new EvaluationSimpleScore();
            var project = generateEvaluationProjectToken(EvaluationProjectRepository.GetProject(Id));

            if (project == null)
            {
                throw new ArgumentException("Unknown Project", "ProjectId");
            }

            if (project.ApiKey != key)
            {
                throw new ArgumentException("Unknown API Key", "key");
            }

            if (!IsValidDomain(project.Domain, domain))
            {
                throw new ArgumentException("Invalid Domain (" + domain + ")", "domain");
            }

            try
            {
                AnswerId = Convert.ToInt32(answerId);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid Answer ID", "AnswerId"); ;
            }

            //look for answer ID in the list of questions.
            foreach (var q in project.Questions)
            {
                foreach (var qitem in q.LanguageQuestions)
                {
                    foreach (var answer in qitem.Answers)
                    {
                        if (answer.Id == AnswerId)
                        {
                            score.QuestionCategory = q.Name;
                            score.QuestionCategoryId = q.Id;
                            score.QuestionId = qitem.Id;
                            score.Question = qitem.Question;
                            score.Language = qitem.Language.Code;
                            score.AnswerId = answer.Id;
                            score.Answer = answer.Name;
                            score.AnswerValue = answer.Value;
                        }
                    }
                }
            }

            if (score.QuestionCategory == "")
            {
                throw new ArgumentException("Invalid Answer ID", "AnswerId"); ;
            }

            score.ProjectID = project.Id;
            score.Domain = domain;
            score.TimeStamp = DateTime.UtcNow;

            if (var1 != null) { score.Var1 = HttpUtility.UrlDecode(var1); }
            if (var2 != null) { score.Var2 = HttpUtility.UrlDecode(var2, System.Text.Encoding.UTF8); }
            if (var3 != null) { score.Var3 = HttpUtility.UrlDecode(var3); }
            if (var4 != null) { score.Var4 = HttpUtility.UrlDecode(var4); }
            if (var5 != null) { score.Var5 = HttpUtility.UrlDecode(var5); }
            if (var6 != null) { score.Var6 = HttpUtility.UrlDecode(var6); }
            if (var7 != null) { score.Var7 = HttpUtility.UrlDecode(var7); }
            if (var8 != null) { score.Var8 = HttpUtility.UrlDecode(var8); }
            if (var9 != null) { score.Var9 = HttpUtility.UrlDecode(var9); }
            if (var10 != null) { score.Var10 = HttpUtility.UrlDecode(var10); }

            score = EvaluationSimpleScoresRepository.Insert(score);

            //update scores.
            foreach (var q in project.Questions)
            {
                q.Count = 0;
                foreach (var qitem in q.LanguageQuestions)
                {
                    qitem.Count = 0;
                    foreach (var answer in qitem.Answers)
                    {
                        answer.Count = EvaluationSimpleScoresRepository.GetCount(answer.Id);
                        EvaluationQuestionItemAnswersRepository.Update(answer);
                        qitem.Count += answer.Count;
                    }

                    EvaluationQuestionItemsRepository.Update(qitem);
                    q.Count += qitem.Count;
                }
                EvaluationQuestionsRepository.Update(q);
            }

            var ret = new EvaluationSimpleScoreResponse();
            if (score != null)
            {
                ret.Success = true;
                ret.ProjectID = score.ProjectID;
                ret.ScoreID = score.Id;
            }
            else
            {
                ret.ScoreID = 0;
                ret.ProjectID = 0;
                ret.Success = false;
            }

            return ret;
        }

        public EvaluationQuestion CreateQuestion(int projectId, string name)
        {
            var project = generateEvaluationProjectToken(EvaluationProjectRepository.GetProject(projectId));
            var question = new EvaluationQuestion(name);
            question = EvaluationQuestionsRepository.Insert(question);
            project.Questions.Add(question);
            project = EvaluationProjectRepository.UpdateProject(project);
            return question;
        }

        public EvaluationQuestion UpdateQuestionCategory(int Id, string name)
        {
            var category = EvaluationQuestionsRepository.Get(Id);
            category.Name = name;
            category = EvaluationQuestionsRepository.Update(category);
            return category;
        }

        public EvaluationQuestionItem CreateQuestionItem(int projectId, int qid, int lid, string name, string action, string confirmation)
        {
            var question = EvaluationQuestionsRepository.Get(qid);
            var language = EvaluationLanguagesRepository.Get(lid);
            var questionitem = new EvaluationQuestionItem();
            questionitem.Question = name;
            questionitem.Action = action;
            questionitem.Language = language;
            questionitem.Confirmation = confirmation;
            questionitem = EvaluationQuestionItemsRepository.Insert(questionitem);
            question.LanguageQuestions.Add(questionitem);
            question = EvaluationQuestionsRepository.Update(question);
            return questionitem;
        }

        public EvaluationQuestionItem UpdateQuestionItem(int id, int lid, string name, string action, string confirmation)
        {
            var language = EvaluationLanguagesRepository.Get(lid);
            var questionItem = EvaluationQuestionItemsRepository.Get(id);
            questionItem.Question = name;
            questionItem.Action = action;
            questionItem.Confirmation = confirmation;
            questionItem.Language = language;
            questionItem = EvaluationQuestionItemsRepository.Update(questionItem);
            return questionItem;
        }

        public EvaluationQuestionItemAnswer CreateQuestionItemAnswer(int projectId, int qid, string answer, string value)
        {
            var qitem = EvaluationQuestionItemsRepository.Get(qid);
            var itemAnswer = new EvaluationQuestionItemAnswer { Name = answer, Value = value };
            itemAnswer = EvaluationQuestionItemAnswersRepository.Insert(itemAnswer);
            qitem.Answers.Add(itemAnswer);
            qitem = EvaluationQuestionItemsRepository.Update(qitem);
            return itemAnswer;
        }

        public EvaluationQuestionItemAnswer UpdateQuestionAnswer(int Id, string answer, string value)
        {
            var itemAnswer = EvaluationQuestionItemAnswersRepository.Get(Id);
            itemAnswer.Name = answer;
            itemAnswer.Value = value;
            itemAnswer = EvaluationQuestionItemAnswersRepository.Update(itemAnswer);
            return itemAnswer;
        }

        #region InternalEvaluation
        public EvaluationProject CreateProject(string name, string description, string org, string domain, string requestor, int type, int postEditProjectId, int evaluationMethod, int duplicationLogic, bool includeOwnerRevision, string emailBody)
        {
            var project = new EvaluationProject();
            var user = UserRepository.GetUserByUserName(requestor);
            if (user == null)
            {
                throw new ArgumentException("Unknown User", "requestor");
            }
            project.Name = name;
            project.Creator = user;
            project.Organization = org;
            project.Description = description;
            project.Domain = domain;
            project.ApiKey = StringUtils.Generate32CharactersStringifiedGuid();
            project.CreationDate = DateTime.UtcNow;
            project = EvaluationProjectRepository.Insert(project);


            project.Type = type == 1 ? project.Type = EvaluationType.Embedded : project.Type = EvaluationType.External;
            project.PostEditProjectReferenceId = project.Type == EvaluationType.Embedded ? project.PostEditProjectReferenceId = postEditProjectId : project.PostEditProjectReferenceId = -1;
            project.IncludePostEditProjectOwner = includeOwnerRevision;
            switch (duplicationLogic)
            {
                case 1: { project.PostEditProjectDuplicationLogic = EvaluationProject.InternalProjectAvoidDuplicates.YesTaskLevel; } break;
                case 2: { project.PostEditProjectDuplicationLogic = EvaluationProject.InternalProjectAvoidDuplicates.YesProjectLevel; } break;
                default:
                    {
                        project.PostEditProjectDuplicationLogic = EvaluationProject.InternalProjectAvoidDuplicates.No;
                    } break;
            }
            switch (evaluationMethod)
            {
                case 1: { project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.EvaluateOriginalOnly; } break;
                case 2: { project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.EvaluateOriginalAndAllRevisions; } break;
                case 3: { project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.EvaluateOnlyRevisions; } break;
                default:
                    {
                        project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.NotSet;
                    } break;
            }

            project.EmailBodyMessage = emailBody;


            return project;
        }

        public EvaluationProject UpdateProject(int projectId, string name, string description, string org, string apiKey, string domain, int type, int postEditProjectId, int evaluationMethod, int duplicationLogic, bool includeOwnerRevision, string emailBody)
        {
            var project = EvaluationProjectRepository.GetProject(projectId);

            if (project == null)
            {
                throw new ArgumentException("Unknown Project", "ProjectId");
            }

            project.Name = name;
            project.Description = description;
            project.Organization = org;
            project.ApiKey = apiKey;
            project.Domain = domain;
            project = generateEvaluationProjectToken(project);
            project.PostEditProjectReferenceId = project.Type == EvaluationType.Embedded ? project.PostEditProjectReferenceId = postEditProjectId : project.PostEditProjectReferenceId = -1;
            project.IncludePostEditProjectOwner = includeOwnerRevision;
            switch (duplicationLogic)
            {
                case 1: { project.PostEditProjectDuplicationLogic = EvaluationProject.InternalProjectAvoidDuplicates.YesTaskLevel; } break;
                case 2: { project.PostEditProjectDuplicationLogic = EvaluationProject.InternalProjectAvoidDuplicates.YesProjectLevel; } break;

                default:
                    {
                        project.PostEditProjectDuplicationLogic = EvaluationProject.InternalProjectAvoidDuplicates.No;
                    } break;
            }

            switch (evaluationMethod)
            {
                case 1: { project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.EvaluateOriginalOnly; } break;
                case 2: { project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.EvaluateOriginalAndAllRevisions; } break;
                case 3: { project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.EvaluateOnlyRevisions; } break;
                default:
                    {
                        project.PostEditProjectEvaluationMethod = EvaluationProject.InternalProjectEvaluationMethod.NotSet;
                    } break;
            }

            project.EmailBodyMessage = emailBody;

            project = EvaluationProjectRepository.UpdateProject(project);
            return project;
        }
        #endregion


        public List<EvaluationDocumentSimple> Documents(int id)
        {
            var x = new string[0];
            var docs = new EvaluationDocumentsRepository().SelectByProject(id);
            var list = new List<EvaluationDocumentSimple>();

            foreach (var doc in docs)
            {
                var item = new EvaluationDocumentSimple();
                item.Name = doc.Name;
                item.Provider = doc.Provider.Name;
                item.ProviderId = doc.Provider.Id;
                item.LanguagePair = doc.LanguagePair.FullName;
                item.LanguagePairId = doc.LanguagePair.Id;
                list.Add(item);
            }
            return list;
        }

        public bool UploadFile(int projectId, int provider, int langpair, string file)
        {
            resultDoc = new EvaluationDocument();
            resultDoc.Name = "tempname123";
            resultDoc.Provider = new EvaluationProvidersRepository().Select(p => p.Id == provider).FirstOrDefault();
            resultDoc.LanguagePair = new EvaluationLanguagePairsRepository().Select(lp => lp.Id == langpair).FirstOrDefault();
            resultDoc.Project = new EvaluationProjectRepository().Select(p => p.Id == projectId).FirstOrDefault(); ;
            string decoded_file = System.Web.HttpUtility.HtmlDecode(file);
            decoded_file = decoded_file.Replace("&", "&amp;");
            XDocument document = XDocument.Parse(decoded_file);
            IEnumerable<XElement> xfiles = document.Root.Elements().ToList();
            space = "{" + document.Root.GetDefaultNamespace().NamespaceName + "}";

            foreach (var xfile in xfiles)
            {
                XAttribute categoryAttr = xfile.Attribute("category");
                if (categoryAttr == null)
                {
                    return false;
                    //throw new DocumentValidationException(DocumentErrorType.CategoryNotDefined);
                }

                XAttribute authorTypeAttr = xfile.Attribute("author_type_id");
                if (authorTypeAttr == null)
                {
                    return false;
                    //throw new DocumentValidationException(DocumentErrorType.AuthorTypeNotDefined);
                }

                XAttribute originalTypeAttr = xfile.Attribute("original");
                if (originalTypeAttr == null)
                {
                    return false;
                    //throw new DocumentValidationException(DocumentErrorType.OriginalNotDefined);
                }

                paragraph = new EvaluationSourceParagraph();
                string categoryName = categoryAttr.Value;
                string authorType = authorTypeAttr.Value;
                string originalName = originalTypeAttr.Value;
                paragraph.Category = new EvaluationCategoryRepository().SelectOrCreate(categoryName);
                paragraph.AuthorType = new EvaluationAuthorTypeRepository().SelectOrCreate(authorType);
                paragraph.Original = originalName;
                resultDoc.Paragraphs.Add(paragraph);
                int count = new EvaluationSourceParagraphRepository().
                    Select(p =>
                           p.Document.Project == resultDoc.Project &&
                           p.Document.LanguagePair == resultDoc.LanguagePair &&
                           p.Document.Provider != resultDoc.Provider &&
                           p.Original == paragraph.Original).
                    Count;

                resultDoc.IsFull = false;
                if (count == 0)
                    resultDoc.IsFull = true;

                if (xfile.HasElements)
                {
                    XElement xbody = xfile.Element(string.Format("{0}body", space));
                    if (xbody == null)
                    {
                        return false;
                        //throw new DocumentValidationException(DocumentErrorType.BodyNotDefined);
                    }

                    IEnumerable<XElement> paragraphSegments = xbody.Elements();
                    if (paragraphSegments.Count() == 0)
                    {
                        return false;
                        //throw new DocumentValidationException(DocumentErrorType.SegmentsEmpty);
                    }

                    ImportSegments(paragraphSegments);

                    //calculate source hash.
                    StringBuilder sb = new StringBuilder();
                    foreach (var seg in paragraph.SourceSegments.Select(s => s.SourceString))
                    {
                        sb.Append(seg);
                    }
                    paragraph.SourceHash = sb.ToString().GetHashCode().ToString();
                }
            }

            EvaluationDocumentsRepository documentsRepository = new EvaluationDocumentsRepository();
            documentsRepository.Create(resultDoc);
            return true;
        }

        private void ImportSegments(IEnumerable<XElement> paragraphSegments)
        {
            foreach (XElement paragraphSegment in paragraphSegments)
            {
                XElement target = paragraphSegment.Element(string.Format("{0}target", space));
                if (target == null)
                {
                    //throw new DocumentValidationException(DocumentErrorType.TargetNotDefined);
                }

                XElement source = paragraphSegment.Element(string.Format("{0}source", space));
                if (source == null)
                {
                    //throw new DocumentValidationException(DocumentErrorType.SourceNotDefined);
                }

                ValidateLanguagePairProvider(target, source);

                string sourceString = "";
                if (source != null) sourceString = source.Value;

                sourceSegment = new EvaluationSourceSegment { SourceString = sourceString };

                if (target != null)
                {
                    string targetString = target.Value;
                    targetSegment = new EvaluationTargetSegment { TargetString = targetString, SourceParagraph = paragraph, SourceSegment = sourceSegment };
                }

                if (paragraphSegment.Element(string.Format("{0}alt-trans", space)) != null)
                {
                    ImportContributors(paragraphSegment);
                }
                if (paragraphSegment.Element(string.Format("{0}count-group", space)) != null)
                {
                    ImportSegmentsElement(paragraphSegment);
                }
                paragraph.SourceSegments.Add(sourceSegment);
                paragraph.TargetSegments.Add(targetSegment);
            }
        }

        private void ValidateLanguagePairProvider(XElement target, XElement source)
        {
            string targetCode = target.Attribute(XNamespace.Xml + "lang").Value;
            targetCode = targetCode.ToLower().Replace('_', '-');

            string sourceCode = source.Attribute(XNamespace.Xml + "lang").Value;
            sourceCode = sourceCode.ToLower().Replace('_', '-');

            if (targetCode != resultDoc.LanguagePair.TargetLanguage.Code.ToLower().Replace('_', '-') ||
                sourceCode != resultDoc.LanguagePair.SourceLanguage.Code.ToLower().Replace('_', '-'))
            {
                EvaluationLanguagePair language =
                    new EvaluationLanguagePairsRepository().Select(
                        lp =>
                            lp.SourceLanguage.Code.ToLower().Replace('_', '-') == sourceCode &&
                            lp.TargetLanguage.Code.ToLower().Replace('_', '-') == targetCode).
                        FirstOrDefault();
            }

            XAttribute provider = target.Attribute("provider");
            if (provider != null)
                if (provider.Value != resultDoc.Provider.Name)
                {
                }
        }

        private void ImportContributors(XElement paragraphSegment)
        {
            string referenceString = paragraphSegment.Element(string.Format("{0}alt-trans", space)).Element(string.Format("{0}target", space)).Value;
            string contributorName = paragraphSegment.Element(string.Format("{0}alt-trans", space)).Attribute("contributor_name").Value;
            EvaluationContributor contributor = new EvaluationContributorRepository().SelectOrCreate(contributorName);
            referenceSegment = new EvaluationReferenceSegment { Contributor = contributor, ReferenceString = referenceString };
            new EvaluationReferenceSegmentRepository().Create(referenceSegment);
            targetSegment.ReferenceSegment = referenceSegment;
        }

        private void ImportSegmentsElement(XElement paragraphSegment)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            IEnumerable<XElement> xElements = paragraphSegment.Elements(string.Format("{0}count-group", space));
            foreach (XElement xElement in xElements)
            {
                string name = xElement.Attribute("name").Value;
                string countString = xElement.Element(string.Format("{0}count", space)).Value;
                double count = Convert.ToDouble(countString, culture);

                if (name.StartsWith("Source"))
                {
                    string field = name.Substring(6);
                    SetSourceSegment(field, count);
                }
                else if (name.StartsWith("Target"))
                {
                    string field = name.Substring(6);
                    SetTargetSegment(field, count);
                }
                else
                {
                    SetTargetSegment(name, count);
                }
            }
        }

        private void SetSourceSegment(string fieldName, double count)
        {
            switch (fieldName)
            {
                case "WordCount": sourceSegment.WordCount = (int)count; break;
                case "Length": sourceSegment.Length = (int)count; break;
                case "SpellCheckFlags": sourceSegment.SpellCheckFlags = (int)count; break;
                case "IQScore": sourceSegment.IQScore = (int)count; break;
            }
        }

        private void SetTargetSegment(string fieldName, double count)
        {
            switch (fieldName)
            {
                case "WordCount": targetSegment.WordCount = (int)count; break;
                case "Length": targetSegment.Length = (int)count; break;
                case "SpellCheckFlags": targetSegment.SpellCheckFlags = (int)count; break;
                case "IQScore": targetSegment.IQScore = (int)count; break;
                case "GTM": targetSegment.GTM = count; break;
                case "BLEU": targetSegment.BLEU = count; break;
                case "TER": targetSegment.TER = count; break;
                case "Meteor": targetSegment.Meteor = count; break;
                case "EditDistance": targetSegment.EditDistance = (int)count; break;
            }
        }

        public EvaluationProject GetProject(int projectId)
        {
            //return EvaluationProjectRepository.GetProject(projectId);
            return generateEvaluationProjectToken(EvaluationProjectRepository.GetProject(projectId));
        }

        public EvaluationQuestion GetQuestion(int id, string key, string language, string category, string question)
        {
            //TODO
            return EvaluationQuestionsRepository.Get(id);
        }

        public EvaluationQuestion GetQuestion(int id)
        {
            return EvaluationQuestionsRepository.Get(id);
        }

        /// <summary>
        /// Get Languages
        /// </summary>
        /// <returns>return a list of languages</returns>
        public List<EvaluationLanguage> GetAllLanguages()
        {
            var list = EvaluationLanguagesRepository.GetAll().ToList();
            return list;
        }

        public List<EvaluationQuestion> GetAllQuestions(int Id)
        {

            var project = generateEvaluationProjectToken(EvaluationProjectRepository.GetProject(Id));
            var list = project.Questions.ToList();
            return list;
        }

        private bool IsValidDomain(string projectDomain, string requestDomain)
        {
            bool bValid = false;

            if (requestDomain == null)
            {
                return false;
            }


            //the project domain could be a semi colon delimited string so split.
            string[] list = projectDomain.Split(';');

            foreach (var s in list)
            {
                if (s == requestDomain)
                {
                    bValid = true;
                }
            }

            return bValid;
        }

        public List<EvaluationQuestion> GetAllQuestions(int Id, string key, string language, string category, string question, string domain)
        {
            var list = new List<EvaluationQuestion>();
            var dblist = new List<EvaluationQuestion>();
            var project = generateEvaluationProjectToken(EvaluationProjectRepository.GetProject(Id));
            int iQuestionId = 0;

            if (project == null)
            {
                throw new ArgumentException("Unknown Project", "ProjectId");
            }

            if (key == null || key != project.ApiKey)
            {
                throw new ArgumentException("Unknown API Key", "key");
            }

            if (!IsValidDomain(project.Domain, domain))
            {
                throw new ArgumentException("Invalid Domain (" + domain + ")", "domain");
            }


            if (category != null)
            {
                int iCategoryId = Convert.ToInt32(category);
                dblist = project.Questions.Where(cid => cid.Id == iCategoryId).ToList();
            }
            else
            {
                dblist = project.Questions.ToList();
            }

            if (question != null)
            {
                iQuestionId = Convert.ToInt32(question);
            }

            foreach (var item in dblist)
            {
                var evaluationQuestion = new EvaluationQuestion();

                evaluationQuestion.Id = item.Id;
                evaluationQuestion.Name = item.Name;

                foreach (var qitems in item.LanguageQuestions)
                {
                    if (iQuestionId == qitems.Id || iQuestionId == 0)
                    {
                        if (language == null || language == qitems.Language.Code)
                        {
                            if (evaluationQuestion.LanguageQuestions == null)
                            {
                                evaluationQuestion.LanguageQuestions = new List<EvaluationQuestionItem>();
                            }
                            evaluationQuestion.LanguageQuestions.Add(qitems);
                        }
                    }
                }

                if (evaluationQuestion.LanguageQuestions != null && evaluationQuestion.LanguageQuestions.Count > 0)
                {
                    list.Add(evaluationQuestion);
                }
            }

            return list;
        }

        public List<EvaluationProject> GetAllProjects()
        {
            var list = EvaluationProjectRepository.GetAll().ToList<EvaluationProject>();
            return list;
        }

        public List<EvaluationProject> GetProjectsByUser(string username)
        {
            var user = UserRepository.GetUser(username);

            if (user == null)
            {
                throw new ArgumentException("Unknown User", "username");
            }
            var list = EvaluationProjectRepository.GetByCreator(user.Id).ToList();
            return list;
        }

        public List<EvaluationSimpleScore> GetAllScores(int Id)
        {
            var list = EvaluationSimpleScoresRepository.GetProjectScores(Id).ToList();
            return list;
        }

        public bool DeleteAnswer(int id)
        {
            var questionList = EvaluationQuestionItemsRepository.GetAll();

            foreach (var evaluationQuestionItem in questionList)
            {
                foreach (var answerItem in evaluationQuestionItem.Answers)
                {
                    if (answerItem.Id == id)
                    {
                        evaluationQuestionItem.Answers.Remove(answerItem);
                        EvaluationQuestionItemsRepository.Update(evaluationQuestionItem);
                        EvaluationQuestionItemAnswersRepository.Delete(answerItem);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DeleteQuestion(int id)
        {
            var categoryList = EvaluationQuestionsRepository.GetAll();

            foreach (var category in categoryList)
            {
                foreach (var question in category.LanguageQuestions)
                {
                    if (question.Id == id)
                    {
                        category.LanguageQuestions.Remove(question);
                        EvaluationQuestionsRepository.Update(category);

                        foreach (var answer in question.Answers)
                        {
                            EvaluationQuestionItemAnswersRepository.Delete(answer);
                        }

                        EvaluationQuestionItemsRepository.Delete(question);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DeleteCategory(int id)
        {
            var project = EvaluationProjectRepository.GetAll().Where(p => p.Questions.Any(q => q.Id == id)).FirstOrDefault();

            if (project == null)
            {
                return false;
            }

            var category = project.Questions.Where(q => q.Id == id).FirstOrDefault();
            if (category == null)
            {
                return false;
            }

            while (category.LanguageQuestions.Count != 0)
            {
                var question = category.LanguageQuestions[0];
                category.LanguageQuestions.Remove(question);
                EvaluationQuestionsRepository.Update(category);

                while (question.Answers.Count != 0)
                {
                    var answer = question.Answers[0];
                    question.Answers.Remove(answer);
                    EvaluationQuestionItemsRepository.Update(question);
                    EvaluationQuestionItemAnswersRepository.Delete(answer);
                }
                EvaluationQuestionItemsRepository.Delete(question);
            }

            project.Questions.Remove(category);
            EvaluationProjectRepository.UpdateProject(project);
            EvaluationQuestionsRepository.Delete(category);
            return true;
        }

        #region InternalEvaluation
        public bool DeleteProject(int id)
        {
            var project = EvaluationProjectRepository.GetProject(id);
            if (project == null)
            {
                return false;
            }

            while (project.Questions.Count != 0)
            {
                var category = project.Questions[0];
                while (category.LanguageQuestions.Count != 0)
                {
                    var question = category.LanguageQuestions[0];
                    category.LanguageQuestions.Remove(question);
                    EvaluationQuestionsRepository.Update(category);

                    while (question.Answers.Count != 0)
                    {
                        var answer = question.Answers[0];
                        question.Answers.Remove(answer);
                        EvaluationQuestionItemsRepository.Update(question);
                        EvaluationQuestionItemAnswersRepository.Delete(answer);
                    }
                    EvaluationQuestionItemsRepository.Delete(question);
                }
                project.Questions.Remove(category);
                EvaluationProjectRepository.UpdateProject(project);
                EvaluationQuestionsRepository.Delete(category);
            }

            List<EvaluationUserProjectRole> userProjectRoles = EvaluationUserProjectRoleRepository.GetEvaluationUserProjectRoleByProject(project).ToList();
            foreach (EvaluationUserProjectRole eupr in userProjectRoles)
                EvaluationUserProjectRoleRepository.Delete(eupr);

            EvaluationProjectRepository.Delete(project);

            return true;
        }


        public InternalEvaluationAudit InsertEvaluationInternalAudit(InternalEvaluationAudit internalAudit)
        {
            if (InternalEvaluationAuditRepository.GetAllByProjectTokenAndUserAndMetadata(internalAudit.ProjectToken, internalAudit.UserName, internalAudit.Meta).ToList().Count() == 0)
                return InternalEvaluationAuditRepository.Insert(internalAudit);
            else
                throw (new Exception("Duplications are not allowed."));
        }

        public List<InternalEvaluationAudit> GetEvaluationInternalAudit(string token, string user, int status)
        {
            return InternalEvaluationAuditRepository.GetAllByProjectTokenAndUser(token, user, status).ToList();
        }

        public EvaluationProject GetProjectByAdminToken(string token)
        {
            return generateEvaluationProjectToken(EvaluationProjectRepository.GetProjectByAdminToken(token));
        }

        public List<EvaluationSimpleScore> GetAllScoresFiltered(int Id, string filter)
        {
            return EvaluationSimpleScoresRepository.GetProjectScoresFiltered(Id, filter).ToList();
        }

        public List<string> GetUserHistoryOnInternalEvaluationProject(int Id, string filter)
        {
            return EvaluationSimpleScoresRepository.GetUserHistoryOnInternalEvaluationProject(Id, filter).ToList();
        }

        public List<EvaluationSimpleScore> GetInternalScores(int Id, string filter)
        {
            return EvaluationSimpleScoresRepository.GetInternalScores(Id, filter).ToList();
        }


        public EvaluationProjectInvitation[] GenerateInvitations(string[] emails, int projectId, string uniqueRoleName, out string projectName)
        {

            EvaluationProject p = EvaluationProjectRepository.GetProject(projectId);

            if (p != null && p.Status == EvaluationStatus.InProgress)
            {

                projectName = p.Name;

                List<EvaluationProjectInvitation> newUsersList = new List<EvaluationProjectInvitation>();

                foreach (string email in emails)
                {

                    if (Utils.StringUtils.EmailValidator(email.Trim()))
                    {
                        User u = UserRepository.GetUserByUserName(email.Trim());

                        if (u != null)
                        {
                            //existing user: just associate him to the project.
                            EvaluationUserProjectRole userProjectRole = EvaluationUserProjectRoleRepository.GetEvaluationUserProjectRoleByUserAndProject(u, p);
                            if (userProjectRole == null)
                            {
                                //means the user as no connection to the project at all.
                                userProjectRole = new EvaluationUserProjectRole(u, RolesRepository.GetRole(uniqueRoleName), p);
                                EvaluationUserProjectRoleRepository.Insert(userProjectRole);
                                EvaluationProjectInvitation projectInvitation = new EvaluationProjectInvitation();
                                projectInvitation.UserName = email.Trim();
                                projectInvitation.ProjectId = p.Id;
                                projectInvitation.ConfirmationCode = Utils.StringUtils.Generate32CharactersStringifiedGuid();
                                projectInvitation.InvitationDate = DateTime.UtcNow;
                                projectInvitation.Type = 1;
                                EvaluationProjectInvitationRepository.Insert(projectInvitation);
                                newUsersList.Add(projectInvitation);
                            }
                            else
                            {
                                //what to do if the user already exists? Currently nothing...                                                          
                            }

                        }
                        else
                        {
                            //new user goes to invitation list.
                            EvaluationProjectInvitation projectInvitation = new EvaluationProjectInvitation();
                            projectInvitation.UserName = email.Trim();
                            projectInvitation.ProjectId = p.Id;
                            projectInvitation.ConfirmationCode = Utils.StringUtils.Generate32CharactersStringifiedGuid();
                            projectInvitation.InvitationDate = DateTime.UtcNow;
                            projectInvitation.Type = 2;
                            EvaluationProjectInvitationRepository.Insert(projectInvitation);
                            newUsersList.Add(projectInvitation);
                        }

                    }

                }


                return newUsersList.ToArray();

            }

            projectName = string.Empty;
            return new EvaluationProjectInvitation[] { }; ;

        }

        public EvaluationProjectInvitation GetProjectInvite(string code)
        {
            return EvaluationProjectInvitationRepository.GetProjecInvitationtByConfirmationCode(code);
        }

        public EvaluationProjectInvitation GetProjectInviteByUserName(string userName)
        {
            return EvaluationProjectInvitationRepository.GetNextValidProjectInvitationByUserName(userName);
        }

        public EvaluationProjectInvitation UpdateProjectInvitationConfirmationCode(string code)
        {
            EvaluationProjectInvitation projInvitation = EvaluationProjectInvitationRepository.GetProjecInvitationtByConfirmationCode(code);
            User u = UserRepository.GetUserByUserName(projInvitation.UserName);
            EvaluationProject p = EvaluationProjectRepository.GetProject(projInvitation.ProjectId);
            EvaluationUserProjectRole userProjectRole = new EvaluationUserProjectRole(u, RolesRepository.GetRole("ProjUser"), p);
            EvaluationUserProjectRoleRepository.Insert(userProjectRole);
            projInvitation.ConfirmationCode = string.Empty;
            return EvaluationProjectInvitationRepository.UpdateProjectInvitation(projInvitation);
        }

        public EvaluationProjectInvitation UpdateProjectInvitationConfirmationDate(string code)
        {
            EvaluationProjectInvitation projInvitation = EvaluationProjectInvitationRepository.GetProjecInvitationtByConfirmationCode(code);
            projInvitation.ConfirmationCode = string.Empty;
            projInvitation.ConfirmationDate = DateTime.UtcNow;
            return EvaluationProjectInvitationRepository.UpdateProjectInvitation(projInvitation);
        }

        public List<EvaluationProjectInvitation> GetInvitationsByProject(int projectId)
        {
            return EvaluationProjectInvitationRepository.GetAllByProjectId(projectId).ToList<EvaluationProjectInvitation>();
        }

        public EvaluationProjectInvitation UpdateInvitation(EvaluationProjectInvitation projectInvitation)
        {
            return EvaluationProjectInvitationRepository.UpdateProjectInvitation(projectInvitation);
        }

        public void AddUserToProject(string userName, int projectId)
        {
            User u = UserRepository.GetUserByUserName(userName);
            if (u == null)
                throw new Exception("User is null.");

            EvaluationProject p = EvaluationProjectRepository.GetProject(projectId);
            if (p == null)
                throw new Exception("Project is null.");

            EvaluationUserProjectRole userProjectRole = new EvaluationUserProjectRole(u, RolesRepository.GetRole("ProjUser"), p);
            EvaluationUserProjectRoleRepository.Insert(userProjectRole);
        }

        public void AddUserToProject(string userName, string projectAdminToken)
        {
            User u = UserRepository.GetUserByUserName(userName);
            if (u == null)
                throw new Exception("User is null.");

            EvaluationProject p = EvaluationProjectRepository.GetEvaluationProjectByAdminToken(projectAdminToken);
            if (p == null)
                throw new Exception("Project is null.");

            EvaluationUserProjectRole userProjectRole = new EvaluationUserProjectRole(u, RolesRepository.GetRole("ProjUser"), p);

            EvaluationUserProjectRoleRepository.Insert(userProjectRole);
        }

        public List<EvaluationProjectInvitation> GetProjectInvitationsByUserName(string userName)
        {
            return EvaluationProjectInvitationRepository.GetAllByUserName(userName).ToList<EvaluationProjectInvitation>();
        }

        public EvaluationUserProjectRole GetRoleInProjectByProjectAndUser(EvaluationProject project, User user)
        {
            return EvaluationUserProjectRoleRepository.GetEvaluationUserProjectRoleByUserAndProject(user, project);
        }


        #endregion

        public EvaluationContentChunk InsertContentChunk(EvaluationContentChunk newContentChunk)
        {
            return EvaluationContentChunkRepository.Insert(newContentChunk);
        }

        public EvaluationProject UpdateProject(EvaluationProject project)
        {
            return EvaluationProjectRepository.UpdateProject(project);
        }

        private EvaluationProject generateEvaluationProjectToken(EvaluationProject p)
        {
            if (p.AdminToken == null || p.AdminToken == string.Empty)
            {
                p.AdminToken = Utils.StringUtils.GenerateTinyHash(p.Name + p.Id + DateTime.UtcNow.ToString());
                return EvaluationProjectRepository.UpdateProject(p);
            }

            return p;
        }

    }
}
