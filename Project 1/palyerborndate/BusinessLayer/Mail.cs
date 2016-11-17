using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace BusinessLayer
{
    public class Mail
    {
        public void SendMail(string body, string subject, bool Isattachment, bool Exception, int ErrorTypeID)
        {
            DB _db = new DB();
            try
            {
                string MailServerDNS = "";
                string MailServerUserName = "";
                string MailServerPassword = "";
                int MailServerTCPPort = 0;
                bool MailServerRequireSSL = false;
                string MailsendTO = "";
                #region GetDetailsOfHost
                SqlDataReader _Reader = _db.GetDR("SELECT * FROM EmailCreditionals");
                if (_Reader.HasRows)
                {
                    while (_Reader.Read())
                    {
                        try
                        {
                            MailServerDNS = _Reader["Mail Server DNS"].ToString();
                            MailServerUserName = _Reader["Mail Server Username"].ToString();
                            MailServerPassword = _Reader["Mail Server Password"].ToString();
                            MailServerTCPPort = Convert.ToInt32(_Reader["Mail Server TCP Port"]);
                            MailServerRequireSSL = Convert.ToBoolean(_Reader["Mail Server Requires SSL"]);
                            MailsendTO = _Reader["Mail Send To"].ToString();
                        }
                        catch { }
                    }
                    _Reader.Close();
                }
                try { _Reader.Close(); }
                catch { }
                #endregion GetDetailsOfHost
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                MailAddress Fromaddress = new MailAddress(MailServerUserName, "Vishal Consultancy");
                message.From = Fromaddress;
                message.Subject = subject;
                message.To.Add(new MailAddress(MailServerUserName));
                message.Body = body;
                message.IsBodyHtml = true;
                System.Net.Mail.SmtpClient mclient = new System.Net.Mail.SmtpClient();
                mclient.Host = MailServerDNS;
                mclient.Port = MailServerTCPPort;
                mclient.EnableSsl = MailServerRequireSSL;

                try
                {
                    string[] mails = MailsendTO.Split(',');
                    MailsendTO = "";
                    foreach (string mail in mails)
                    {
                        if (mail.Length > 0)
                        {
                            try
                            {
                                message.CC.Add(mail);
                                MailsendTO = MailsendTO + "," + mail;
                            }
                            catch { }
                        }
                    }
                }
                catch
                {
                }

                mclient.Credentials = new System.Net.NetworkCredential(MailServerUserName, MailServerPassword);
                mclient.Send(message);
                #region InsertErrorEmails
                _db.ExecuteCommand("insert into ErrorEmails(ErrorDescription,ErrorTypeID,SendTo,CreatedOn) values('" + body + "'," + ErrorTypeID + ",'" + MailsendTO + "','" + DateTime.Now + "')");
                #endregion InsertErrorEmails
            }
            catch
            {
                #region InsertErrorEmails
                _db.ExecuteCommand("insert into ErrorEmails(ErrorDescription,ErrorTypeID,SendTo,CreatedOn) values('" + body + "'," + ErrorTypeID + ",'','" + DateTime.Now + "')");
                #endregion InsertErrorEmails
            }

        }

        public void SendMailWithAttchment(string body, string subject, bool Isattachment, bool Exception, int ErrorTypeID, string Attachment,string Email)
        {
            DB _db = new DB();
            try
            {
                string MailServerDNS = "";
                string MailServerUserName = "";
                string MailServerPassword = "";
                int MailServerTCPPort = 0;
                bool MailServerRequireSSL = false;
                string MailsendTO = "";
                #region GetDetailsOfHost
                SqlDataReader _Reader = _db.GetDR("SELECT * FROM EmailCreditionals");
                if (_Reader.HasRows)
                {
                    while (_Reader.Read())
                    {
                        try
                        {
                            MailServerDNS = _Reader["Mail Server DNS"].ToString();
                            MailServerUserName = _Reader["Mail Server Username"].ToString();
                            MailServerPassword = _Reader["Mail Server Password"].ToString();
                            MailServerTCPPort = Convert.ToInt32(_Reader["Mail Server TCP Port"]);
                            MailServerRequireSSL = Convert.ToBoolean(_Reader["Mail Server Requires SSL"]);
                            MailsendTO = Email;
                        }
                        catch { }
                    }
                    _Reader.Close();
                }
                try { _Reader.Close(); }
                catch { }
                #endregion GetDetailsOfHost
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                MailAddress Fromaddress = new MailAddress(MailServerUserName, "Vishal Consultancy");
                message.From = Fromaddress;
                message.Subject = subject;
                message.To.Add(new MailAddress(MailServerUserName));
                message.Body = body;
                message.IsBodyHtml = true;
                if (Attachment != "")
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(Attachment);
                    message.Attachments.Add(attachment);
                }

                System.Net.Mail.SmtpClient mclient = new System.Net.Mail.SmtpClient();
                mclient.Host = MailServerDNS;
                mclient.Port = MailServerTCPPort;
                mclient.EnableSsl = MailServerRequireSSL;
                try
                {
                    string[] mails = MailsendTO.Split(',');
                    MailsendTO = "";
                    foreach (string mail in mails)
                    {
                        if (mail.Length > 0)
                        {
                            try
                            {
                                message.CC.Add(mail);
                                MailsendTO = MailsendTO + "," + mail;
                            }
                            catch { }
                        }
                    }
                }
                catch
                {
                }

                mclient.Credentials = new System.Net.NetworkCredential(MailServerUserName, MailServerPassword);
                mclient.Send(message);
            }
            catch
            {
                #region InsertErrorEmails
                _db.ExecuteCommand("insert into ErrorEmails(ErrorDescription,ErrorTypeID,SendTo,CreatedOn) values('" + "Issue accured in sending mail of report file to Email: "+Email + "'," + ErrorTypeID + ",'','" + DateTime.Now + "')");
                #endregion InsertErrorEmails
            }

        }
    }
}
