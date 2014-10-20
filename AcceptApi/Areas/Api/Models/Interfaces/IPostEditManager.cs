using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Domain.PostEditAudit;
using AcceptApi.Areas.Api.Models.PostEdit;

namespace AcceptApi.Areas.Api.Models.Interfaces
{
    public interface IPostEditManager
    {
        CoreApiResponse GetPostEditContent();
        Document GetPostEditContentRaw();
        CoreApiResponse AddDocumentToProjectRaw(string jsonRaw, int projectId);
        CoreApiResponse AddDocumentToProject(string token, Document document);                
        CoreApiResponse GetDocumentByDocumentId(int documentId);
        CoreApiResponse GetDocumentByTextId(string textId);
        CoreApiResponse GetProjectDocumentsByUser(int projectId, string userName);               
        #region Audit       
        CoreApiResponse GetDocumentForPostEditByUserId(string textId, string userIdentifier);        
        CoreApiResponse SaveRevisionPhase(string userIdentifier, string textId, int index, DateTime timeStamp, string state, string phaseName, string source, string target, Phase phase, List<PhaseCount> phaseCounts);        
        CoreApiResponse CompleteDocument(string textId, string userIdentifier, int projectQuestionId, string reply);
        CoreApiResponse UpdateRevisionTotalEditingTime(string textId, string userIdentifier, string seconds);
        string GenerateXliffFormat(string textId, string userIdentifier);
        CoreApiResponse InviteUsersToProject(string[] emails, int projectId, string uniqueRoleName, string projectOwner);
        CoreApiResponse GetProjectInvitation(string code);
        CoreApiResponse GetProjectInvitationByUserName(string userName);
        CoreApiResponse UpdateInvitationCode(string code);
        CoreApiResponse DeleteDocument(string textId, string userName);
        CoreApiResponse UpdateInvitationConfirmationDateByCode(string code);
        string GenerateProjectOwnerXliff(string projectToken, bool showUserInfo);
        string GenerateTaskRevisionsXliff(string projectToken, string textId);
        CoreApiResponse GetProjectTaskStatus(int projectId, string token);
        CoreApiResponse GetTaskStatus(string textId);


        #endregion



    }
}
