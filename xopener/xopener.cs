using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Web;
using System.Windows.Forms;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            // 何か引数がある場合
            // カスタムURLスキームでは1番目の引数に「xopener:ファイルパス」が入る
            // URLエンコードされてるのでデコードして、冒頭のxopener:も消す
            var path = HttpUtility.UrlDecode(args[0]).Replace('/','\\').Replace("xopener:","");

            if (File.Exists(path) || Directory.Exists(path))
            {
                // ファイル・フォルダが存在するなら開く
                Process.Start("explorer.exe", path);
            }
            else
            {
                // ファイル・フォルダが存在しない
                SystemSounds.Beep.Play();
                MessageBox.Show(path + " は存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            // 引数がおかしい
            SystemSounds.Beep.Play();
            MessageBox.Show("起動には引数が必要です", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}