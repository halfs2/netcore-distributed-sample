using NCD.Core.DomainObjects;
using System.Text.RegularExpressions;

namespace NCD.Customers.API.Model.ValueObjects
{
    public class Email
    {
        public const int EmailAddressMaxLength = 254;
        public const int EmailAddressMinLength = 5;
        public string EmailAddress { get; private set; }

        public Email(string emailAddress)
        {
            if (!IsValid(emailAddress)) throw new DomainException("Invalid e-mail");
            EmailAddress = emailAddress;
        }

        public static bool IsValid(string email)
        {
            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return regexEmail.IsMatch(email);
        }
    }
}
