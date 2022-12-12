using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace XOpenerConverter
{
    public static class Program
    {
        private static string CanCopyMessage = "\r\n\r\n（このメッセージは Ctrl + C でコピーできます）";

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var data = Clipboard.GetDataObject();
                if (data.GetDataPresent(DataFormats.Text))
                {
                    var path = Clipboard.GetText();
                    if (string.IsNullOrEmpty(path) || path.Contains("\n") || path.StartsWith("http:"))
                    {
                        System.Media.SystemSounds.Beep.Play();
                        MessageBox.Show("有効な文字列がクリップボードにありません\r\n" + path + CanCopyMessage, "XOpener-Converter エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var pathWithoutQuotation = path.Replace("\"", "");
                        CopyPathToClipBoard(pathWithoutQuotation);
                    }
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show(string.Join(",", data.GetFormats().Select(x => x.ToString())), "XOpener-Converter エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var path = args[0];
                if (string.IsNullOrEmpty(path) || path.Contains("\n") || path.StartsWith("http:"))
                {
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show("有効な文字列ではありません\r\n" + path + CanCopyMessage, "XOpener-Converter エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var pathWithoutQuotation = path.Replace("\"", "");
                    CopyPathToClipBoard(pathWithoutQuotation);
                }
            }
        }

        private static void CopyPathToClipBoard(string pathWithoutQuotation)
        {
            var url = "http://localhost:10082?path=" + HttpUtility.UrlEncode(pathWithoutQuotation);
            Clipboard.SetText(url);

#if DISABLE_RESULT_WINDOW
            // すぐに終了するとクリップボードに貼り付けられないので少し待つ
            Task.Run(async () =>
            {
                await Task.Delay(500);
                Application.Exit();
            });
            Application.Run();
#else
            System.Media.SystemSounds.Asterisk.Play();
            MessageBox.Show(pathWithoutQuotation + "\r\nをHTML形式に変換しました", "XOpener-Convert 情報", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
#endif
        }

    }
}