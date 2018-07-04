# uPDFLoader
UnityでPDFをTexture化してロードできるようにするライブラリです。(Windows限定)

PDFをコマンドラインでランタイムに1枚ずつ画像化(jpg)し、JPGの連番をキャッシュディレクトリに生成します。(PDF変換時に連番画像のパスを取得しているのでそれを用いて擬似的にPDFをロードできます。)

UniRxを用いて別スレッドで非同期に変換処理を行っているため、変換の作業にはある程度時間がかかりますが、別スレッドで行うためアプリの処理時間に影響を及ぼしにくいものになっています。

# 使い方
### クローンまたはUnityPackageからインポートする。

### [GhostScriptのインストール](https://www.ghostscript.com/download/gsdnld.html)

### UniRxの導入
https://assetstore.unity.com/packages/tools/unirx-reactive-extensions-for-unity-17276


# コード

``` PDFLoader.cs
        PDFGenerator gen = new PDFGenerator();
        //変換処理を行う前のコードを書く

        gen.PDFGenerateObservable(pdfName)
                .Subscribe(directoryPath =>
            {
                info = new PDFImageDirInfo(directoryPath);
                //変換処理後のコードを書く
            });
```

# UnityPackage版
[リリースチャンネルの方を参照して下さい](https://github.com/negipoyoc/uPDFLoader/releases)


# 注意
このコードで使われているGhostScriptのバージョンは __gs9.23__ です。<br>
またインストール時にデフォルトで選択されるディレクトリにインストールされています。

もしGhostScriptのバージョンがgs9.23ではない、またはインストールディレクトリがデフォルトではない場合は、コードの書き換えが必要になります。

そういった時は、PDFGenerator.csのgsPath変数に、任意の __GhostScriptの実行ファイルのパス__ を書いて下さい。


```
        readonly string gsPath = string.Format("\"{0}\"", @"C:\Program Files\gs\gs9.23\bin\gswin64c.exe");
```

# 権利表記
使用させていただいたものに関して以下に表記します。

-----

UniRx <br>
https://github.com/neuecc/UniRx

The MIT License (MIT)

Copyright (c) 2014 Yoshifumi Kawai

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
