using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace company.G03.PL.Helper
{
    public static class EmailSetting
    {
        public static bool SendEmail(Email email)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("moyassin2004522@gmail.com", "nhtrerqgnkqxosng");
                client.Send("moyassin2004522@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
    } }
}