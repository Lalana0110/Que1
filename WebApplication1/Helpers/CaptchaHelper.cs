using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace WebApplication1.Helpers
{
    //验证码
    public class CaptchaHelper
    {
        // 生成验证码
        public static (string code, byte[] imageData) GenerateCaptcha()
        {
            // 生成随机验证码
            string code = GenerateRandomCode(4);

            // 创建验证码图片
            using (Bitmap bitmap = new Bitmap(100, 40))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // 设置背景色
                    g.Clear(Color.White);

                    // 绘制干扰线
                    Random random = new Random();
                    for (int i = 0; i < 5; i++)
                    {
                        int x1 = random.Next(bitmap.Width);
                        int y1 = random.Next(bitmap.Height);
                        int x2 = random.Next(bitmap.Width);
                        int y2 = random.Next(bitmap.Height);
                        g.DrawLine(new Pen(Color.LightGray), x1, y1, x2, y2);
                    }

                    // 绘制验证码
                    Font font = new Font("Arial", 18, FontStyle.Bold | FontStyle.Italic);
                    LinearGradientBrush brush = new LinearGradientBrush(
                        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        Color.Blue, Color.DarkRed, 1.2f, true);
                    g.DrawString(code, font, brush, 10, 5);

                    // 绘制干扰点
                    for (int i = 0; i < 30; i++)
                    {
                        bitmap.SetPixel(random.Next(bitmap.Width), random.Next(bitmap.Height),
                            Color.FromArgb(random.Next()));
                    }

                    // 保存到内存流
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Jpeg);
                        return (code, ms.ToArray());
                    }
                }
            }
        }

        // 生成随机字符串
        private static string GenerateRandomCode(int length)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            char[] codeChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                codeChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(codeChars);
        }


        // MD5加密密码
        public static string Md5Encrypt(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 转换为十六进制字符串
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}