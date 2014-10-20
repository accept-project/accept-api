using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEditAudit;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Interfaces.PostEditAudit
{
    public interface IPostEditAuditManager
    {
        Document GetDocumentForPostEditByUserId(string textId, string userIdentifier, out DateTime? lastBlockDate);   

        DocumentRevision AddTranslationRevisionPhase(string userIdentifier, string textId, int index, DateTime timeStamp, string state, string phaseName, string source, string target, Phase revisionPhase);

        string GenerateXliffFormatWithAlternativeTranslations(string textId, string userIdentifier, string startPeTool, string startPeToolId, string startPeProcessName);

        DocumentRevision CompleteDocument(string textId, string userIdentifier, int projectQuestionId, string reply);

        DocumentRevision UpdateRevisionTotalEditingTime(string textId, string userIdentifier, string seconds);

        DocumentRevision GetDocumentRevisionByUserIdAndTextId(string userId, string textId);

        List<DocumentRevision> GetAllDocumentRevisionsByTextId(string textId);

        string GetProjectOwnerXliff(string projectToken, bool showUserInfo, string startPeTool, string startPeToolId, string startPeProcessName);

        List<DocumentRevision> GetAllRevisionsByUserId(string userName);

        string GetAllUserRevisionsPerTaskXliff(string token, string textId, string startPeTool, string startPeToolId, string startPeProcessName);

        List<List<string>> GetAllTargetSentencesForPostEditDocument(string textId, string userIdentifier);

        DocumentRevision AddTranslationRevisionThinkingPhase(string userIdentifier, string textId, int index, DateTime timeStamp, string state, string phaseName, string source, string target, ThinkPhase revisionPhase);
    }
}
    