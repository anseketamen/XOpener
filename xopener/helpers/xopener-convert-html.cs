using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Text;
using System.Globalization;

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
                    MessageBox.Show("有効な文字列がクリップボードにありません\r\n" + CanCopyMessage, "XOpener-Convert エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var pathWithoutQuotation = path.Replace("\"", "");
                    var url = "<a href=\"" + "http://localhost:10082?path=" + HttpUtility.UrlEncode(pathWithoutQuotation) + "\">xopener:" + pathWithoutQuotation + "</a>";
                    var html = CreateHtmlDocument(url);
                    // HTMLをクリップボードに貼り付けるときはUTF-8に変換したMemoryStreamにする
                    // byte[]だとエラーになった
                    var obj = new DataObject();
                    obj.SetData(DataFormats.Html, true, new MemoryStream(Encoding.UTF8.GetBytes(html)));
                    obj.SetData(DataFormats.Text, true, "xopener:" + pathWithoutQuotation);
                    Clipboard.SetDataObject(obj);

                    System.Media.SystemSounds.Asterisk.Play();
                    // コンバート確認のポップアップを表示
                    if (_hidesConvertedPopUp == false)
                    {
                        MessageBox.Show(pathWithoutQuotation + "\r\nを" + html + "に変換しました", "XOpener-Convert 情報", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("有効な文字列がクリップボードにありません\r\n" + CanCopyMessage, "XOpener-Convert エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string CreateHtmlDocument(string htmlBody)
        {
            // こんな感じのテキストを生成する
            // StartHTMLからEndFragmentまで、000000でもWordでは貼り付けられた
            // だけど一応UTF-8にしたときのバイト数をちゃんと計算しておく
            // =============================================================================================================
            // Version:0.9
            // StartHTML:000000
            // EndHTML:000000
            // StartFragment:000000
            // EndFragment:000000<html><head><meta http-equiv="Content-Type" content="text/html; charset=utf-8"></head>
            // <body><!--StartFragment-->
            // <a href="リンク先">リンクテキスト</a>
            // <!--EndFragment--></body>
            // </html>
            // =============================================================================================================

            var htmlBuilder = new StringBuilder();

            var htmlHeader = "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head>\r\n<body>\r\n<!--StartFragment-->\r\n";
            var htmlFooter = "<!--EndFragment-->\r\n</body>\r\n</html>\r\n";

            int headerSize = 89;
            int htmlHeaderSize = Encoding.UTF8.GetByteCount(htmlHeader);
            int htmlFragmentSize = Encoding.UTF8.GetByteCount(htmlBody);
            int htmlFooterSize = Encoding.UTF8.GetByteCount(htmlFooter);

            htmlBuilder.Append("Version:0.9\r\nStartHTML:");
            htmlBuilder.AppendFormat("{0:000000}", headerSize);
            htmlBuilder.Append("\r\nEndHTML:");
            htmlBuilder.AppendFormat("{0:000000}", headerSize + htmlHeaderSize + htmlFragmentSize + htmlFooterSize);
            htmlBuilder.Append("\r\nStartFragment:");
            htmlBuilder.AppendFormat("{0:000000}", headerSize + htmlHeaderSize);
            htmlBuilder.Append("\r\nEndFragment:");
            htmlBuilder.AppendFormat("{0:000000}", headerSize + htmlHeaderSize + htmlFragmentSize);
            htmlBuilder.Append("\r\n");
            htmlBuilder.Append(htmlHeader);
            htmlBuilder.Append(htmlBody);
            htmlBuilder.Append("\r\n");
            htmlBuilder.Append(htmlFooter);

            return htmlBuilder.ToString();
        }
    }
}
