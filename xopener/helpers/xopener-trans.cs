using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace XOpenerTrans
{
    public class Program
    {
        private static RedirectServer _server;
        private static readonly int ListeningPort = 10082;

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                _server = new RedirectServer(ListeningPort);
                _server.Start();
            }
            catch (Exception)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("プログラムの起動に失敗しました。すでに同じプログラムが起動しているか、ポートが使用されています。", "XOpener-Trans エラー");
                return;
            }
            Application.Run();
        }
    }

    public class RedirectServer : IDisposable
    {
        private TcpListener _listener;
        private CancellationTokenSource _listenerTaskCancellationTokenSource;

        public RedirectServer(int listenerPort)
        {
            _listener = new TcpListener(IPAddress.Any, listenerPort);
            _listenerTaskCancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _listener.Start();

            Task.Run(() =>
            {
                while (true)
                {
                    if (_listenerTaskCancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }
                    var client = _listener.AcceptTcpClient();
                    Task.Run(async () =>
                    {
                        try
                        {
                            await TcpAcceptTask(client);
                        }
                        finally
                        {
                            client.Dispose();
                        }
                    }, _listenerTaskCancellationTokenSource.Token);
                }
            }, _listenerTaskCancellationTokenSource.Token);
        }

        private async Task TcpAcceptTask(TcpClient tcpClient)
        {
            using (var stream = tcpClient.GetStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                // ヘッダー部分を全部読んでおく
                var requestHeaders = new List<string>();
                while (true)
                {
                    var line = await reader.ReadLineAsync();
                    // HTTPはヘッダー部分とレスポンス部分の間にCR+LFだけの行がある
                    // そこまで読んだら終わり
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }
                    requestHeaders.Add(line);
                }

                // ヘッダが空なら400エラー
                if (requestHeaders.Count == 0)
                {
                    await WriteBadRequestAsync(writer);
                    return;
                }

                // 1行目は GET / HTTP/1.1 みたいな感じ
                var requestLine = requestHeaders.FirstOrDefault();
                var requestParts = requestLine.Split(' ');
                if (requestParts.Length != 3)
                {
                    await WriteBadRequestAsync(writer);
                    return;
                }

                // パスとクエリ
                var pathAndQuery = requestParts[1];

                // 転送用クエリを検出
                if (pathAndQuery.StartsWith("/?path="))
                {
                    var path = pathAndQuery.Substring(7);
                    var decodedPath = HttpUtility.UrlDecode(path);

                    // 転送
                    await writer.WriteLineAsync("HTTP/1.0 303 See Other");
                    await writer.WriteLineAsync("Content-Type: text/plain; charset=UTF-8");
                    await writer.WriteLineAsync("Location: xopener:" + path);
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("path: " + decodedPath);
                }
                else
                {
                    await WriteBadRequestAsync(writer);
                    return;
                }
            }
        }

        private async Task WriteBadRequestAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync("HTTP/1.0 400 Bad Request");
            await writer.WriteLineAsync("Content-Type: text/html; charset=UTF-8");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("<h1>Bad Request</h1>");
        }

        public void Dispose()
        {
            _listenerTaskCancellationTokenSource.Cancel();
            _listenerTaskCancellationTokenSource.Dispose();
            _listenerTaskCancellationTokenSource = null;
        }
    }
}
