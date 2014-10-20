using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Reflection;

namespace AcceptApi.Areas.Api.Models.Utils
{
    public static class AcceptApiCoreUtils
    {
        private static string _acrolinxServerUrl;
        private static string _acrolinxServerPort;
        private static string _acrolinxServerIdRestPath;
        private static string _acrolinxServerCapabilitiesRestPath;
        private static string _fullAcrolinxServerIdRestPath;
        private static string _fullAcrolinxServerCapabilitiesRestPath;
        private static string _fullAcrolinxAuthenticationTokenPath;
        private static string _fullAcrolinxSignatureTokenPath;      
        private static string _fullAcrolinxRequestSessionPath;
        private static string _acrolinxRequestSessionBody;
        private static string _acrolinxCodeNumberPath;       
        private static string _acrolinxCodeNumberBody;
        private static string _acrolinxFinalResultPath;
        private static string _acrolinxBeforeFinalResultPath;
        private static string _fullAcrolinxServerLanguagesPath;
        private static string _fullAcrolinxUserSelfRegistrationPath;
        private static string _fullAcrolinxCreateUserPath;
        private static string _acrolinxEmptyJsonResult;
        private static string _acceptPortalEmailFrom;
        private static string _acceptPortalEmailConfirmationAddress;
        private static string _acceptPortalPasswordRecoveryEmailConfirmationAddress;
        private static string _acrolinxUser;       
        private static string _acrolinxUserPassword;
        private static string _acrolinxUserEnable;
        private static string _acceptPortalProjectInvitationAddress;
        private static string _acceptPortalProjectInvitationAddressFrench;
        private static string _acceptPortalProjectInvitationAddressGerman;       
        private static string _acceptPortalTaskCompleteUrl;
        private static string _acceptFrenchToEnglishPostEditDemoProjectToken;
        private static string _acceptEnglishToGermanPostEditDemoProjectToken;
        private static string _acceptEnglishToFrenchPostEditDemoProjectToken;
        private static string _acceptFrenchToEnglishCollaborativePostEditDemoProjectToken;
        private static string _acceptEnglishToGermanCollaborativePostEditDemoProjectToken;
        private static string _acceptEnglishToFrenchCollaborativePostEditDemoProjectToken;
            
        #region consts
        //private const string _acrolinxServerUrlKey = "AcrolinxServerUrl";
        //private const string _acrolinxServerPortKey = "AcrolinxSPort";
        //private const string _acrolinxServerIdRestPathKey = "AcrolinxServerIdRestPath";
        //private const string _acrolinxSeAcrolinxServerCapabilitiesRestPathrverPortKey = "AcrolinxServerCapabilitiesRestPath";
        //private const string _fullAcrolinxServerIdRestPathKey = "FullAcrolinxServerIdRestPath";
        //private const string _fullAcrolinxServerCapabilitiesRestPathKey = "FullAcrolinxServerCapabilitiesRestPath";
        //private const string _fullAcrolinxAuthenticationTokenPathKey = "FullAcrolinxAuthenticationTokenPath";
        //private const string _fullAcrolinxRequestSessionPathKey = "FullAcrolinxRequestSessionPath";
        //private const string _acrolinxRequestSessionBodyKey = "AcrolinxRequestSessionBody";
        //private const string _acrolinxCodeNumberPathKey = "AcrolinxCodeNumberPath";
        //private const string _acrolinxCodeNumberBodyKey = "AcrolinxCodeNumberBody";
        //private const string _acrolinxFinalResultPathKey = "AcrolinxFinalResultPath";
        //private const string _acrolinxBeforeFinalResultPathKey = "AcrolinxBeforeFinalResultPath";
        //private const string _fullAcrolinxSignatureTokenPathKey = "FullAcrolinxSignatureTokenPath";
        //private const string FULLACROLINXSERVERLANGUAGESPATH = "FullAcrolinxServerLanguagesPath";
        //private const string FULLACROLINXUSERSELFREGISTRATIONPATH = "FullAcrolinxUserSelfRegistrationPath";
        //private const string FULLACROLINXCREATEUSERPATH = "FullAcrolinxCreateUserPath";
        #endregion

