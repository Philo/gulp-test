using System;
using System.Text;
using System.Web;
using NssCorporateUmbraco.Code.Base.Extensions;
using NssCorporateUmbraco.Code.Email.Attributes;

namespace NssCorporateUmbraco.Code.Email.Services
{
    public class TemplateService
    {
        public string Build<T>(T model, string templateLocation)
        {
            var tableRows = new StringBuilder();
            
            foreach (var property in model.GetType().GetProperties())
            {
                var ignoreProperty = Attribute.IsDefined(property, typeof(EmailIgnorePropertyAttribute));

                if (!ignoreProperty)
                {
                    tableRows.AppendLine(CreateTableData(ReflectionExtensions.GetPropertyDisplayName(property), Convert.ToString(property.GetValue(model))));
                }
            }

            var message = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(templateLocation));

            return message.Replace("{{TableRows}}", tableRows.ToString());
        }

        private string CreateTableData(string title, string dataValue)
        {
            var rowString = new StringBuilder();

            rowString.AppendLine("<tr>");
            rowString.AppendLine("<td class='table_field'><strong>");
            rowString.AppendLine(title);
            rowString.AppendLine("</strong></td>");
            rowString.AppendLine("<td class='table_data'>");
            rowString.AppendLine(dataValue);
            rowString.AppendLine("</td>");
            rowString.AppendLine("</tr>");

            return rowString.ToString();
        }
    }
}