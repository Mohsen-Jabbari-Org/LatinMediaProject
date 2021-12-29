using Kavenegar;
using Kavenegar.Exceptions;
using System;
using PayamakPanel;
using System.Collections.Generic;
using System.Text;
using PayamakPanel.Core.Models;

namespace LatinMedia.Core.Senders
{
    public class SendSms
    {

        public static void Send(string Mobile , string Massage)
        {
            try
            {
                string TotalMsg = "کد فعالسازی شما در سامانه آموزش آنلاین موسسه راهگشا : " + Massage;
                var api = new KavenegarApi("45764277716566314637493844494A59324C5852544748466F453973724B75583832494F4F615536466A513D");
                var result = api.Send("1000596446", Mobile, TotalMsg);
            }

            catch (ApiException ex)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                Console.Write("Message : " + ex.Message);
            }
            catch (HttpException ex)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                Console.Write("Message : " + ex.Message);
            }
        }

        public static void SendSMS(string Mobile, string Massage)
        {
            PayamakPanel.Core.FaraApi fara = new PayamakPanel.Core.FaraApi();
            string TotalMsg = "کد فعالسازی شما در سامانه آموزش آنلاین موسسه راهگشا : " + Massage;
            Result result = fara.UseBaseService("09121783601", "Leon&45108", Massage, Mobile, 50159);
        }
    }
}