        #region Paraphrasing Utils

        private static string _paraphrasingEndpoint;

        private static string _paraphrasingDefaultEnglish;       
        private static string _paraphrasingDefaultEnglishMaxResults;
        private static string _paraphrasingDefaultEnglishLanguage;
        private static string _paraphrasingDefaultEnglishSystemId;      
        private static string _paraphrasingDefaultFrench;
        private static string _paraphrasingDefaultFrenchMaxResults;
        private static string _paraphrasingDefaultFrenchLanguage;
        private static string _paraphrasingDefaultFrenchSystemId;
        private static string _interactiveCheckRuleSet;
        private static int _paraphrasingTimeoutPeriod;
        
        #endregion

        #region PostEdit Report Defaults
        private static string _postEditReportsStartPhaseToolName;
        private static string _postEditReportsStartPhaseToolID;
        private static string _postEditReportsStartPhaseProcessName;
        #endregion

        static AcceptApiCoreUtils()
        {
            _acrolinxServerUrl = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_SERVER_URL_KEY];
            _acrolinxServerPort = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_SERVER_PORT_KEY];
            _acrolinxServerIdRestPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINXSERVER_GETSERVERID_PATH_KEY];
            _acrolinxServerCapabilitiesRestPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINXSERVER_GETCAPABILITIES_PATH_KEY];
            _fullAcrolinxServerIdRestPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINXSERVER_GET_SERVERID_FULLPATH_KEY];
            _fullAcrolinxServerCapabilitiesRestPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINXSERVER_GETCAPABILITIES_FULLPATH_KEY];
            _fullAcrolinxAuthenticationTokenPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINXSERVER_AUTHENTICATIONTOKEN_FULLPATH_KEY];
            _fullAcrolinxSignatureTokenPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINXSERVER_SIGNATURETOKEN_FULLPATH_KEY];
            _acrolinxRequestSessionBody = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_REQUESTSESSION_BODY_KEY];
            _fullAcrolinxRequestSessionPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_REQUESTSESSION_FULLPATH_KEY];
            _acrolinxCodeNumberPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_REQUESTCODENUMBER_FULLPATH_KEY];
            _acrolinxCodeNumberBody = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_CODENUMBER_BODYREQUEST_KEY];
            _acrolinxFinalResultPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_FINALRESULT_FULLPATH_KEY];
            _acrolinxBeforeFinalResultPath = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_BEFOREFINALRESULT_FULLPATH_KEY];
            _fullAcrolinxServerLanguagesPath = ConfigurationManager.AppSettings[AcceptApiSettings.FULLACROLINXSERVERLANGUAGESPATH];
            _fullAcrolinxUserSelfRegistrationPath = ConfigurationManager.AppSettings[AcceptApiSettings.FULLACROLINXUSERSELFREGISTRATIONPATH];
            _fullAcrolinxCreateUserPath = ConfigurationManager.AppSettings[AcceptApiSettings.FULLACROLINXCREATEUSERPATH];
            _acrolinxEmptyJsonResult = ConfigurationManager.AppSettings[AcceptApiSettings.ACROLINX_EMPTYJSON_RESPONSE_KEY];
            _acceptPortalEmailFrom = ConfigurationManager.AppSettings[AcceptApiSettings.ACCEPT_PORTAL_REGISTRATION_EMAIL_FROM_ADDRESS_KEY];
            _acceptPortalEmailConfirmationAddress = ConfigurationManager.AppSettings[AcceptApiSettings.ACCEPT_PORTAL_VERIFICATION_EMAIL_URL_KEY];
            _acceptPortalPasswordRecoveryEmailConfirmationAddress = ConfigurationManager.AppSettings[AcceptApiSettings.ACCEPT_PORTAL_VERIFICATION_EMAIL_PASSWORDRECOVERY_URL_KEY];
            _acrolinxUser = ConfigurationManager.AppSettings[AcceptApiSettings.SA_USER];
            _acrolinxUserPassword = ConfigurationManager.AppSettings[AcceptApiSettings.SA_PASSWORD];
            _acrolinxUserEnable = ConfigurationManager.AppSettings[AcceptApiSettings.SA_ENABLE];
            _acceptPortalProjectInvitationAddress = ConfigurationManager.AppSettings[AcceptApiSettings.ACCEPT_PORTAL_PROJECT_INVITATION_URL_KEY];
            _acceptPortalProjectInvitationAddressFrench = ConfigurationManager.AppSettings[AcceptApiSettings.ACCEPT_PORTAL_PROJECT_INVITATION_URL_KEY_FRENCH];
            _acceptPortalProjectInvitationAddressGerman = ConfigurationManager.AppSettings[AcceptApiSettings.ACCEPT_PORTAL_PROJECT_INVITATION_URL_KEY_GERMAN];
            _acceptPortalTaskCompleteUrl = ConfigurationManager.AppSettings[AcceptApiSettings.ACCEPT_PORTAL_TASKCOMPLETED_URL_KEY];

            _acceptEnglishToFrenchPostEditDemoProjectToken = ConfigurationManager.AppSettings[AcceptApiSettings.ENGLISH_TO_FRENCH_POSTEDIT_DEMO_TOKEN];
            _acceptEnglishToGermanPostEditDemoProjectToken = ConfigurationManager.AppSettings[AcceptApiSettings.ENGLISH_TO_GERMAN_POSTEDIT_DEMO_TOKEN];
            _acceptFrenchToEnglishPostEditDemoProjectToken = ConfigurationManager.AppSettings[AcceptApiSettings.FRENCH_TO_ENGLISH_POSTEDIT_DEMO_TOKEN];

            _acceptEnglishToFrenchCollaborativePostEditDemoProjectToken = ConfigurationManager.AppSettings[AcceptApiSettings.ENGLISH_TO_FRENCH_COLLABORATIVE_POSTEDIT_DEMO_TOKEN];
            _acceptEnglishToGermanCollaborativePostEditDemoProjectToken = ConfigurationManager.AppSettings[AcceptApiSettings.ENGLISH_TO_GERMAN_COLLABORATIVE_POSTEDIT_DEMO_TOKEN];
            _acceptFrenchToEnglishCollaborativePostEditDemoProjectToken = ConfigurationManager.AppSettings[AcceptApiSettings.FRENCH_TO_ENGLISH_COLLABORATIVE_POSTEDIT_DEMO_TOKEN];


            _paraphrasingEndpoint = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_ENDPOINT];

            _paraphrasingDefaultEnglish = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_ENGLISH];
            _paraphrasingDefaultEnglishLanguage = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_ENGLISH_LANGUAGE];
            _paraphrasingDefaultEnglishMaxResults = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_ENGLISH_MAXRESULTS];
            _paraphrasingDefaultEnglishSystemId = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_ENGLISH_SYSTEM_ID];

            _paraphrasingDefaultFrench = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_FRENCH];
            _paraphrasingDefaultFrenchLanguage = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_FRENCH_LANGUAGE];
            _paraphrasingDefaultFrenchMaxResults = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_FRENCH_MAXRESULTS];
            _paraphrasingDefaultFrenchSystemId = ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_DEFAULT_FRENCH_SYSTEM_ID];
            _interactiveCheckRuleSet = ConfigurationManager.AppSettings[AcceptApiSettings.INTERACTIVE_CHECK_RULESET];
            _paraphrasingTimeoutPeriod = int.Parse(ConfigurationManager.AppSettings[AcceptApiSettings.PARAPHRASING_TIMEOUT_PERIOD]);

            _postEditReportsStartPhaseToolName = ConfigurationManager.AppSettings[AcceptApiSettings.POSTEDIT_REPORTS_STARTPE_TOOL]; ;
            _postEditReportsStartPhaseToolID = ConfigurationManager.AppSettings[AcceptApiSettings.POSTEDIT_REPORTS_STARTPE_TOOLID]; ;
            _postEditReportsStartPhaseProcessName = ConfigurationManager.AppSettings[AcceptApiSettings.POSTEDIT_REPORTS_STARTPE_PROCESSNAME]; ;
        
        }

        #region Acrolinx Properties

        public static string AcrolinxServerUrl
        {
            get { return _acrolinxServerUrl;  } 
        }

        public static string AcrolinxServerPort
        {
            get { return _acrolinxServerPort; }
        }


        public static string AcrolinxServerIdRestPath
        {
            get { return _acrolinxServerIdRestPath; }
        }


        public static string AcrolinxServerCapabilitiesRestPath
        {
            get { return _acrolinxServerCapabilitiesRestPath; }
        }


        public static string FullAcrolinxServerIdRestPath
        {
            get { return _fullAcrolinxServerIdRestPath; }
        }


        public static string FullAcrolinxServerCapabilitiesRestPath
        {
            get { return _fullAcrolinxServerCapabilitiesRestPath; }
        }


        public static string FullAcrolinxAuthenticationTokenPath
        {
            get { return _fullAcrolinxAuthenticationTokenPath; }
            
        }



        public static string FullAcrolinxRequestSessionPath
        {
            get { return _fullAcrolinxRequestSessionPath; }          
        }

        public static string AcrolinxRequestSessionBody
        {
            get { return _acrolinxRequestSessionBody; }            
        }


        public static string AcrolinxCodeNumberBody
        {
            get { return _acrolinxCodeNumberBody; }            
        }


        public static string AcrolinxCodeNumberPath
        {
            get { return _acrolinxCodeNumberPath; }            
        }


        public static string AcrolinxFinalResultPath
        {
            get { return _acrolinxFinalResultPath; }        
        }

        public static string FullAcrolinxSignatureTokenPath
        {
            get { return _fullAcrolinxSignatureTokenPath; }
           
        }


        public static string AcrolinxBeforeFinalResultPath
        {
            get { return _acrolinxBeforeFinalResultPath; }            
        }



        public static string FullAcrolinxServerLanguagesPath
        {
            get { return _fullAcrolinxServerLanguagesPath; }            
        }

        public static string FullAcrolinxUserSelfRegistrationPath
        {
            get { return _fullAcrolinxUserSelfRegistrationPath; }           
        }


        public static string FullAcrolinxCreateUserPath
        {
            get { return _fullAcrolinxCreateUserPath; }           
        }

        public static string AcrolinxEmptyJsonResult
        {
            get { return _acrolinxEmptyJsonResult; }

        }
        

        #endregion

        #region Authentication Properties

        public static string AcceptPortalEmailFrom
        {
            get { return AcceptApiCoreUtils._acceptPortalEmailFrom; }
        }

        public static string AcceptPortalEmailConfirmationAddress
        {
            get { return AcceptApiCoreUtils._acceptPortalEmailConfirmationAddress; }
        }

        public static string AcceptPortalPasswordRecoveryEmailConfirmationAddress
        {
            get { return AcceptApiCoreUtils._acceptPortalPasswordRecoveryEmailConfirmationAddress; }
        }

        public static string AcrolinxUserPassword
        {
            get { return AcceptApiCoreUtils._acrolinxUserPassword; }
        }

        public static string AcrolinxUserEnable
        {
            get { return AcceptApiCoreUtils._acrolinxUserEnable; }
        }

        public static string AcrolinxUser
        {
            get { return AcceptApiCoreUtils._acrolinxUser; }
        }


        public static string AcceptPortalProjectInvitationAddress
        {
            get { return AcceptApiCoreUtils._acceptPortalProjectInvitationAddress; }

        }

        public static string AcceptPortalProjectInvitationAddressFrench
        {
            get { return AcceptApiCoreUtils._acceptPortalProjectInvitationAddressFrench; }

        }

        public static string AcceptPortalProjectInvitationAddressGerman
        {
            get { return AcceptApiCoreUtils._acceptPortalProjectInvitationAddressGerman; }

        }

        public static string AcceptPortalTaskCompleteUrl
        {
            get { return AcceptApiCoreUtils._acceptPortalTaskCompleteUrl; }
        }
        #endregion

        #region PostEditDemos Properties

        public static string AcceptEnglishToFrenchPostEditDemoProjectToken{get { return AcceptApiCoreUtils._acceptEnglishToFrenchPostEditDemoProjectToken;}}
        public static string AcceptEnglishToGermanPostEditDemoProjectToken{get { return AcceptApiCoreUtils._acceptEnglishToGermanPostEditDemoProjectToken; }}
        public static string AcceptFrenchToEnglishPostEditDemoProjectToken{get { return AcceptApiCoreUtils._acceptFrenchToEnglishPostEditDemoProjectToken; }}
        public static string AcceptFrenchToEnglishCollaborativePostEditDemoProjectToken { get { return AcceptApiCoreUtils._acceptFrenchToEnglishCollaborativePostEditDemoProjectToken; } }
        public static string AcceptEnglishToGermanCollaborativePostEditDemoProjectToken{get { return AcceptApiCoreUtils._acceptEnglishToGermanCollaborativePostEditDemoProjectToken; }}
        public static string AcceptEnglishToFrenchCollaborativePostEditDemoProjectToken{get { return AcceptApiCoreUtils._acceptEnglishToFrenchCollaborativePostEditDemoProjectToken; }}
       
        #endregion

        #region Paraphrasing Utils
        public static string ParaphrasingEndpoint { get { return AcceptApiCoreUtils._paraphrasingEndpoint; } }
       
        public static string ParaphrasingDefaultEnglish { get { return AcceptApiCoreUtils._paraphrasingDefaultEnglish; } }
        public static string ParaphrasingDefaultEnglishMaxResults { get { return AcceptApiCoreUtils._paraphrasingDefaultEnglishMaxResults; } }
        public static string ParaphrasingDefaultEnglishLanguage { get { return AcceptApiCoreUtils._paraphrasingDefaultEnglishLanguage; } }
        public static string ParaphrasingDefaultEnglishSystemId { get { return AcceptApiCoreUtils._paraphrasingDefaultEnglishSystemId; } }

        public static string ParaphrasingDefaultFrench { get { return AcceptApiCoreUtils._paraphrasingDefaultFrench; } }
        public static string ParaphrasingDefaultFrenchMaxResults { get { return AcceptApiCoreUtils._paraphrasingDefaultFrenchMaxResults; } }
        public static string ParaphrasingDefaultFrenchLanguage { get { return AcceptApiCoreUtils._paraphrasingDefaultFrenchLanguage; } }
        public static string ParaphrasingDefaultFrenchSystemId { get { return AcceptApiCoreUtils._paraphrasingDefaultFrenchSystemId; } }
        public static string InteractiveCheckRuleSet { get { return AcceptApiCoreUtils._interactiveCheckRuleSet; } }

        public static int ParaphrasingTimeoutPeriod { get { return AcceptApiCoreUtils._paraphrasingTimeoutPeriod; } }
        #endregion

        #region PostEdit Report Defaults
        public static string PostEditReportsStartPhaseToolName { get { return AcceptApiCoreUtils._postEditReportsStartPhaseToolName; } }
        public static string PostEditReportsStartPhaseToolID { get { return AcceptApiCoreUtils._postEditReportsStartPhaseToolID; } }
        public static string PostEditReportsStartPhaseProcessName { get { return AcceptApiCoreUtils._postEditReportsStartPhaseProcessName; } }
        #endregion

        #region UTILS

        public static string DecodeBase64(string encodedString)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encodedString);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                try
                {
                    Byte[] byteData = Convert.FromBase64String(encodedString);
                    string stringData = Encoding.ASCII.GetString(byteData);
                }
                catch
                { 
                }

                return string.Empty;
            }
            
        }
        
        public static IEnumerable<string> ToCsv(string separator, IEnumerable<object> objectlist, Type type)
        {
            if (objectlist.Any())
            {              
                FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);                
                FieldInfo[] fields3 = GetAllFields(type).ToArray();
                foreach (var o in objectlist)
                {              
                    yield return string.Join(separator, fields.Select(f => (f.GetValue(o) ?? "").ToString()).ToArray());                
                }
            }
        }

        public static IEnumerable<FieldInfo> GetAllFields(Type t)
        {
            if (t == null)
                return Enumerable.Empty<FieldInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
        }
        
        #endregion

    }
}