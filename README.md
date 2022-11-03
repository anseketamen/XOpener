# XOpener

ChromeやEdgeからエクスプローラーを開くツール


## インストール

`git clone`するか右上のボタンから`Download ZIP`するかしてファイルを落として、Cドライブ直下に`xopener`というフォルダを作ってその中にファイルを配置します。

この3つのファイルがあればOKです。

```
C:\xopener\install-xopener.bat
C:\xopener\xopener.cs
C:\xopener\xopener.reg
```

配置したら`install-xopener.bat`を実行します。UAC制御が出てきたら「はい」を選択してください。


## 使い方

`xopener:ファイルパス`という形のURLにアクセスします。Cドライブなら`xopener:C:\`です。

Webページだったら以下のようにします。

```html
<a href="xopener:C:\">Cドライブへのリンク</a>
```

Markdownはこれ。

```markdown
[Cドライブへのリンク](xopener:C:\\)
```


## その他

「Cドライブ直下にフォルダ作りたくない！」って人がいたら、`xopener.reg`の中身を書き換えて対応してください。


## ライセンス

MIT


## 仕組み

ChromeやEdgeでエクスプローラを起動してフォルダを開く - アンセケターメンはてなエディション - https://anseketamen.hatenablog.com/entry/2022/09/19/211136
