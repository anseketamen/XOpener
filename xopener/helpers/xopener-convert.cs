using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace XOpenerConverter
{
    public static class Program
    {
        private static readonly bool _hidesConvertedPopUp = false;
        private static string CanCopyMessage = "\r\n\r\n（このメッセージは Ctrl + C でコピーできます）";

        [STAThread]
        public static void Main(string[] args)
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
                    var url = "http://localhost:10082?path=" + HttpUtility.UrlEncode(pathWithoutQuotation);
                    Clipboard.SetText(url);

                    System.Media.SystemSounds.Asterisk.Play();
                    if (_hidesConvertedPopUp == false)
                    {
                        MessageBox.Show(pathWithoutQuotation + "\r\nを" + url + "\r\nに変換しました", "XOpener-Converter 情報", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(string.Join(",", data.GetFormats().Select(x => x.ToString())), "XOpener-Converter エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}