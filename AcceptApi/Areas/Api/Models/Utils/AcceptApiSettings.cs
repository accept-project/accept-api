using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcceptApi.Areas.Api.Models.Utils
{
    public static class AcceptApiSettings
    {
        #region Core Application Settings
        public static readonly string ACCEPT_PORTAL_REGISTRATION_EMAIL_FROM_ADDRESS_KEY = "AcceptPortalEmailFrom";
        public static readonly string ACCEPT_PORTAL_VERIFICATION_EMAIL_URL_KEY = "AcceptPortalRegistrationVerifyUrl";
        public static readonly string ACCEPT_PORTAL_VERIFICATION_EMAIL_PASSWORDRECOVERY_URL_KEY = "AcceptPortalPasswordRecoveryUrl";
        public static readonly string SA_USER = "SaUser";
        public static readonly string SA_PASSWORD = "SaPassword";
        public static readonly string SA_ENABLE = "EnableSa";
        public static readonly string ACCEPT_PORTAL_PROJECT_INVITATION_URL_KEY = "AcceptPortalProjectInvitesUrl";
        public static readonly string ACCEPT_PORTAL_PROJECT_INVITATION_URL_KEY_FRENCH = "AcceptPortalProjectInvitesUrlFrench";
        public static readonly string ACCEPT_PORTAL_PROJECT_INVITATION_URL_KEY_GERMAN = "AcceptPortalProjectInvitesUrlGerman";
        public static readonly string ACCEPT_PORTAL_TASKCOMPLETED_URL_KEY = "AcceptPortalTaskCompletedUrl";
        public static readonly string FRENCH_TO_ENGLISH_POSTEDIT_DEMO_TOKEN = "FrenchToEnglishDemoToken";
        public static readonly string ENGLISH_TO_GERMAN_POSTEDIT_DEMO_TOKEN = "EnglishToGermanDemoToken";
        public static readonly string ENGLISH_TO_FRENCH_POSTEDIT_DEMO_TOKEN = "EnglishToFrenchDemoToken";
        public static readonly string FRENCH_TO_ENGLISH_COLLABORATIVE_POSTEDIT_DEMO_TOKEN = "FrenchToEnglishCollaborativeDemoToken";
        public static readonly string ENGLISH_TO_GERMAN_COLLABORATIVE_POSTEDIT_DEMO_TOKEN = "EnglishToGermanCollaborativeDemoToken";
        public static readonly string ENGLISH_TO_FRENCH_COLLABORATIVE_POSTEDIT_DEMO_TOKEN = "EnglishToFrenchCollaborativeDemoToken";
        #endregion

        #region Acrolinx Settings
        public static readonly string ACROLINX_SERVER_URL_KEY = "AcrolinxServerUrl";
        public static readonly string ACROLINX_SERVER_PORT_KEY = "AcrolinxSPort";
        public static readonly string ACROLINXSERVER_GETSERVERID_PATH_KEY = "AcrolinxServerIdRestPath";
        public static readonly string ACROLINXSERVER_GETCAPABILITIES_PATH_KEY = "AcrolinxServerCapabilitiesRestPath";
        public static readonly string ACROLINXSERVER_GET_SERVERID_FULLPATH_KEY = "FullAcrolinxServerIdRestPath";
        public static readonly string ACROLINXSERVER_GETCAPABILITIES_FULLPATH_KEY = "FullAcrolinxServerCapabilitiesRestPath";
        public static readonly string ACROLINXSERVER_AUTHENTICATIONTOKEN_FULLPATH_KEY = "FullAcrolinxAuthenticationTokenPath";
        public static readonly string ACROLINX_REQUESTSESSION_FULLPATH_KEY = "FullAcrolinxRequestSessionPath";
        public static readonly string ACROLINX_REQUESTSESSION_BODY_KEY = "AcrolinxRequestSessionBody";
        public static readonly string ACROLINX_REQUESTCODENUMBER_FULLPATH_KEY = "AcrolinxCodeNumberPath";
        public static readonly string ACROLINX_CODENUMBER_BODYREQUEST_KEY = "AcrolinxCodeNumberBody";
        public static readonly string ACROLINX_FINALRESULT_FULLPATH_KEY = "AcrolinxFinalResultPath";
        public static readonly string ACROLINX_BEFOREFINALRESULT_FULLPATH_KEY = "AcrolinxBeforeFinalResultPath";
        public static readonly string ACROLINXSERVER_SIGNATURETOKEN_FULLPATH_KEY = "FullAcrolinxSignatureTokenPath";
        public static readonly string FULLACROLINXSERVERLANGUAGESPATH = "FullAcrolinxServerLanguagesPath";
        public static readonly string FULLACROLINXUSERSELFREGISTRATIONPATH = "FullAcrolinxUserSelfRegistrationPath";
        public static readonly string FULLACROLINXCREATEUSERPATH = "FullAcrolinxCreateUserPath";
        public static readonly string ACROLINX_EMPTYJSON_RESPONSE_KEY = "AcrolinxEmptyJson";
        #endregion

        #region Paraphrasing Settings
        public static readonly string PARAPHRASING_ENDPOINT = "ParaphrasingEndPoint";
        public static readonly string PARAPHRASING_DEFAULT_ENGLISH_SYSTEM_ID = "ParaphrasingDefaultEnglishSystemId";
        public static readonly string PARAPHRASING_DEFAULT_ENGLISH_LANGUAGE = "ParaphrasingDefaultEnglishLanguage";
        public static readonly string PARAPHRASING_DEFAULT_ENGLISH_MAXRESULTS = "ParaphrasingDefaultEnglishMaxResults";
        public static readonly string PARAPHRASING_DEFAULT_ENGLISH = "ParaphrasingDefaultEnglish";
        public static readonly string PARAPHRASING_DEFAULT_FRENCH_SYSTEM_ID = "ParaphrasingDefaultFrenchSystemId";
        public static readonly string PARAPHRASING_DEFAULT_FRENCH_LANGUAGE = "ParaphrasingDefaultFrenchLanguage";
        public static readonly string PARAPHRASING_DEFAULT_FRENCH_MAXRESULTS = "ParaphrasingDefaultFrenchMaxResults";
        public static readonly string PARAPHRASING_DEFAULT_FRENCH = "ParaphrasingDefaultFrench";
        public static readonly string INTERACTIVE_CHECK_RULESET = "InteractiveCheckRuleset";
        public static readonly string PARAPHRASING_TIMEOUT_PERIOD = "ParaphrasingTimeout";         
        #endregion

        #region Post-Edit Reports
         public static readonly string POSTEDIT_REPORTS_STARTPE_TOOL = "PostEditReportStartPeTool";
         public static readonly string POSTEDIT_REPORTS_STARTPE_TOOLID = "PostEditReportStartPeToolID";
         public static readonly string POSTEDIT_REPORTS_STARTPE_PROCESSNAME = "PostEditReportStartPeMTBaselineProcessName";     
        #endregion
    }
}