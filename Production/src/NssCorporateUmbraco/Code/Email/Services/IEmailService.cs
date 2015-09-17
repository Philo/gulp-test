using NssCorporateUmbraco.Code.Email.Models;

namespace NssCorporateUmbraco.Code.Email.Services
{
    public interface IEmailService
    {
        void Send<T>(T model, string templateLocation, EmailSettings settings);
    }
}