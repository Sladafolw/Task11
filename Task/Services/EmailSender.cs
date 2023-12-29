namespace Practic.Services
{
   
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using MimeKit;


    public class EmailSender : IEmailSender
        {
            private readonly ILogger _logger;
            public AuthMessageSenderOptions Options { get; }


            public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                ILogger<EmailSender> logger)
            {
                Options = optionsAccessor.Value;
                _logger = logger;
            }


            public async Task SendEmailAsync(string email, string subject, string message)
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Администрация сайта", "CommunityPortalService@mail.ru"));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.mail.ru", 25, false);
                    await client.AuthenticateAsync("CommunityPortalService@mail.ru", "j3VLjLwZ5FcRwyunJTDM");
                    var response = await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
        }
    }


