using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Interfaces.PostEditAudit;
using AcceptFramework.Domain.PostEditAudit;
using AcceptFramework.Domain.PostEdit;
using AcceptFramework.Repository.PostEdit;
using AcceptFramework.Repository.PostEditAudit;
using AcceptFramework.Business.Utils;
using System.Xml.Linq;
using System.Globalization;
using AcceptFramework.Domain.Audit;
using AcceptFramework.Repository.Audit;
using AcceptFramework.Repository.Common;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Business.PostEditAudit
{
    internal class PostEditAuditManager : IPostEditAuditManager
    {
        public PostEditAuditManager()
        {            
            //TODO: create all audit type objects
        }
         
        public Document GetDocumentForPostEditByUserId(string textId, string userIdentifier, out DateTime? lastBlockDate)
        {                       

            Document originalDocument = DocumentRepository.GetDocumentByTextId(textId);
            lastBlockDate = null;
            
            if (originalDocument == null)
                throw new Exception("Document not found:" + textId);

            if(originalDocument.Project.Status != 1)
                throw new Exception("Invalid project status.");
                                  
            //check if the user exists on our system before letting the document go.
            Project revisionProject; revisionProject = originalDocument.Project;

            if (revisionProject.External)
            {
                ExternalPostEditUser externalUser = ExternalPostEditUserRepository.GetExternalPostEditUserByUserName(userIdentifier);
                
                if(externalUser == null)
                    externalUser = ExternalPostEditUserRepository.Insert(new ExternalPostEditUser(userIdentifier,DateTime.UtcNow,(userIdentifier + revisionProject.Id + DateTime.UtcNow).ToMD5(),false));

                if (externalUser == null)
                    throw new Exception("Problem creating new external user.");

                ExternalUserProjectRole extUserProjRole = ExternalUserProjectRoleRepository.GetUserProjectRoleByExternalUserAndProject(externalUser, revisionProject);
                
                if(extUserProjRole == null)
                    extUserProjRole = ExternalUserProjectRoleRepository.Insert(new ExternalUserProjectRole(externalUser,RolesRepository.GetRole("ProjUser"),revisionProject));

                if(extUserProjRole == null)
                    throw new Exception("Problem creating new external user role.");
            }
            else
            { 
                  User user = UserRepository.GetUserByUserName(userIdentifier);
                
                  if(user == null)
                    throw new Exception(string.Format("User: {0} not found.", userIdentifier));
            }

            DocumentRevision documentRevision; documentRevision = null;

            if (originalDocument.IsSingleRevision != null && originalDocument.IsSingleRevision == true && revisionProject.IsSingleRevision != null && revisionProject.IsSingleRevision == true)
            {
                #region Single Revision
                
                documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifierWithUpdateNoWaitLock(textId, originalDocument.UniqueReviewerId);

                if (documentRevision != null)
                {
                    #region Revision Exists                    

                    #region Locking Document
                    if (documentRevision.IsLocked && documentRevision.LockedBy != userIdentifier)
                    {
                      
                        TimeSpan timeElapsedBetweenRevisionLocked = (DateTime.UtcNow - Convert.ToDateTime(documentRevision.LockedDate));
                        AuditPostEditRevisionLock auditRevisionLock;
                        if ((timeElapsedBetweenRevisionLocked < revisionProject.MaxIdleThreshold) || (timeElapsedBetweenRevisionLocked < revisionProject.MaxThreshold))
                        {
                            DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);
                            auditRevisionLock = new AuditPostEditRevisionLock("locked-revision-requested-failed", userIdentifier, null, DateTime.UtcNow, textId);
                            AuditPostEditRevisionLockRepository.Insert(auditRevisionLock);
                            throw new Exception("Document revision is currently locked.");
                        }

                        auditRevisionLock = new AuditPostEditRevisionLock("revision-lock-update", userIdentifier, "previously-locked-by:" + documentRevision.LockedBy, DateTime.UtcNow, textId);
                        documentRevision.IsLocked = true;
                        documentRevision.LockedBy = userIdentifier;
                        documentRevision.LockedDate = DateTime.UtcNow;                        
                        documentRevision = DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);
                        AuditPostEditRevisionLockRepository.Insert(auditRevisionLock);                        
                    }
                    else
                        if (!documentRevision.IsLocked && documentRevision.LockedBy != userIdentifier)
                        {
                            documentRevision.IsLocked = true;
                            documentRevision.LockedBy = userIdentifier;
                            documentRevision.LockedDate = DateTime.UtcNow;
                            documentRevision = DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);
                            AuditPostEditRevisionLock auditRevisionLock = new AuditPostEditRevisionLock("revision-lock", userIdentifier, null, DateTime.UtcNow, textId);
                            AuditPostEditRevisionLockRepository.Insert(auditRevisionLock);                           
                        }
                        else
                        {
                            documentRevision.IsLocked = true;
                            documentRevision.LockedBy = userIdentifier;
                            if(revisionProject.ResetBlockTimeStampWhenSameUser)
                                documentRevision.LockedDate = DateTime.UtcNow;
                            documentRevision = DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);

                            AuditPostEditRevisionLock auditRevisionLock = new AuditPostEditRevisionLock("locked-revision-requested-ok", userIdentifier, null, DateTime.UtcNow, textId);
                            AuditPostEditRevisionLockRepository.Insert(auditRevisionLock);
                        }
                    
                    lastBlockDate = documentRevision.LockedDate;

                    #endregion

                    #region Prepare Document

                    int targetIndex = 1;

                    foreach (TargetSentence target in originalDocument.tgt_sentences)
                    {
                        TranslationRevision lastTranslationRevisionForSegment; lastTranslationRevisionForSegment = null;
                        try
                        {
                            lastTranslationRevisionForSegment = documentRevision.TranslationUnits.OrderByDescending(x => x.DateCreated).Where(a => a.SegmentIndex == targetIndex).ToList().First();
                        }
                        catch { lastTranslationRevisionForSegment = null; }

                        if (lastTranslationRevisionForSegment != null)
                        {
                            target.text = lastTranslationRevisionForSegment.Target;
                            int lastThinkNoteId = -1;
                            int lastRevisionNoteId = 0;
                            if (lastTranslationRevisionForSegment.ThinkPhases.Count > 0)
                            {
                                //think phase notes/comments.                                                      
                                List<PhaseNote> lastThinkPhaseNotes; lastThinkPhaseNotes = null;
                                try
                                {
                                    lastTranslationRevisionForSegment.ThinkPhases.ToArray<Phase>().Last().Notes.OrderBy(x => x.Id).Where(a => a.Annotates == "general").ToList<PhaseNote>();
                                }
                                catch { }

                                if (lastThinkPhaseNotes != null && lastThinkPhaseNotes.Count > 0)
                                {
                                    try
                                    {
                                        target.lastComment = lastThinkPhaseNotes.Last().Note;
                                        lastThinkNoteId = lastThinkPhaseNotes.Last().Id;
                                    }
                                    catch { }
                                    try
                                    {
                                        target.lastOption = lastThinkPhaseNotes.OrderBy(x => x.Id).Where(a => a.Annotates == "target" && a.NoteFrom == "user").Last().Note;
                                    }
                                    catch { }

                                }
                            
                            }
                            if (lastTranslationRevisionForSegment.Phases.Count > 0)
                            {   
                                //edition phases.
                                List<PhaseNote> lastPhaseNotes; lastPhaseNotes = null;
                                try
                                {
                                    lastPhaseNotes = lastTranslationRevisionForSegment.Phases.ToArray<Phase>().Last().Notes.OrderBy(x => x.Id).Where(a => a.Annotates == "general").ToList<PhaseNote>();
                                }
                                catch { }
                                if (lastPhaseNotes != null && lastPhaseNotes.Count > 0)
                                {
                                    try
                                    {
                                        lastRevisionNoteId = lastPhaseNotes.Last().Id;
                                    }
                                    catch { }

                                    if (lastRevisionNoteId > lastThinkNoteId)
                                    {
                                        try
                                        {
                                            target.lastComment = lastPhaseNotes.Last().Note;
                                        }
                                        catch { }
                                        try
                                        {
                                            target.lastOption = lastPhaseNotes.OrderBy(x => x.Id).Where(a => a.Annotates == "target" && a.NoteFrom == "user").Last().Note;
                                        }
                                        catch { }
                                    }
                                }
                            }

                        }

                        targetIndex++;
                    }

                    return originalDocument;
                    #endregion


                    #endregion
                }
                else
                {
                    #region New Revision
                        documentRevision = new DocumentRevision((originalDocument.IsSingleRevision == true ? originalDocument.UniqueReviewerId : userIdentifier), textId, 1, DateTime.UtcNow, DateTime.UtcNow, (DateTime)System.Data.SqlTypes.SqlDateTime.Null, "", new List<TranslationRevision>());
                        int index = 0;
                        for (int i = 0; i < originalDocument.tgt_sentences.Count; i++)
                        {
                            TranslationRevision mtTrasnUnitRevision = new TranslationRevision(++index, "needs-review-translation", "mt_baseline", originalDocument.src_sentences.ToArray<SourceSentence>()[i].text, originalDocument.tgt_sentences.ToArray<TargetSentence>()[i].text, documentRevision.DateCreated, new List<Phase>(), documentRevision.DateCreated);
                            documentRevision.TranslationUnits.Add(mtTrasnUnitRevision);
                        }
                        documentRevision.IsLocked = true;
                        documentRevision.LockedBy = userIdentifier;
                        documentRevision.LockedDate = DateTime.UtcNow;
                        DocumentRevisionRepository.Insert(documentRevision);
                        AuditPostEditRevisionLock auditRevisionLock = new AuditPostEditRevisionLock("revision-lock-init", userIdentifier, null, DateTime.UtcNow, textId);
                        AuditPostEditRevisionLockRepository.Insert(auditRevisionLock);
                        //avoid lazy load issue the first time each task is loaded.                               
                        originalDocument.DocumentRevisions = DocumentRevisionRepository.GetAllDocumentRevisionBytexttId(textId).ToList();                                   
                        DocumentRepository.UpdateDocument(originalDocument);
                        return originalDocument;
                    #endregion
                }

                #endregion
            }
            else
            {
                documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userIdentifier);

                if (documentRevision != null)
                {
                    #region Revision Exists

                    #region Prepare Document

                    int targetIndex = 1;

                    foreach (TargetSentence target in originalDocument.tgt_sentences)
                    {
                        TranslationRevision lastTranslationRevisionForSegment; lastTranslationRevisionForSegment = null;
                        try
                        {
                            lastTranslationRevisionForSegment = documentRevision.TranslationUnits.OrderByDescending(x => x.DateCreated).Where(a => a.SegmentIndex == targetIndex).ToList().First();
                        }
                        catch { lastTranslationRevisionForSegment = null; }

                        if (lastTranslationRevisionForSegment != null)
                        {
                            target.text = lastTranslationRevisionForSegment.Target;
                            int lastThinkNoteId = -1;
                            int lastRevisionNoteId = 0;
                            if (lastTranslationRevisionForSegment.ThinkPhases.Count > 0)
                            {
                                //think phase notes/comments.                                                      
                                List<PhaseNote> lastThinkPhaseNotes; lastThinkPhaseNotes = null;
                                #region user comments
                                try
                                {
                                  lastThinkPhaseNotes = lastTranslationRevisionForSegment.ThinkPhases.ToArray<Phase>().Last().Notes.OrderBy(x => x.Id).Where(a => a.Annotates == "general").ToList<PhaseNote>();
                                }
                                catch { }

                                if (lastThinkPhaseNotes != null && lastThinkPhaseNotes.Count > 0)
                                {
                                    try
                                    {
                                        target.lastComment = lastThinkPhaseNotes.Last().Note;
                                        lastThinkNoteId = lastThinkPhaseNotes.Last().Id;
                                    }
                                    catch { }                                   

                                }
                                #endregion
                                else
                                {
                                    //since no "general" comments from "user" were found we now need to look for project pre-defined options (client ddl options).
                                    #region pre-defined options
                                    try
                                    {
                                        lastThinkPhaseNotes = lastTranslationRevisionForSegment.ThinkPhases.ToArray<Phase>().Last().Notes.OrderBy(x => x.Id).Where(a => a.Annotates == "target" && a.NoteFrom == "user").ToList<PhaseNote>();
                                    }
                                    catch { }
                                    if (lastThinkPhaseNotes != null && lastThinkPhaseNotes.Count > 0)
                                    {
                                        try
                                        {
                                            target.lastOption = lastThinkPhaseNotes.Last().Note;
                                            lastThinkNoteId = lastThinkPhaseNotes.Last().Id;
                                        }
                                        catch { }
                                    }
                                    #endregion
                                }
                               
                            }
                            if (lastTranslationRevisionForSegment.Phases.Count > 0)
                            {
                                //edition phase notes/comments.    
                                List<PhaseNote> lastPhaseNotes; lastPhaseNotes = null;
                                #region user comments
                                try
                                {
                                    lastPhaseNotes = lastTranslationRevisionForSegment.Phases.ToArray<Phase>().Last().Notes.OrderBy(x => x.Id).Where(a => a.Annotates == "general").ToList<PhaseNote>();
                                }
                                catch { }

                                if (lastPhaseNotes != null && lastPhaseNotes.Count > 0)
                                {
                                    try
                                    {
                                        lastRevisionNoteId = lastPhaseNotes.Last().Id;
                                    }
                                    catch { }

                                    if (lastRevisionNoteId > lastThinkNoteId)
                                    {
                                        try
                                        {
                                            target.lastComment = lastPhaseNotes.Last().Note;
                                        }
                                        catch { }                                        
                                    }

                                #endregion
                                }
                                else
                                {
                                    #region pre-defined options
                                    try
                                    {
                                        lastPhaseNotes = lastTranslationRevisionForSegment.Phases.ToArray<Phase>().Last().Notes.OrderBy(x => x.Id).Where(a => a.Annotates == "target" && a.NoteFrom == "user").ToList<PhaseNote>();
                                    }
                                    catch { }
                                    if (lastPhaseNotes != null && lastPhaseNotes.Count > 0)
                                    {
                                        try
                                        {
                                            lastRevisionNoteId = lastPhaseNotes.Last().Id;
                                        }
                                        catch { }

                                        if (lastRevisionNoteId > lastThinkNoteId)
                                        {
                                            try
                                            {
                                                target.lastOption = lastPhaseNotes.Last().Note;
                                            }
                                            catch { }                                         
                                        }
                                    
                                    }


                                    #endregion
                                }
                            }

                        }

                        targetIndex++;
                    }

                    return originalDocument;

                    #endregion

                    #endregion
                }
                else
                {
                    #region New Revision
                    documentRevision = new DocumentRevision((originalDocument.IsSingleRevision == true ? originalDocument.UniqueReviewerId : userIdentifier), textId, 1, DateTime.UtcNow, DateTime.UtcNow, (DateTime)System.Data.SqlTypes.SqlDateTime.Null, "", new List<TranslationRevision>());
                    int index = 0;
                    for (int i = 0; i < originalDocument.tgt_sentences.Count; i++)
                    {
                        TranslationRevision mtTrasnUnitRevision = new TranslationRevision(++index, "needs-review-translation", "mt_baseline", originalDocument.src_sentences.ToArray<SourceSentence>()[i].text, originalDocument.tgt_sentences.ToArray<TargetSentence>()[i].text, documentRevision.DateCreated, new List<Phase>(), documentRevision.DateCreated);
                        documentRevision.TranslationUnits.Add(mtTrasnUnitRevision);
                    }
                    DocumentRevisionRepository.Insert(documentRevision);
                    //avoid lazy load issue first time the task is loaded.                               
                    originalDocument.DocumentRevisions = DocumentRevisionRepository.GetAllDocumentRevisionBytexttId(textId).ToList();                         
                    DocumentRepository.UpdateDocument(originalDocument);
                    return originalDocument;
                    #endregion
                }
            }            
        }

        public List<List<string>> GetAllTargetSentencesForPostEditDocument(string textId, string userIdentifier)
        {
            try
            {
                List<List<string>> allTargetRevisions = new List<List<string>>();
                Document originalDocument = DocumentRepository.GetDocumentByTextId(textId);
                if (originalDocument == null)
                throw new Exception("Document not found" + textId);
                if(originalDocument.Project.Status != 1)
                throw new Exception("Invalid project");
            
                DocumentRevision documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userIdentifier);
                
                if (documentRevision != null)
                {

                    foreach(TranslationRevision trevision in documentRevision.TranslationUnits)
                    {
                        allTargetRevisions.Add(trevision.AlternativeTranslations.Select(a => a.Target).ToList<string>());
                        allTargetRevisions[allTargetRevisions.Count - 1].Add(trevision.Target);
                    }
                  
                }

                return allTargetRevisions;

            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public DocumentRevision CompleteDocument(string textId, string userIdentifier, int projectQuestionId, string reply)
        { 
            DocumentRevision documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userIdentifier);

            if (documentRevision != null)
            {
                    if (projectQuestionId > 0)
                    {
                        ProjectQuestion question = ProjectQuestionRepository.GetProjectQuestion(projectQuestionId);

                        if (question != null && reply != null && reply.Length > 0)                                                    
                            documentRevision.QuestionsReplied.Add(new QuestionReply(question, reply));
                        
                    }
                    documentRevision.CompleteDate = DateTime.UtcNow;
                    documentRevision.Status = 2;
                    return DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);              
            }
            else
                throw new ArgumentException("Document revision is null");
        }

        public DocumentRevision UpdateRevisionTotalEditingTime(string textId, string userIdentifier, string seconds)
        {
            DocumentRevision documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userIdentifier);

            if (documentRevision != null)
            {
                double secondsToAdd = 0;                           
                double.TryParse(seconds, out secondsToAdd);
                documentRevision.DateLastUpdate = documentRevision.DateLastUpdate.AddSeconds(secondsToAdd);               
                return DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);

            }
            else
                throw new ArgumentException("Document revision is null");
        }
        
        /// <summary>
        /// Add new "edition" phase.
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <param name="textId"></param>
        /// <param name="index"></param>
        /// <param name="timeStamp"></param>
        /// <param name="state"></param>
        /// <param name="phaseName"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="revisionPhase"></param>
        /// <returns></returns>
        public DocumentRevision AddTranslationRevisionPhase(string userIdentifier, string textId, int index, DateTime timeStamp, string state, string phaseName, string source, string target, Phase revisionPhase)
        {

            #region Single Revision
            DocumentRevision documentRevision; documentRevision = null;
            Document originalDocument; originalDocument = DocumentRepository.GetDocumentByTextId(textId);
            
            if (originalDocument == null)
                throw new Exception("Document is null.");

            if (originalDocument.IsSingleRevision != null && originalDocument.IsSingleRevision)
            {
                documentRevision = DocumentRevisionRepository.GetDocumentRevisionByTextId(textId.Trim());
                if (documentRevision == null)
                    throw new Exception("Single revision is null.");

                if (userIdentifier != documentRevision.LockedBy)
                {                   
                        AuditPostEditRevisionLockRepository.Insert(new AuditPostEditRevisionLock("locked-revision-segment-update-failed", userIdentifier, null, DateTime.UtcNow, textId));
                        throw new Exception("Document revision is currently locked.");                    
                }
            }           
            else
            {
                documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userIdentifier);
                if (documentRevision == null)
                    throw new Exception("Document revision is null.");        
            }

            #endregion

            var itemList = from t in documentRevision.TranslationUnits
                           where t.SegmentIndex == index                           
                           select t;
            //if already exists a transalation unit for the current segment and for the current user.
            if (itemList != null && itemList.ToArray<TranslationRevision>().Length > 0)
            {
                List<TranslationRevision> revisions = itemList.ToList<TranslationRevision>();
                TranslationRevision r = TranslationRevisionRepository.GetTranslationRevision(revisions[0].Id);                                                                                
                //add the new Phase revision for the current segment (ATTENTION: The new phase name needs to be: the count of current Phases for the segment + 1 new Phase)
                //this incremental work keeps us from having always to send back to the client the number of the current phase revision.
                revisionPhase.PhaseName = "r" + index.ToString() + "." + (r.Phases.Count + 1).ToString();
                revisionPhase.PhaseCountGroup.Name = index.ToString();
                if (revisionPhase.PhaseCountGroup.PhaseCounts != null)
                foreach (PhaseCount count in revisionPhase.PhaseCountGroup.PhaseCounts)
                    count.PhaseName = revisionPhase.PhaseName;

                r.Phases.Add(revisionPhase);
                //before update the phase name transform the current main translation: in a new alternative translation.
                AlternativeTranslation newAlternativeTranslation = new AlternativeTranslation(r.PhaseName, r.Target);
                r.AlternativeTranslations.Add(newAlternativeTranslation);
                r.PhaseName = revisionPhase.PhaseName;
                r.Target = target;
                r.LastUpdate = timeStamp;                                               
                TranslationRevisionRepository.UpdateTranslationRevision(r);
                documentRevision = DocumentRevisionRepository.GetDocumentRevision(documentRevision.Id);
                //update the last update date field for the document.
                documentRevision.DateLastUpdate = timeStamp;
                return DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);
            }
            else
            {
                //build edition phase("r.number") acronym.
                string newPhaseName = "r" + index.ToString() + ".1";

                if (revisionPhase.PhaseCountGroup.PhaseCounts != null)
                    foreach (PhaseCount pcount in revisionPhase.PhaseCountGroup.PhaseCounts)
                        pcount.PhaseName = newPhaseName;

                List<Phase> translationUnitphases = new List<Phase>(); translationUnitphases.Add(PhaseRepository.Insert(revisionPhase));
                TranslationRevision newTranslationRevision = new TranslationRevision(index, state, newPhaseName, source, target, timeStamp, translationUnitphases, timeStamp);
                TranslationRevisionRepository.Insert(newTranslationRevision);                
                documentRevision.TranslationUnits.Add(newTranslationRevision);
                //update the last update date field for the document.
                documentRevision.DateLastUpdate = timeStamp;
                return DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);
            }           
        }
        
        /// <summary>
        /// Add new "thinking phase".
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <param name="textId"></param>
        /// <param name="index"></param>
        /// <param name="timeStamp"></param>
        /// <param name="state"></param>
        /// <param name="phaseName"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="revisionPhase"></param>
        /// <returns></returns>
        public DocumentRevision AddTranslationRevisionThinkingPhase(string userIdentifier, string textId, int index, DateTime timeStamp, string state, string phaseName, string source, string target, ThinkPhase revisionPhase)
        {

            #region Single Revision
            DocumentRevision documentRevision;documentRevision = null;
            Document originalDocument; originalDocument = DocumentRepository.GetDocumentByTextId(textId);
            if (originalDocument == null)
                throw new Exception("Document is null.");
                      
            if (originalDocument.IsSingleRevision != null && originalDocument.IsSingleRevision)
            {
                documentRevision = DocumentRevisionRepository.GetDocumentRevisionByTextId(textId);
                if (documentRevision == null)
                    throw new Exception("Single revision is null.");

                if (userIdentifier != documentRevision.LockedBy)
                {                    
                    AuditPostEditRevisionLockRepository.Insert(new AuditPostEditRevisionLock("locked-revision-segment-update-failed", userIdentifier, null, DateTime.UtcNow, textId));
                    throw new Exception("Document revision is currently locked.");
                }
            }
            else
            {
                documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userIdentifier);
                if (documentRevision == null)
                    throw new Exception("Document revision is null.");        
            }
            #endregion

            var itemList = from t in documentRevision.TranslationUnits
                           where t.SegmentIndex == index
                           select t;

            //if already exists a transalation unit for the current segment and for the current user.
            if (itemList != null && itemList.ToArray<TranslationRevision>().Length > 0)
            {
                List<TranslationRevision> revisions = itemList.ToList<TranslationRevision>();
                TranslationRevision r = TranslationRevisionRepository.GetTranslationRevision(revisions[0].Id);

                //add the new Phase revision for the current segment (ATTENTION: The new phase name needs to be: the count of current Phases for the segment + 1 new Phase)
                //this incremental work keeps us from having always to send back to the client the number of the current phase revision.
                revisionPhase.PhaseName = "t" + index.ToString() + "." + (r.ThinkPhases.Count + 1).ToString();
                revisionPhase.PhaseCountGroup.Name = index.ToString();

                if(revisionPhase.PhaseCountGroup.PhaseCounts != null)
                    foreach (PhaseCount count in revisionPhase.PhaseCountGroup.PhaseCounts)
                        count.PhaseName = revisionPhase.PhaseName;

                r.ThinkPhases.Add(revisionPhase);          
                TranslationRevisionRepository.UpdateTranslationRevision(r);
                documentRevision = DocumentRevisionRepository.GetDocumentRevision(documentRevision.Id);
                return DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);
            }
            else
            {
                //build think phase("r.number") acronym.
                string newPhaseName = "t" + index.ToString() + ".1";
                if (revisionPhase.PhaseCountGroup.PhaseCounts != null)
                foreach (PhaseCount pcount in revisionPhase.PhaseCountGroup.PhaseCounts)
                    pcount.PhaseName = newPhaseName;

                List<ThinkPhase> translationUnitphases = new List<ThinkPhase>(); translationUnitphases.Add(ThinkPhaseRepository.Insert(revisionPhase));
                TranslationRevision newTranslationRevision = new TranslationRevision(index, state, newPhaseName, source, target, timeStamp, new List<Phase>(), timeStamp);
                TranslationRevisionRepository.Insert(newTranslationRevision);
                documentRevision.TranslationUnits.Add(newTranslationRevision);
                return DocumentRevisionRepository.UpdateDocumentRevision(documentRevision);
            }
        }
      
        public DocumentRevision GetDocumentRevisionByUserIdAndTextId(string userId, string textId)
        {
            return DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userId);
        }

        public List<DocumentRevision> GetAllDocumentRevisionsByTextId(string textId)
        {
            return DocumentRevisionRepository.GetAllDocumentRevisionBytexttId(textId).ToList();
        }

        #region Xliff Rrports Generation

        public List<DocumentRevision> GetAllRevisionsByUserId(string userName)
        {
            return DocumentRevisionRepository.GetAllDocumentRevisionByUserIdentifier(userName).ToList<DocumentRevision>();
        }

        public string GenerateXliffFormatWithAlternativeTranslations(string textId, string userIdentifier, string startPeTool, string startPeToolId, string startPeProcessName)
        {
            Document originalDocument = DocumentRepository.GetDocumentByTextId(textId);
            DocumentRevision documentRevision = DocumentRevisionRepository.GetDocumentRevisionByUserIdentifier(textId, userIdentifier);

            if (documentRevision == null)
            {
                documentRevision = new DocumentRevision();
                int index = 0;
                for (int i = 0; i < originalDocument.tgt_sentences.Count; i++)
                {
                    TranslationRevision mtTrasnUnitRevision = new TranslationRevision(++index, "needs-review-translation", "mt_baseline", originalDocument.src_sentences.ToArray<SourceSentence>()[i].text, originalDocument.tgt_sentences.ToArray<TargetSentence>()[i].text, documentRevision.DateCreated, new List<Phase>(), documentRevision.DateCreated);
                    documentRevision.TranslationUnits.Add(mtTrasnUnitRevision);
                }

            }

            XDocument doc;
            DateTime dateUTC;
            string processName = string.Empty;
            string mtDate;
            string mtContactEmail; 
            string mtBaseLineProcessName = startPeProcessName;
            mtContactEmail = originalDocument.MtContactEmail != null ? mtContactEmail = originalDocument.MtContactEmail : mtDate = string.Empty;      
            processName = originalDocument.Project.InterfaceConfiguration == 1 ? processName = "bilingual" :  processName = "monolingual";                                    
            
            try
            {
                dateUTC = DateTime.Parse(originalDocument.MtDate != null ? mtDate = originalDocument.MtDate : mtDate = string.Empty);
                mtDate = dateUTC.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");                
            }
            catch
            {
                mtDate = string.Empty;
            }
                       
            if (documentRevision.Status != 2)
            {
                #region document not completed

                doc = new XDocument(
                    new XElement("xliff", new XAttribute("version", "1.2"),
                        new XElement("file", new XAttribute("original", documentRevision.DocumentId), new XAttribute("source-language", originalDocument.SourceLanguage), new XAttribute("target-language", originalDocument.TargetLanguage), new XAttribute("datatype", originalDocument.DataType), new XAttribute("category", originalDocument.Category), new XAttribute("product-name", originalDocument.ProductName),
                            new XElement("header", new XElement("phase-group", new XElement("phase", new XAttribute("phase-name", "mt_baseline"), new XAttribute("process-name", mtBaseLineProcessName), new XAttribute("tool", originalDocument.MtTool), new XAttribute("toold-id", originalDocument.MtToolId), new XAttribute("date", mtDate),  new XAttribute("contact-email", mtContactEmail)) /*ADD MT_BASELINE PHASE HERE*/,/*ADD START_PE PHASE HERE*/
                                new XElement("phase", new XAttribute("phase-name", "start_pe"), new XAttribute("process-name", processName), new XAttribute("tool", startPeTool), new XAttribute("toold-id", startPeToolId), new XAttribute("date", documentRevision.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", documentRevision.UserIdentifier)), documentRevision.TranslationUnits.Select(x => x.Phases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("process-name", y.ProcessName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")),  new XAttribute("contact-email", y.ContactEmail),
                                y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))                                )
                                ))
                                ,//think phases.
                                       documentRevision.TranslationUnits.Select(x => x.ThinkPhases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName),new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      )))
                                       )//closing count group.                               
                                ,//close phase group.                                                                                                                                                                 
                                documentRevision.TranslationUnits.Select(a => new XElement("count-group", new XAttribute("name", a.SegmentIndex), /*revision phase counts*/a.Phases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                                    //think phase counts.
                                    ,a.ThinkPhases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                                    )//close count-group.
                                    )

                            ),//close header.

                            new XElement("body",
                            documentRevision.TranslationUnits.Select(a => new XElement("trans-unit", new XAttribute("id", a.SegmentIndex), new XElement("source", a.Source), new XElement("target", a.Target,/* new XAttribute("state", a.State),*/ new XAttribute("phase-name", a.PhaseName)), a.AlternativeTranslations.Select(d => new XElement("alt-trans", new XAttribute("phase-name", d.PhaseName), new XElement("target", d.Target))))

                            )//close translation unit.
                                )

                    )//close file
                )//close XLIFF.
                );//close XDocument.

                #endregion
            }
            else
            {
                //sum of editing time for all revisions.
                double totalEditingTime = GetEditingDate(documentRevision.TranslationUnits.ToList<TranslationRevision>());
                //get project stuff.                
                if (documentRevision.QuestionsReplied != null && documentRevision.QuestionsReplied.ToArray<QuestionReply>().Length > 0)
                {
                    #region if project has questions
                    //document is finhished, so we need to add the complete phase groups.
                    doc = new XDocument(
                          new XElement("xliff", new XAttribute("version", "1.2"),
                              new XElement("file", new XAttribute("original", documentRevision.DocumentId), new XAttribute("source-language", originalDocument.SourceLanguage), new XAttribute("target-language", originalDocument.TargetLanguage), new XAttribute("datatype", originalDocument.DataType), new XAttribute("category", originalDocument.Category), new XAttribute("product-name", originalDocument.ProductName),
                                  new XElement("header", new XElement("phase-group", new XElement("phase", new XAttribute("phase-name", "mt_baseline"), new XAttribute("process-name", mtBaseLineProcessName), new XAttribute("tool", originalDocument.MtTool), new XAttribute("toold-id", originalDocument.MtToolId), new XAttribute("date", mtDate),new XAttribute("contact-email", mtContactEmail)) /*ADD MT_BASELINE PHASE HERE*/,
                                    /*ADD START_PE PHASE HERE*/
                                    new XElement("phase", new XAttribute("phase-name", "start_pe"), new XAttribute("process-name", processName), new XAttribute("tool", startPeTool), new XAttribute("toold-id", startPeToolId), new XAttribute("date", documentRevision.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", documentRevision.UserIdentifier))
                                      , documentRevision.TranslationUnits.Select(x => x.Phases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      ))),//think phases.
                                       documentRevision.TranslationUnits.Select(x => x.ThinkPhases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      )))
                                      
                                      /*complete_pe phase*/, new XElement("phase", new XAttribute("phase-name", "complete_pe"), new XAttribute("date", documentRevision.CompleteDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", documentRevision.UserIdentifier)) 
                                      /*complete_pe note*/, new XElement("note", new XAttribute("annotates", "general"), documentRevision.QuestionsReplied.ToArray<QuestionReply>()[0].ReplyText)),//close phase group 
                                      documentRevision.TranslationUnits.Select(a => new XElement("count-group", new XAttribute("name", a.SegmentIndex), /*revision phase counts*/a.Phases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                                          /*think phases counts*/
                                    ,a.ThinkPhases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))

                                          )//close count-group.
                                          ), new XElement("count-group", new XAttribute("name", "total"), new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-total-time"), new XAttribute("unit", "x-seconds"), CoreUtils.GetDifferenceInSeconds(documentRevision.DateCreated.ToUniversalTime(), documentRevision.CompleteDate.ToUniversalTime()).ToString())
                                             , new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-editing-time"), new XAttribute("unit", "x-seconds"), totalEditingTime)

                                              )

                                  ),//close header.

                                  new XElement("body",
                                  documentRevision.TranslationUnits.Select(a => new XElement("trans-unit", new XAttribute("id", a.SegmentIndex), new XElement("source", a.Source), new XElement("target", a.Target,  new XAttribute("phase-name", a.PhaseName)), a.AlternativeTranslations.Select(d => new XElement("alt-trans", new XAttribute("phase-name", d.PhaseName), new XElement("target", d.Target))))

                                  )//close translation unit.
                                      )

                          )//close file.
                      )//close XLIFF
                      );//close XDocument

                    #endregion
                }
                else
                {
                    #region without Questions

                    //document is finished, so there is the need to add the complete phase groups.
                    doc =new XDocument(
                          new XElement("xliff", new XAttribute("version", "1.2"),
                              new XElement("file", new XAttribute("original", documentRevision.DocumentId), new XAttribute("source-language", originalDocument.SourceLanguage), new XAttribute("target-language", originalDocument.TargetLanguage), new XAttribute("datatype", originalDocument.DataType), new XAttribute("category", originalDocument.Category), new XAttribute("product-name", originalDocument.ProductName),

                                  new XElement("header", new XElement("phase-group", new XElement("phase", new XAttribute("phase-name", "mt_baseline"), new XAttribute("process-name", mtBaseLineProcessName), new XAttribute("tool", originalDocument.MtTool), new XAttribute("toold-id", originalDocument.MtToolId), new XAttribute("date", mtDate), new XAttribute("contact-email", mtContactEmail)) /*ADD MT_BASELINE PHASE HERE*/,
                        /*ADD START_PE PHASE HERE*/

                                    new XElement("phase", new XAttribute("phase-name", "start_pe"), new XAttribute("process-name", processName), new XAttribute("tool", startPeTool), new XAttribute("toold-id", startPeToolId), new XAttribute("date", documentRevision.DateCreated.ToUniversalTime()),  new XAttribute("contact-email", documentRevision.UserIdentifier))
                                      , documentRevision.TranslationUnits.Select(x => x.Phases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      ))),//think phases.
                                       documentRevision.TranslationUnits.Select(x => x.ThinkPhases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      )))
                                      /*complete_pe phase*/, new XElement("phase", new XAttribute("phase-name", "complete_pe"), new XAttribute("date", documentRevision.CompleteDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", documentRevision.UserIdentifier))),//close phase group                                                                                                                                                                 
                                      documentRevision.TranslationUnits.Select(a => new XElement("count-group", new XAttribute("name", a.SegmentIndex), /*revision phase counts*/a.Phases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                                          /*think phase counts*/
                                    , a.ThinkPhases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                                          )//close count-group.
                                          ), new XElement("count-group", new XAttribute("name", "total"), new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-total-time"), new XAttribute("unit", "x-seconds"), CoreUtils.GetDifferenceInSeconds(documentRevision.DateCreated.ToUniversalTime(), documentRevision.CompleteDate.ToUniversalTime()).ToString()),
                                              new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-editing-time"), new XAttribute("unit", "x-seconds"), totalEditingTime))
                                  ),//close header.
                                  new XElement("body",
                                  documentRevision.TranslationUnits.Select(a => new XElement("trans-unit", new XAttribute("id", a.SegmentIndex), new XElement("source", a.Source), new XElement("target", a.Target,  new XAttribute("phase-name", a.PhaseName)), a.AlternativeTranslations.Select(d => new XElement("alt-trans", new XAttribute("phase-name", d.PhaseName), new XElement("target", d.Target))))
                                  )//close translation unit.
                                      )
                          )//close file.
                      )//close XLIFF.
                      );//close XDocument.
                    #endregion
                }

            }

            return doc.ToString();
        }

        private XElement GetSingleXliffFileNode(Document original, DocumentRevision documentRev, bool showUserInfo, string startPeTool, string startPeToolId, string startPeProcessName)
        {
            Document originalDocument = original;
            DocumentRevision documentRevision = documentRev;            
            XElement xliffFileElement;
                
            string processName = string.Empty;
            processName = originalDocument.Project.InterfaceConfiguration == 1 ?  processName = "bilingual" : processName = "monolingual";

            string mtDate;
            DateTime dateUTC;

            try
            {
                dateUTC = DateTime.Parse(originalDocument.MtDate != null ? mtDate = originalDocument.MtDate : mtDate = string.Empty);
                mtDate = dateUTC.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
            catch
            {
                mtDate = string.Empty;
            }
                                    
            string mtContactEmail; mtContactEmail = originalDocument.MtContactEmail != null ? mtContactEmail = originalDocument.MtContactEmail : mtDate = string.Empty;
            string mtBaseLineProcessName = startPeProcessName;
            string documentRevisionUser = documentRevision.UserIdentifier;

            if (!showUserInfo)
            {
                mtContactEmail = Utils.StringUtils.GenerateTinyHash(mtContactEmail);
                documentRevisionUser = Utils.StringUtils.GenerateTinyHash(documentRevisionUser); 
            }
           
            if (documentRevision.Status != 2)
            {
                #region document not completed
                xliffFileElement = new XElement("file", new XAttribute("original", documentRevision.DocumentId), new XAttribute("source-language", originalDocument.SourceLanguage), new XAttribute("target-language", originalDocument.TargetLanguage), new XAttribute("datatype", originalDocument.DataType), new XAttribute("category", originalDocument.Category), new XAttribute("product-name", originalDocument.ProductName),
                       new XElement("header", new XElement("phase-group", new XElement("phase", new XAttribute("phase-name", "mt_baseline"),
                           new XAttribute("process-name", mtBaseLineProcessName), new XAttribute("tool", originalDocument.MtTool),
                           new XAttribute("toold-id", originalDocument.MtToolId), new XAttribute("date", mtDate),  new XAttribute("contact-email", mtContactEmail)) /*ADD MT_BASELINE PHASE HERE*/,/*ADD START_PE PHASE HERE*/
                           new XElement("phase", new XAttribute("phase-name", "start_pe"), new XAttribute("process-name", processName), new XAttribute("tool", startPeTool), new XAttribute("toold-id", startPeToolId), new XAttribute("date", documentRevision.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")),  new XAttribute("contact-email", documentRevisionUser)), documentRevision.TranslationUnits.Select(x => x.Phases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("process-name", y.ProcessName),
                                new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")),  new XAttribute("contact-email", showUserInfo ? y.ContactEmail : Utils.StringUtils.GenerateTinyHash(y.ContactEmail)),
                           y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                           )
                           ))
                           ,//think phases.
                                       documentRevision.TranslationUnits.Select(x => x.ThinkPhases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      )))                                                                                            
                           )//new close phase group.  
                           ,                                                                                                                                                               
                           documentRevision.TranslationUnits.Select(a => new XElement("count-group", new XAttribute("name", a.SegmentIndex), /*revision phase counts*/a.Phases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                               /*think phases counts*/
                                    , a.ThinkPhases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                               )//close count-group.
                               )

                       ),//close header.

                       new XElement("body",
                       documentRevision.TranslationUnits.Select(a => new XElement("trans-unit", new XAttribute("id", a.SegmentIndex), new XElement("source", a.Source), new XElement("target", a.Target,/* new XAttribute("state", a.State),*/ new XAttribute("phase-name", a.PhaseName)), a.AlternativeTranslations.Select(d => new XElement("alt-trans", new XAttribute("phase-name", d.PhaseName), new XElement("target", d.Target))))

                       )//close translation unit.
                           )

               );//close file.
                #endregion
            }
            else
            {
                //sum of editing time for all  revisions.
                double totalEditingTime = GetEditingDate(documentRevision.TranslationUnits.ToList<TranslationRevision>());
                //get project stuff                
                if (documentRevision.QuestionsReplied != null && documentRevision.QuestionsReplied.ToArray<QuestionReply>().Length > 0)
                {
                    #region if project has questions

                    //document is finished, so there is the need to add the complete phase groups.                  
                    xliffFileElement = new XElement("file", new XAttribute("original", documentRevision.DocumentId), new XAttribute("source-language", originalDocument.SourceLanguage), new XAttribute("target-language", originalDocument.TargetLanguage), new XAttribute("datatype", originalDocument.DataType), new XAttribute("category", originalDocument.Category), new XAttribute("product-name", originalDocument.ProductName),

                            new XElement("header", new XElement("phase-group", new XElement("phase", new XAttribute("phase-name", "mt_baseline"), new XAttribute("process-name", mtBaseLineProcessName), new XAttribute("tool", originalDocument.MtTool), new XAttribute("toold-id", originalDocument.MtToolId), new XAttribute("date", mtDate),/* new XAttribute("job-id", "mt_job_id"),*/ new XAttribute("contact-email", mtContactEmail)) /*ADD MT_BASELINE PHASE HERE*/,
                        /*ADD START_PE PHASE HERE*/

                              new XElement("phase", new XAttribute("phase-name", "start_pe"), new XAttribute("process-name", processName), new XAttribute("tool", startPeTool), new XAttribute("toold-id", startPeToolId), new XAttribute("date", documentRevision.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), /*new XAttribute("job-id", "start_pe_job_id"), */new XAttribute("contact-email", documentRevisionUser))
                                , documentRevision.TranslationUnits.Select(x => x.Phases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", showUserInfo ? y.ContactEmail : Utils.StringUtils.GenerateTinyHash(y.ContactEmail)),
                                y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note)))))
                                ,//think phases
                                       documentRevision.TranslationUnits.Select(x => x.ThinkPhases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      )))
                                /*complete_pe phase*/, new XElement("phase", new XAttribute("phase-name", "complete_pe")/*,new XAttribute("process-name", "complete_pe_process_name"), new XAttribute("tool", "complete_pe_tool"), new XAttribute("toold-id", "complete_pe_tool_Id")*/, new XAttribute("date", documentRevision.CompleteDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", documentRevisionUser)) /*complete_pe note*/, new XElement("note", new XAttribute("annotates", "general"), documentRevision.QuestionsReplied.ToArray<QuestionReply>()[0].ReplyText)),//close phase group                                                                                                                                                                 
                                documentRevision.TranslationUnits.Select(a => new XElement("count-group", new XAttribute("name", a.SegmentIndex), /*revision phase counts*/a.Phases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                                    /*think phases counts*/
                                    , a.ThinkPhases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))

                                    )//close count-group.
                                    ), new XElement("count-group", new XAttribute("name", "total"), new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-total-time"), new XAttribute("unit", "x-seconds"), CoreUtils.GetDifferenceInSeconds(documentRevision.DateCreated.ToUniversalTime(), documentRevision.CompleteDate.ToUniversalTime()).ToString())
                                       , new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-editing-time"), new XAttribute("unit", "x-seconds"), totalEditingTime)

                                        )

                            ),//close header.

                            new XElement("body",
                            documentRevision.TranslationUnits.Select(a => new XElement("trans-unit", new XAttribute("id", a.SegmentIndex), new XElement("source", a.Source), new XElement("target", a.Target,  new XAttribute("phase-name", a.PhaseName)), a.AlternativeTranslations.Select(d => new XElement("alt-trans", new XAttribute("phase-name", d.PhaseName), new XElement("target", d.Target))))

                            )//close translation unit.
                                )

                    );//close file.

                    #endregion
                }
                else
                {
                    #region without Questions

                    //document is finished, so there is the need to add the complete phase groups.
                    xliffFileElement = new XElement("file", new XAttribute("original", documentRevision.DocumentId), new XAttribute("source-language", originalDocument.SourceLanguage), new XAttribute("target-language", originalDocument.TargetLanguage), new XAttribute("datatype", originalDocument.DataType), new XAttribute("category", originalDocument.Category), new XAttribute("product-name", originalDocument.ProductName),

                           new XElement("header", new XElement("phase-group", new XElement("phase", new XAttribute("phase-name", "mt_baseline"), new XAttribute("process-name", mtBaseLineProcessName), new XAttribute("tool", originalDocument.MtTool), new XAttribute("toold-id", originalDocument.MtToolId), new XAttribute("date", mtDate), new XAttribute("contact-email", mtContactEmail)) /*ADD MT_BASELINE PHASE HERE*/,
                        /*ADD START_PE PHASE HERE*/

                             new XElement("phase", new XAttribute("phase-name", "start_pe"), new XAttribute("process-name", processName), new XAttribute("tool", startPeTool), new XAttribute("toold-id", startPeToolId), new XAttribute("date", documentRevision.DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")),  new XAttribute("contact-email", documentRevisionUser))
                               , documentRevision.TranslationUnits.Select(x => x.Phases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", showUserInfo ? y.ContactEmail : Utils.StringUtils.GenerateTinyHash(y.ContactEmail)),
                               y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note)))))
                               ,//think phases.
                                       documentRevision.TranslationUnits.Select(x => x.ThinkPhases.Select(y => new XElement("phase", new XAttribute("phase-name", y.PhaseName), new XAttribute("date", y.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", y.ContactEmail),
                                      y.Notes.Select(z => new XElement("note", new XAttribute("annotates", z.Annotates), new XAttribute("from", z.NoteFrom), z.Note))
                                      )))                               
                               /*complete_pe phase*/, new XElement("phase", new XAttribute("phase-name", "complete_pe"), new XAttribute("date", documentRevision.CompleteDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), new XAttribute("contact-email", documentRevisionUser))),//close phase group                                                                                                                                                                 
                               documentRevision.TranslationUnits.Select(a => new XElement("count-group", new XAttribute("name", a.SegmentIndex), /*revision phase counts*/ a.Phases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))
                                   /*think phases counts*/
                                    , a.ThinkPhases.Select(b => b.PhaseCountGroup.PhaseCounts.Select(r => new XElement("count", new XAttribute("phase-name", r.PhaseName), new XAttribute("count-type", r.CountType), new XAttribute("unit", r.Unit), r.Value)))


                                   )//close count-group.
                                   ), new XElement("count-group", new XAttribute("name", "total"), new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-total-time"), new XAttribute("unit", "x-seconds"), CoreUtils.GetDifferenceInSeconds(documentRevision.DateCreated.ToUniversalTime(), documentRevision.CompleteDate.ToUniversalTime()).ToString()),

                                       new XElement("count", new XAttribute("phase-name", "complete_pe"), new XAttribute("count-type", "x-editing-time"), new XAttribute("unit", "x-seconds"), totalEditingTime))

                           ),//close header.

                           new XElement("body",
                           documentRevision.TranslationUnits.Select(a => new XElement("trans-unit", new XAttribute("id", a.SegmentIndex), new XElement("source", a.Source), new XElement("target", a.Target,  new XAttribute("phase-name", a.PhaseName)), a.AlternativeTranslations.Select(d => new XElement("alt-trans", new XAttribute("phase-name", d.PhaseName), new XElement("target", d.Target))))

                           )//close translation unit.
                               )

                   );//close file.

                    #endregion
                }

            }

            return xliffFileElement;
        }

        private double GetEditingDate(List<TranslationRevision> revisions)
        {
            double totalSeconds; totalSeconds = 0;

            try
            {
                foreach (TranslationRevision tr in revisions)
                {

                    foreach (Phase p in tr.Phases)
                    {
                        var items = from n in p.PhaseCountGroup.PhaseCounts
                                    where n.CountType == "x-editing-time"
                                    select double.Parse(n.Value);

                        double[] toAdd = items.ToArray<double>();
                        totalSeconds = totalSeconds + toAdd.Sum();

                    }
                }

            }
            catch
            {

            }

            return totalSeconds;
        }

        public string GetProjectOwnerXliff(string projectToken, bool showUserInfo, string startPeTool, string startPeToolId, string startPeProcessName)
        {
            XDocument doc;
            List<DocumentRevision> allDocumentRevisions = new List<DocumentRevision>();
            Project p = ProjectRepository.GetProjectByAdminToken(projectToken);                        
            if (p == null)
                throw new Exception("Project not found");

            p.ProjectDocuments = DocumentRepository.GetAllByProjectId(p).ToList<Document>();

            doc = new XDocument(); XElement firstNode = new XElement("xliff", new XAttribute("version", "1.2"));

            foreach (Document d in p.ProjectDocuments)
            {
                List<DocumentRevision> currentDocumentRevisions = DocumentRevisionRepository.GetAllDocumentRevisionBytexttId(d.text_id).ToList<DocumentRevision>();                
                foreach (DocumentRevision revision in currentDocumentRevisions)
                {
                    firstNode.Add(GetSingleXliffFileNode(d, revision, showUserInfo, startPeTool, startPeToolId, startPeProcessName));
                }
            }

            doc.Add(firstNode);
            return doc.ToString();
        }

        public string GetAllUserRevisionsPerTaskXliff(string token, string textId, string startPeTool, string startPeToolId, string startPeProcessName)
        {

            XDocument doc; doc = new XDocument();
            List<DocumentRevision> allDocumentRevisions = new List<DocumentRevision>();

            Project p = ProjectRepository.GetProjectByAdminToken(token);
            if (p == null)
                throw new Exception("Project not found");

            Document d = DocumentRepository.GetDocumentByTextId(textId);
            if (d == null)
                throw new Exception("Document not found!");

            allDocumentRevisions = DocumentRevisionRepository.GetAllDocumentRevisionBytexttId(textId).ToList<DocumentRevision>();

            doc = new XDocument();
            XElement firstNode = new XElement("xliff", new XAttribute("version", "1.2"));           

            foreach (DocumentRevision revision in allDocumentRevisions)
            {
               firstNode.Add(GetSingleXliffFileNode(d, revision,true,startPeTool,startPeToolId,startPeProcessName));
            }
            
            doc.Add(firstNode);
            return doc.ToString();
        }

        #endregion

    }
}
