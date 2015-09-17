using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NssCorporateUmbraco.Code.Base.Extensions
{
    public static class ReflectionExtensions
    {
        public static T GetAttribute<T>(this MemberInfo member, bool isRequired) where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                return null;
            }

            return (T)attribute;
        }

        public static string GetPropertyDisplayName(PropertyInfo property)
        {
            if (property == null)
                return string.Empty;
            var attr = property.GetAttribute<DisplayAttribute>(false);
            return attr == null ? property.Name : attr.Name;
        }
    }
}