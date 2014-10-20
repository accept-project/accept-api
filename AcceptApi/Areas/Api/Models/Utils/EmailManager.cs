using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Mail;
using System.Web.Configuration;
using System.IO;

namespace AcceptApi.Areas.Api.Models.Utils
{
    public class EmailManager
    {

        public static void SentTestEmail(string emailFrom, string emailTo)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    using (var message = new MailMessage(emailFrom, emailTo))
                    {
                        message.Subject = "Accept Portal - TEST EMAIL";
                        message.Body = "Awesome stuff it actually works! :)";
                        message.IsBodyHtml = true;                                                
                        client.EnableSsl = true;
                        client.Send(message);
                    };
                };
            }
            catch (Exception e)
            {

                throw (e);
            }
        }
       
        public static void SendConfirmationEmail(string emailFrom, string userName, string confirmationCode, string verificationUrl)
        {                       
            string verifyUrl = verificationUrl + confirmationCode;
            string templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendConfirmationEmailTemplate"]);
            string text = File.ReadAllText(templateFile);
            text = text.Replace("%LinkToRegistration%", verifyUrl);
            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, userName))
                {
                    message.Subject = "Accept Portal - Please Verify your Account";
                    message.Body = text;                  
                    message.IsBodyHtml = true;                    
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };

            #region Notify
            SendNotificationEmail(emailFrom, userName, "New Account Requested", "requested for an ACCEPT Portal account.");
            #endregion
        }

        public static void SendConfirmationEmailForPasswordRecovery(string emailFrom, string userName, string confirmationCode, string verificationUrl)
        {
            string verifyUrl = verificationUrl + confirmationCode;
            string templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendPasswordRecoveryEmailTemplate"]);
            string text = File.ReadAllText(templateFile);
            text = text.Replace("%Link%", verifyUrl);
            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, userName))
                {
                    message.Subject = "Accept Portal - Password Recovery";
                    message.Body = text;                    
                    message.IsBodyHtml = true;                    
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };

            #region Notify
            SendNotificationEmail(emailFrom,userName,"Password Recovery","requested for a password recovery.");
            #endregion
        }

        public static void SendNotificationEmail(string emailFrom, string userName,string subject, string bodyMessage)
        {
            
            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, "davidluzsilva@gmail.com"))
                {
                    message.Subject = "Accept Portal Notification - " + subject;
                    message.Body = "<html><head><meta content=\"text/html; charset=utf-8\" /></head><body>" +
                        "<p>The user: " + userName + "</p>" + "<p>" + bodyMessage + "</p>"
                        +"</body></html>";

                    message.IsBodyHtml = true;
                    //message.Bcc.Add(new MailAddress("someone@someplace.com"));                                              
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };
        
        }

        public static void SendInvitationEmail(string emailFrom, string emailTo, string projectName, string verifyUrl, string language, string extraProjectOwnerNote, string projectOwner)
        {

            string templateFile;templateFile = string.Empty;
            string text; text = string.Empty;                       
            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, emailTo))
                {                  
                    switch (language)
                    {
                        case "fr": 
                        {
                            templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendProjectInvitationEmailTemplate_fr"]);
                            text = File.ReadAllText(templateFile);
                            text = text.Replace("%ProjectName%", projectName);
                            text = text.Replace("%Link%", verifyUrl);
                            message.Subject = "Invitation pour le Portail ACCEPT";                                                  
                            if (extraProjectOwnerNote.Length > 0)
                            {                               
                                text = text.Replace("%MESSAGENOTE%", "La personne qui s’occupe du suivi de ce projet voudrait vous donner le complément d’information suivant:");
                                text = text.Replace("%MESSAGE%", extraProjectOwnerNote);
                            }
                            else
                            {
                                text = text.Replace("%MESSAGENOTE%", "");
                                text = text.Replace("%MESSAGE%", "");
                            }
                            message.Body = text;
                        
                        } break;
                        case "de": {
                            templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendProjectInvitationEmailTemplate_de"]);
                            text = File.ReadAllText(templateFile);
                            text = text.Replace("%ProjectName%", projectName);
                            text = text.Replace("%Link%", verifyUrl);
                            message.Subject = "Einladung zum ACCEPT-Portal";                           
                            if (extraProjectOwnerNote.Length > 0)
                            {                             
                                text = text.Replace("%MESSAGENOTE%", "Wenn Sie dieses Nachbearbeitungsprojekt abgeschlossen haben, werden Sie auf Anfrage Zugang zu diesen Daten haben. Der Projektmanager möchte zusätzlich folgende Informationen mit Ihnen teilen:");
                                text = text.Replace("%MESSAGE%", extraProjectOwnerNote);
                            }
                            else
                            {
                                text = text.Replace("%MESSAGENOTE%", "");
                                text = text.Replace("%MESSAGE%", "");                            
                            }
                            message.Body = text;            
                                                
                        } break;
                        default: {
                        templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendProjectInvitationEmailTemplate"]);
                        text = File.ReadAllText(templateFile);
                        text = text.Replace("%ProjectName%", projectName);
                        text = text.Replace("%Link%", verifyUrl);                                                    
                        message.Subject = "ACCEPT Portal Invitation";                      
                        if (extraProjectOwnerNote.Length > 0)
                        {
                            text = text.Replace("%MESSAGENOTE%", "The person in charge of this project would like to share the additional information with you:");
                            text = text.Replace("%MESSAGE%", extraProjectOwnerNote);
                        }
                        else
                        {
                            text = text.Replace("%MESSAGENOTE%", "");
                            text = text.Replace("%MESSAGE%", "");
                        }
                   
                        message.Body = text;
                        } break;                                        
                    }
                   
                    message.IsBodyHtml = true;
            
                    message.CC.Add(new MailAddress(projectOwner));
                    message.Bcc.Add(new MailAddress("davidluzsilva@gmail.com"));

                    client.EnableSsl = true;

                    client.Send(message);
                };
            };

        }

        public static void SendNotificationEmailToProjectOwnerForExistingUser(string projectName, List<string> emailsFailed, List<string> emailsSent, string emailTo, string emailFrom, string languageCode)
        {

            string templateFile; templateFile = string.Empty;
            string text; text = string.Empty;
            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, emailTo))
                {                                                                       
                    switch (languageCode)
                    {
                        case "fr": 
                        {
                            message.Subject = "Accept Portal Notification - Project Invitation Report";
                            templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["ProjectManagerReportEmailTemplate_fr"]);
                            text = File.ReadAllText(templateFile);
                        
                        }break;
                        case "de": 
                        {
                            message.Subject = "ACCEPT-Portal-Benachrichtigung: Bericht über Projekteinladungen";
                            templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["ProjectManagerReportEmailTemplate_de"]);
                            text = File.ReadAllText(templateFile);                        

                        }break;
                        default:
                        {
                            message.Subject = "Accept Portal Notification - Project Invitation Report";
                            templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["ProjectManagerReportEmailTemplate"]);
                            text = File.ReadAllText(templateFile);
                        
                        }break;                    
                    }

                    text = text.Replace("%ProjectName%", projectName);
                    text = text.Replace("%TOTALRECIPIENTS%", (emailsFailed.Count + emailsSent.Count).ToString());
                    string sentAddresses = string.Empty;
                    string notSentAddresses = string.Empty;

                    foreach (string emailSentAddress in emailsSent)
                        sentAddresses = sentAddresses + "<p>" + emailSentAddress + "<p/>"; // message.Body = message.Body + "<p>" + emailSentAddress + "<p/>";
                                     
                    if (emailsFailed.Count > 0)
                    {                       
                        foreach (string emailNotSentAddress in emailsFailed)
                            notSentAddresses = notSentAddresses + "<p>" + emailNotSentAddress + "<p/>"; // message.Body = message.Body + "<p>" + emailNotSentAddress + "<p/>";
                    }

                    text = text.Replace("%SENTEMAILS%", sentAddresses);
                    text = text.Replace("%NOTSENTEMAILS%", notSentAddresses);
                    message.Body = text;
                    message.IsBodyHtml = true;
                    message.Bcc.Add(new MailAddress("davidluzsilva@gmail.com"));
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };

        }
      
        #region Localization for Portal Registration and Password Recovey
       
        public static void SendConfirmationEmailFrench(string emailFrom, string userName, string confirmationCode, string verificationUrl)
        {          
            string verifyUrl = verificationUrl + confirmationCode;

            string templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendConfirmationEmailTemplate_fr"]);
            string text = File.ReadAllText(templateFile);
            text = text.Replace("%LinkToRegistration%", verifyUrl);

            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, userName))
                {
                    message.Subject = " Portal ACCEPT – Veuillez verifier votre compte.";
                    message.Body = text;                  
                    message.IsBodyHtml = true;                    
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };

            #region Notify

            SendNotificationEmail(emailFrom, userName, "New Account Requested", "requested for an ACCEPT Portal account.");

            #endregion
        }
        public static void SendConfirmationEmailGerman(string emailFrom, string userName, string confirmationCode, string verificationUrl)
        {
            string verifyUrl = verificationUrl + confirmationCode;

            string templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendConfirmationEmailTemplate_de"]);
            string text = File.ReadAllText(templateFile);
            text = text.Replace("%LinkToRegistration%", verifyUrl);

            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, userName))
                {
                    message.Subject = "ACCEPT-Portal – Bitte bestätigen Sie Ihr Konto";
                    message.Body = text;                   
                    message.IsBodyHtml = true;
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };

            #region Notify

            SendNotificationEmail(emailFrom, userName, "New Account Requested", "requested for an ACCEPT Portal account.");

            #endregion
        }
        public static void SendConfirmationEmailForPasswordRecoveryGerman(string emailFrom, string userName, string confirmationCode, string verificationUrl)
        {

            string verifyUrl = verificationUrl + confirmationCode;
            string templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendPasswordRecoveryEmailTemplate_de"]);
            string text = File.ReadAllText(templateFile);
            text = text.Replace("%Link%", verifyUrl);

            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, userName))
                {
                    message.Subject = "ACCEPT-Portal – Passwort wiederherstellen";
                    message.Body = text;
                    message.IsBodyHtml = true;
                    //message.Bcc.Add("davidluzsilva@gmail.com");
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };

            #region Notify

            SendNotificationEmail(emailFrom, userName, "Password Recovery", "requested for a password recovery.");

            #endregion

        }
        public static void SendConfirmationEmailForPasswordRecoveryFrench(string emailFrom, string userName, string confirmationCode, string verificationUrl)
        {

            string verifyUrl = verificationUrl + confirmationCode;
            string templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["SendPasswordRecoveryEmailTemplate_fr"]);
            string text = File.ReadAllText(templateFile);
            text = text.Replace("%Link%", verifyUrl);

            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, userName))
                {
                    message.Subject = "Portal ACCEPT – Récupération du mot de passe";
                    message.Body = text;                   
                    message.IsBodyHtml = true;
                    //message.Bcc.Add("davidluzsilva@gmail.com");
                    client.EnableSsl = true;
                    client.Send(message);
                };
            };

            #region Notify

            SendNotificationEmail(emailFrom, userName, "Password Recovery", "requested for a password recovery.");

            #endregion

        }
        public static void SendTaskCompletedNotification(string emailFrom, string userName, string taskOwner, string linkToTaskDetails, string projectName, string projectLanguage)
        {
            string templateFile;
            string text; text = string.Empty;
            string subject; subject = string.Empty;

            switch (projectLanguage)
            {
                case "French": 
                {
                    templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["TaskCompletedEmailTemplate_fr"]);
                    text = File.ReadAllText(templateFile);
                    subject = "Notification du portail ACCEPT - Tâche terminée";                
                }break;
                case "German": { 
                    templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["TaskCompletedEmailTemplate_de"]);
                    text = File.ReadAllText(templateFile);
                    subject = "ACCEPT-Portal-Benachrichtigung: Aufgabe abgeschlossen";                               
                }break;
                default:
                {
                    templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["TaskCompletedEmailTemplate"]);
                    text = File.ReadAllText(templateFile);
                    subject = "Accept Portal Notification - Task Completed";
                
                } break;
            
            }
            
            text = text.Replace("%ProjectName%", projectName);
            text = text.Replace("%TaskOwner%", taskOwner);
            text = text.Replace("%LinkToTaskDetails%", linkToTaskDetails);

            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(emailFrom, userName))
                {
                    //message.Subject = "Accept Portal Notification - Task Completed";
                    message.Subject = subject;
                    message.Body = text;
                    //message.Body = "<html><head><meta content=\"text/html; charset=utf-8\" /></head><body>" +
                    //    "<p>User " + taskOwner + " completed the following task:</p>"
                    //    + "<p> <a href=\"" + linkToTaskDetails + "\" target=\"_blank\">" + linkToTaskDetails
                    //    + "</a> </p>  <div>Best regards,</div><div>Accept-Portal Team.</div><p>Do not forward "
                    //    + "this email. The verify link is private.</p></body></html>";

                    message.IsBodyHtml = true;
                    message.Bcc.Add("davidluzsilva@gmail.com");
                    client.EnableSsl = true;

                    client.Send(message);
                };
            };


            #region Notify

            SendNotificationEmail(emailFrom, taskOwner, "Task Completed", "User complete a task.");

            #endregion

        }

        #endregion

        public static void SendFeedbackEmail(string from, string[] to, string user, string email, string link, string feedbackMessage, string subject)
        {

            string templateFile = System.Web.HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["FeedbackTemplate"]);
            string text = File.ReadAllText(templateFile);
            text = text.Replace("%USER%", user);
            text = text.Replace("%EMAIL%", email);
            text = text.Replace("%LINK%", link);
            text = text.Replace("%MESSAGEBODY%", feedbackMessage);
            
            
            using (var client = new SmtpClient())
            {
                foreach (string recipient in to)
                {
                    using (var message = new MailMessage(from, recipient))
                    {
                        message.Subject = "Accept Portal - User Feedback:" + subject;
                        message.Body = text;
                        message.IsBodyHtml = true;
                        client.EnableSsl = true;
                        client.Send(message);
                    };
                }
            };         
        }









    }
}