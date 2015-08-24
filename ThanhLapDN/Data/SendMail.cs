﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using vpro.functions;
using System.Configuration;
using System.Net.Configuration;

namespace ThanhLapDN.Data
{
    public class SendMail
    {
        private AppketoanDataContext db = new AppketoanDataContext();
        public void SendMail_RecoverPassword(string to, string pass, string name)
        {
            string url = ConfigurationManager.AppSettings["URLWebsite"];
            string nameweb = ConfigurationManager.AppSettings["EmailDisplayName"];
            string strBody = "<html>"
                      + "<P>Xin chào:</P>" + name
                      + "<P>Bạn đang yêu cầu khôi phục mật khẩu của bạn tại <A href=\"" + url + "\"></A>.</P>"
                      + "<P> Mật khẩu : " + pass
                      + "<P>Chúng tôi xin cảm ơn những tình cảm tốt đẹp bạn đã dành cho chúng tôi và chúng tôi cũng luôn cầu chúc những điều tốt đẹp nhất sẽ đến với bạn và Gia đình.</P>"
                      + "<P>P/s : Đừng quên thỉnh thoảng ghé qua <A href=\"" + url + "\">" + nameweb + "</A> để cập nhật tin tức về chúng tôi nhé !</P>"
                      + "</html>";

            SendEmailSMTP("Khôi phục mật khẩu tại " + nameweb + " !", to.ToString().Trim(), "", "", strBody, true, false);
        }
        public void SendMail_ChangePassword(string Email, int OID, string Code_Active)
        {

            string link = System.Configuration.ConfigurationManager.AppSettings["URLWebsite"] + "/en-us/doi-mat-khau.aspx?code=" + Code_Active + "&id=" + OID;
            string strBody = "<html>"
                      + "<P>Xin chào,</P>"
                      + "<P> Để thay đổi mật khẩu vui lòng nhấp vào link sau : <a href=" + link + "> " + link + "</a>"
                      + "<P>Chúng tôi xin cảm ơn những tình cảm tốt đẹp bạn đã dành cho chúng tôi và chúng tôi cũng luôn cầu chúc những điều tốt đẹp nhất sẽ đến với bạn và Gia đình.</P>"
                      + "<P>P/s : Đừng quên thỉnh thoảng ghé qua <A href=\"http://esell.vn\">esell.vn</A> để cập nhật tin tức về chúng tôi nhé !</P>"
                      + "</html>";

            SendEmailSMTP("Khôi phục mật khẩu tại esell.vn !", Email.ToString().Trim(), "", "", strBody, true, false);
        }

        public void Send_Link_ChangePassword(string toAddress, string ccAddress, string bccAddress, string body, bool isHtml, bool isSSL)
        {
            string strBody = "<html>" + body + "</html>";
            SendEmailSMTP("Đổi mật khẩu tại esell.vn !", toAddress.ToString().Trim(), "", "", strBody, true, false);
        }

        public void Send_Mail_Order(string toAddress, string ccAddress, string bccAddress, string body, string strSubject)
        {
            string strBody = "<html>" + body + "</html>";
            SendEmailSMTP(strSubject, toAddress, ccAddress, bccAddress, strBody, true, false);
        }

        public void SendMail_Active_Account(string toAddress, string ccAddress, string bccAddress, string body)
        {
            string strBody = "<html>" + body + "</html>";
            SendEmailSMTP("Xác nhận đăng ký tài khoản thành công !", toAddress, ccAddress, bccAddress, strBody, true, false);
        }

        //public void SendEmail(string strSubject, string toAddress, string ccAddress, string bccAddress, string body, bool isHtml, bool isSSL)
        //{
        //    try
        //    {
        //        using (MailMessage mail = new MailMessage())
        //        {
        //            mail.From = new MailAddress(FormAddress, System.Configuration.ConfigurationManager.AppSettings["EmailDisplayName"]);
        //            mail.To.Add(toAddress);
        //            if (ccAddress != "")
        //            {
        //                mail.CC.Add(ccAddress);
        //            }
        //            if (bccAddress != "")
        //            {
        //                mail.Bcc.Add(bccAddress);
        //            }
        //            mail.Subject = strSubject;

        //            string str = "<html>" + body + "</html>";
        //            mail.Body = str;
        //            mail.IsBodyHtml = isHtml;
        //            SmtpClient client = new SmtpClient();
        //            client.EnableSsl = isSSL;
        //            client.Send(mail);
        //        }
        //    }
        //    catch (SmtpException ex)
        //    {
        //        clsVproErrorHandler.HandlerError(ex);
        //    }
        //}

        public void SendEmailSMTP(string strSubject, string toAddress, string ccAddress, string bccAddress, string body, bool isHtml, bool isSSL)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(Utils.CStrDef(System.Configuration.ConfigurationManager.AppSettings["Email"]), Utils.CStrDef(System.Configuration.ConfigurationManager.AppSettings["EmailDisplayName"]));
                    mail.To.Add(toAddress);
                    if (ccAddress != "")
                    {
                        mail.CC.Add(ccAddress);
                    }
                    if (bccAddress != "")
                    {
                        mail.Bcc.Add(bccAddress);
                    }
                    mail.Subject = strSubject;

