using NssCorporateUmbraco.Code.Email.Models;
using umbraco;

namespace NssCorporateUmbraco.Code.Email.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly TemplateService templateService;

        public EmailService() : this(new TemplateService()) { }

        public EmailService(TemplateService templateService)
        {
            this.templateService = templateService;
        }
        public void Send<T>(T model, string templateLocation, EmailSettings settings)
        {
            var message = templateService.Build(model, templateLocation);
            library.SendMail(settings.From, settings.To, settings.Subject, message, true);
        }
    }
}