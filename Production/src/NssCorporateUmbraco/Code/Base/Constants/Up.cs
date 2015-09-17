using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NssCorporateUmbraco.Code.Base.Constants
{
    public class Up
    {
        public class Content
        {
            public class Common
            {
                public static string Text = "text";
            }
        }

        public class Mixins
        {
            public class StandardContent
            {
                public static string BodyText = "bodyText";
                public static string CallToAction = "callToAction";
                public static string CtaAdditionalText = "ctaAdditionalText";
                public static string Introduction = "introductionText";
                public static string GridBodyText = "gridBodyText";
                public static string PageSummary = "pageSummary";
            }

            public class PageInfo
            {
                public static string PrimaryNavigation = "primaryNavigation";
                public static string FooterNavigation = "footerNavigation";
                public static string UrlOverride = "urlOverride";
                public static string HideInParentLister = "hideInParentLister";
            }

            public class Seo
            {
                public static string TitleTag = "titleTag";
                public static string OgTitle = "ogTitle";
                public static string OgDescription = "ogDescription";
                public static string MetaDescription = "metaDescription";
                public static string H1PageTitle = "h1PageTitle";
                public static string OgImage = "ogImage";
            }

            public class InformationBlocks
            {
                public static string textbox = "docInformationBlockTextbox";
                public static string title = "title";

                public static string contact = "docInformationBlockContact";
                public static string contactFullName = "contactFullName";
                public static string contactJobTitle = "contactJobTitle";
                public static string email = "email";
                public static string phone = "phone";
                public static string mobile = "mobile";
                public static string mobileAdditionalInfo = "mobileAdditionalInfo";
                public static string textRelay = "textRelay";
                public static string textRelayAdditionalInfo = "textRelayAdditionalInfo";

                public static string form = "docInformationBlockForm";
                public static string formDescription = "formDescription";

                public static string ourAims = "docOurAims";
                public static string aim = "aim";
            }

            public class EmailSettings
            {
                public static string RecipientEmailAddress = "recipientEmailAddress";
                public static string SenderEmailAddress = "senderEmailAddress";
                public static string DefaultSubjectField = "defaultSubjectField";
            }
        }
    }
}