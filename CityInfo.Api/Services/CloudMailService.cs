namespace CityInfo.Api.Services {
    public class CloudMailService : IMailService {
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration) {
            _mailTo = configuration["MailSettings:MailTo"];
            _mailFrom = configuration["MailSettings:MailFrom"];
        }


        public void Send(string subject, string message) {
            Console.WriteLine($"Nachricht gesendet an {_mailTo} von {_mailFrom} durch {nameof(CloudMailService)}");
            Console.WriteLine($"Betreff: {subject}");
            Console.WriteLine($"Nachricht: {message}");
        }
    }
}
