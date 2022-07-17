﻿using System.Net.Mail;

namespace FrontToBack.Helper
{
    public class EmailHelper
    {
        private readonly string _email;
        private readonly string _password;

        public EmailHelper(string email,string password)
        {
            _password = password;
            _email = email;
        }

        public bool SendEmail(string UserEmail, string ConfirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_email);
            mailMessage.To.Add(new MailAddress(UserEmail));

            mailMessage.Subject = "Confirm Email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = ConfirmationLink;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_email, _password);
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Port = 587;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (System.Exception)
            {

                
            }
            return false;
        }
    }
}
