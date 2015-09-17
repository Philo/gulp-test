using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NssCorporateUmbraco.Code.Base;
using NssCorporateUmbraco.Code.Base.Constants;
using NssCorporateUmbraco.Code.Base.Extensions;
using NssCorporateUmbraco.Code.Email.Models;
using NssCorporateUmbraco.Code.Email.Services;
using NssCorporateUmbraco.Code.Email.Services.Impl;
using NssCorporateUmbraco.Code.Forms.Models;
using Umbraco.Web.Mvc;

namespace NssCorporateUmbraco.Controllers
{
    public class FormController : SurfaceController
    {

       private readonly IEmailService emailService;
        //private readonly IUmbracoRepository repository;

        public FormController() : this(new EmailService()) { }

        public FormController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public ActionResult Send(ContactFormModel model)
        {
            const string successMessage = "Your form was successfully submitted";

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            ActionsOnSuccess(model);

            TempData.Add("Message", successMessage);
            return Redirect(RedirectToCurrentUmbracoPage().Url + "#contactanchor");
        }

        private void ActionsOnSuccess(ContactFormModel model)
        {
            var settingsNode = Umbraco.TypedContent(1600);

            var settings = new EmailSettings
            {
                To = settingsNode.Get(Up.Mixins.EmailSettings.RecipientEmailAddress),
                From = settingsNode.Get(Up.Mixins.EmailSettings.SenderEmailAddress),
                Subject = settingsNode.Get(Up.Mixins.EmailSettings.DefaultSubjectField)
            };

            //repository.Save(model);
            emailService.Send(model, Settings.DefaultEmailTemplate, settings);
        }
    }
}