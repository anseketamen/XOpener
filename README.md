# XOpener

ChromeやEdgeからエクスプローラーを開くツール


## インストール

1. `git clone`するか右上のボタンから`Download ZIP`するかしてファイルを落として、中の`xopener`というフォルダをCドライブ直下に配置します。  
以下のようなファイル・フォルダがあればOKです。

```
C:\xopener\install-xopener.bat
C:\xopener\install-xopener-full.bat
C:\xopener\xopener.cs
C:\xopener\xopener-pico.reg
C:\xopener\xopener-full.reg
C:\xopener\helpers
C:\xopener\helpers\xopener-convert.cs
C:\xopener\helpers\xopener-convert-html.cs
C:\xopener\helpers\xopener-trans.cs
C:\xopener\helpers\xopener-trans.lnk
```

※エクスプローラー上では`XOpener-Trans.lnk`の拡張子は見えなくなっています。

2. バッチファイルを起動します。以下3つのうちから1つを選んでください。

  * 【おすすめ】XOpener-Helpersもインストールする場合:  
  `install-xopener-full.bat`を実行します。UAC制御が出てきたら「はい」を選択してください。

  * XOpenerだけインストールする場合:  
`install-xopener.bat`を実行します。UAC制御が出てきたら「はい」を選択してください。


## 使い方

`xopener:[ファイルパス]`という形のURLにアクセスします。Cドライブなら`xopener:C:\`です。

Webページだったら以下のようにします。

```html
<a href="xopener:C:\">Cドライブへのリンク</a>
```

Markdownはこれ。

```markdown
[Cドライブへのリンク](xopener:C:\\)
```

`install-xopener-full.bat` を使用して XOpener-Helpers もインストールした場合は以下も使えます。

`helpers\\xopener-convert.exe` を実行、もしくは「送る」などに登録して実行した場合、Discordなどで使えるプレーンテキスト形式のリンクが生成されます。  
ただし、リンクを開くためには送信先の相手も `install-xopener-full.bat` を使用して XOpener のインストールを済ませておく必要があります。

エクスプローラのファイルやフォルダをShiftを押しながらクリックしてコンテキストメニューから「XOpener用にパスをコピー」を選択すると、Teamsなどで使えるHTML形式のリンクが生成されます。  
`xopener-convert.exe` と同様、直接exeをダブルクリックしたり「送る」などに登録したりして実行しても同様の動作となります。  
こちらも、リンクを開くためには送信先の相手も `install-xopener-full.bat` を使用して XOpener のインストールを済ませておく必要があります。


## アンインストール

1. `xopener.reg`を見つつ、それっぽいレジストリキーを削除します。XOpener-Helpersも利用していた場合は、2.以降も実施します。
2. スタートアップにXOpener-Transのショートカットが作成されているので削除します。
3. XOpener-Trans.exeが起動していれば、タスクマネージャから終了します。
4. `xopener-convert-shell.reg` を見つつ、それっぽいレジストリキーを削除します。
5. helpersフォルダを削除します。


## XOpener-Helpersについてもう少し詳細

`helpers`フォルダに入っている、XOpenerをさらに便利に使うためのツールです。

### 何に使うのこれ

Microsoft TeamsからXOpenerを使ってエクスプローラーでフォルダを開くために使います。

ChromeやEdgeからエクスプローラーを開くことができるXOpenerですが、Microsoft Teamsでは使うことができません。Microsoft Teamsは現時点 (2022/11/08) でカスタムURIスキームのリンクに対応しておらず、httpやhttpsのリンクにのみ対応しているからです。

そのため、一旦localhostの10082番ポートにアクセスするhttpのリンクを経由して、そこからXOpenerのリンクに転送することで、Microsoft TeamsでもXOpenerの利用を可能にしました。

Microsoft Teams以外にDiscordなどでも使うことができます。


### 中身

* XOpener-Trans：メインのプログラムです。待ち受けポートに対しパスが`/`、クエリが`?path=[ファイル・フォルダパス]`となっているURLでアクセスすることでxopenerを起動できるようにします。
* XOpener-Convert：ファイルパスをクリップボードにコピーした状態でこれを起動することで、XOpener-Transで用いるhttpリンクを作成することができます。
* XOpener-Convert-Html：XOpener-ConvertだとエンコードされたURLで読みづらいため、代わりにHTMLに整形したリンクを生成します。TeamsやWordなどにそのままリンクとして貼り付けることができます。


## その他

「Cドライブ直下にフォルダ作りたくない！」って人がいたら、`xopener.reg`などの中身を書き換えて対応してください。


## ライセンス

MIT


## 仕組み

ChromeやEdgeでエクスプローラを起動してフォルダを開く - アンセケターメンはてなエディション - https://anseketamen.hatenablog.com/entry/2022/09/19/211136
