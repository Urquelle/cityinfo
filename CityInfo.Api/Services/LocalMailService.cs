namespace CityInfo.Api.Services {
    public class LocalMailService : IMailService {
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        public LocalMailService(IConfiguration configuration) {
            _mailTo = configuration["MailSettings:MailTo"];
            _mailFrom = configuration["MailSettings:MailFrom"];
        }

        public void Send(string subject, string message) {
            Console.WriteLine($"Nachricht gesendet an {_mailTo} von {_mailFrom} durch {nameof(LocalMailService)}");
            Console.WriteLine($"Betreff: {subject}");
            Console.WriteLine($"Nachricht: {message}");
        }
    }
}
