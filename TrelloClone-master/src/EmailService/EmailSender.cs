using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = Directory.GetParent(currentDirectory).FullName;
            var filesDirectory = Path.Combine(parentDirectory, "Files");

            var bodyBuilder = new BodyBuilder();
            var image = bodyBuilder.LinkedResources.Add(Path.Combine(filesDirectory, "smartproj_icon.png"));
            image.IsAttachment = false;
            image.ContentId = MimeUtils.GenerateMessageId();

            bodyBuilder.HtmlBody = string.Format(@$"
            <center><div style=""border: 15px solid #EDF4FF; width:700px; border-radius: 16px;"">
		            <div style=""padding:15px; font-family:Tahoma; border-radius: 16px;"">	
                        <span style=\""width:50px; height:50px;\"">
                            <img style=\""width:50px; height:50px;\"" src='cid:{{0}}'>
                        </span>
			            <center>			
				            <div style=""text-align: left; padding: 0px 10px 10px 10px; color: #1B74FD;"">

					            <h3><b>Уважаемый(ая)</b></h3>

					            <h2><b>{message.AddresseeName}</b></h2>

					            <div style=""color:#333;"">
						            <br>
                                        <div class=""MAIN_BLOCKMESSAGE"">
                                            {message.Content}
                                        </div>						            	
						            <br>
					            </div>
				            </div>
			            </center>
		            </div>
	            </div>
            </center>",
            image.ContentId);


            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port);
                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
