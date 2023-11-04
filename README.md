# HaruaConvert
ffmpegフロントエンド（動画変換アプリ）の改修依頼です。<br>
WPF/C# で制作しています。<br>
<br>
機能の概要<br>
a.ドラッグ&ドロップで動画をTwitter（X）に投稿可能な動画ファイルとして出力します。<br>
　-b:v 500k -codec:v h264 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -y -threads 2<br>
 <br>
b.ffmpegに渡すパラメーターを任意に変更出来るため、画像出力やgif出力にも対応します。<br>

c.パラメーターを100個まで保存しておけます。<br>
<br>
4.bitrateを指定する事で、動画のサイズ圧縮を可能にします<br>
　→ソース動画はbitrateなどの情報が出るようになっています。表示されたbitrate以下の数を指定してください。<br>
