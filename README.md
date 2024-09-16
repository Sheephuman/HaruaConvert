# このソフトウェアについて

カジュアルな外観（かなり多分）のffmpegフロントエンドです
WPF/C# で制作しています。<br>
aviutlのLog表示の挙動を取り入れてます<br>

元ネタは　Twitter用に動画変換するヤツ([2016_twitter_convert](https://cloth.moe/2016_twitter_convert)　)（2019）　で、ffmpegに渡すパラメータを随時変更出来、bitrate指定により動画圧縮まで可能にします。

<br>
機能の概要<br>
a.ドラッグ&ドロップで動画をTwitter（X）に投稿可能な動画ファイルとして出力します。<br>
　-b:v 1200k -codec:v h264 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -y -threads 2<br>
h264とvf yadif=0:-1:1　がポイントらしいです。

![285221377-f659886e-615e-410b-8055-225ecf9d745f](https://github.com/user-attachments/assets/6f90793d-cf8a-48f4-bfc1-d83178d4daf1)


<br>
パラメータは公開されており、好みのパラメータに差し替え可能です。
 
 <br>
b.ffmpegに渡すパラメーターを任意に変更出来るため、画像出力やgif出力にも対応します。<br>

<br>
c.パラメーターを100個まで保存しておけます。<br>

![285222181-1941a8df-8f37-4589-b66f-b299b215d6ae](https://github.com/user-attachments/assets/e6e9a938-22ef-499e-8d97-b79cd6399883)


<br>

4.bitrateを指定する事で、動画のサイズ圧縮を可能にします<br>
　→ソース動画を読み込むとbitrateなどの情報が出るようになっています。表示されたbitrate以下の数を指定してください。<br>
 　　最低でも500k以上を指定した方が画質を維持出来ます。<br>
  →簡易の動画Codec情報閲覧ツールとしても使えなくもないです。


2024/8/17　開発版
　
## Download the Application

[暫定的な開発版(v1.1.0-alpha)](https://github.com/Sheephuman/HaruaConvert/releases/tag/v1.1.0-alpha) of the application.


# 最近の更新
・パラメータ組み立て機能の追加　作りかけなのでちょっと不便な点もあるかもしれないです

![Uploading 367869898-80ef7301-f9c9-4df9-8641-f5135b5d4a5c.png…]()



# 今後の更新予定<br>
・任意の画像でWaterMarkを付けられるようにする
<br>
・変換する添え字を変えられるように改良<br>
・GIFアニメ等を付ける<br>
・拡張パラメータシステムの機能追加<br>
・色合いも変更出来るように