                    string str = "<html>" + body + "</html>";
                    mail.Body = str;
                    mail.IsBodyHtml = isHtml;
                    SmtpClient client = new SmtpClient();
                    client.EnableSsl = isSSL;
                    client.Host = Utils.CStrDef(System.Configuration.ConfigurationManager.AppSettings["EmailHost"]);
                    client.Port = Utils.CIntDef(System.Configuration.ConfigurationManager.AppSettings["EmailPort"]);
                    client.Credentials = new System.Net.NetworkCredential(Utils.CStrDef(System.Configuration.ConfigurationManager.AppSettings["Email"]), Utils.CStrDef(System.Configuration.ConfigurationManager.AppSettings["EmailPassword"]));

                    client.Send(mail);
                }
            }
            catch (SmtpException)
            {

            }
        }

        public static string FormAddress
        {
            get
            {
                SmtpSection cfg = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
                return cfg.Network.UserName;
            }
        }


        internal void SendMail_Active_Account(System.Web.UI.HtmlControls.HtmlInputText txtEmail, string p, string p_2, string p_3)
        {
            throw new NotImplementedException();
        }

        public bool SendEmail(int idUserSend, int idUserRecive,int type, string str, string company)
        {
            try
            {
                string stype = "";
                switch (type)
                {
                    case 1: stype = "Hồ sơ thành lập mới"; break;
                    case 2: stype = "Hồ sơ thay đổi"; break;
                    case 3: stype = "Hồ sơ hành chánh"; break;
                }
                var _getUserSend = db.USERs.Where(n => n.USER_ID == idUserSend).ToList();
                var _getUserRecive = db.USERs.Where(n => n.USER_ID == idUserRecive).ToList();

                string _nameSend = _getUserSend[0].USER_NAME;
                string _emailRecive = _getUserRecive[0].USER_EMAIL;
                string _nameRecive = _getUserRecive[0].USER_NAME;
                string _mailBody = string.Empty;
                _mailBody += "<br/>Xin chào: <b>" + _nameRecive + "</b>";
                _mailBody += "<br/>Bạn vừa nhận được yêu cầu xử lý quy trình <strong>" + stype + "</strong> được gửi đến từ <strong>" + _nameSend + "</strong>";
                _mailBody += "<br/>Tên khách hàng: <strong>" + company + "</strong>";
                _mailBody += "<br/>Tiến độ: <strong>" + str + "</strong>";
                _mailBody += "<br/>Xin vui lòng đăng nhập vào website <a href='quanly.ketoanvn.com.vn'>quanly.ketoanvn.com.vn</a> để biết thêm thông tin.<br/><br/>";
                _mailBody += "Ngày gửi <i>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "</i><br/>";
                string _sMailBody = string.Empty;
                _sMailBody += "Đây là email được gửi từ website của " + System.Configuration.ConfigurationManager.AppSettings["EmailDisplayName"] + " <br>" + _mailBody;
                SendEmailSMTP("Thông báo: Tiếp nhận xử lý thành lập doanh nghiệp", _emailRecive, "", "", _sMailBody, true, false);
                return true;
            }
            catch { return false; }
        }
        public bool SendEmailInfoKT(int idUserSend, int idUserRecive, int type, string str)
        {
            try
            {
                string stype = "";
                switch (type)
                {
                    case 1: stype = "Hồ sơ thành lập mới"; break;
                    case 2: stype = "Thay đổi hồ sơ"; break;
                    case 3: stype = "Hồ sơ hành chánh"; break;
                }
                var _getUserSend = db.USERs.Where(n => n.USER_ID == idUserSend).ToList();
                var _getUserRecive = db.USERs.Where(n => n.USER_ID == idUserRecive).ToList();

                string _nameSend = _getUserSend[0].USER_NAME;
                string _emailRecive = _getUserRecive[0].USER_EMAIL;
                string _nameRecive = _getUserRecive[0].USER_NAME;
                string _mailBody = string.Empty;
                _mailBody += "<br/>Xin chào: <b>" + _nameRecive + "</b>";
                _mailBody += "<br/>Bạn vừa nhận được yêu cầu xử lý quy trình <strong>" + stype + "</strong> được gửi đến từ <strong>" + _nameSend + "</strong>";
                _mailBody += "<br/>Tiến độ: <strong>" + str + "</strong>";
                _mailBody += "<br/>Xin vui lòng đăng nhập vào website <a href='quanly.ketoanvn.com.vn'>quanly.ketoanvn.com.vn</a> để biết thêm thông tin.<br/><br/>";
                _mailBody += "Ngày gửi <i>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "</i><br/>";
                string _sMailBody = string.Empty;
                _sMailBody += "Đây là email được gửi từ website của " + System.Configuration.ConfigurationManager.AppSettings["EmailDisplayName"] + " <br>" + _mailBody;
                SendEmailSMTP("Thông báo: Tiếp nhận xử lý thành lập doanh nghiệp", _emailRecive, "", "", _sMailBody, true, false);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}