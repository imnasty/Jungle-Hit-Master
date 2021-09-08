using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TMPro;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    public Text _feedbackText = default;
    [SerializeField] private string password;

    public void GoToStorePage()
    {
        GameManager.OffFeedBacks = 4;

#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + Application.identifier);
#endif
    }

    public void SendFeedback()
    {
        GameManager.OffFeedBacks = 4;

        MailAddress from = new MailAddress("bogdanbif@mail.ru");
        MailAddress to = new MailAddress("bogdanbif@mail.ru");
        MailMessage message = new MailMessage(from, to);

        message.Subject = "Отзыв";
        
        message.Body = _feedbackText.text;

        SmtpClient client = new SmtpClient("smtp.mail.ru", 587);
        client.Credentials = new NetworkCredential("bogdanbif@mail.ru", password);
        client.EnableSsl = true;
        client.Send(message);
    }


}
