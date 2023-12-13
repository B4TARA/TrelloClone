using System.IO;
using System;

namespace TrelloClone.Services.Base64Decode
{
    public class Base64Decode
    {
        public static void Base64ToImage(string base64String, string f_name, string birth_date, string f_type)
        {
            if (f_type != "")
            {
                string b_date = birth_date.Replace(".", "");

                //var path = "C:\\Users\\tomchikadm\\Documents\\GitHub\\TrelloClone\\TrelloClone-master\\src\\TrelloClone\\wwwroot\\image\\user_image\\";
                var path = "C:\\Users\\evgen\\OneDrive\\Документы\\GitHub\\TrelloClone\\TrelloClone-master\\src\\TrelloClone\\wwwroot\\image\\user_image\\";

                var bytes = Convert.FromBase64String(base64String);
                using (var imageFile = new FileStream(path + f_name + b_date + "." + f_type, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
            }
        }
    }
}
