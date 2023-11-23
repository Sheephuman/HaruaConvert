# HaruaConvert

WPF/C# で制作しています。<br>
<br>
機能の概要<br>
a.ドラッグ&ドロップで動画をTwitter（X）に投稿可能な動画ファイルとして出力します。<br>
　-b:v 500k -codec:v h264 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -y -threads 2<br>

![スクリーンショット 2023-11-23 223057](https://github.com/Sheephuman/HaruaConvert/assets/34499259/f659886e-615e-410b-8055-225ecf9d745f)
<br>
パラメータは公開されており、好みのパラメータに差し替え可能です。
 
 <br>
b.ffmpegに渡すパラメーターを任意に変更出来るため、画像出力やgif出力にも対応します。<br>

<br>
c.パラメーターを100個まで保存しておけます。<br>
<br>
![スクリーンショット 2023-11-23 223259](https://github.com/Sheephuman/HaruaConvert/assets/34499259/1941a8df-8f37-4589-b66f-b299b215d6ae)

<br>

4.bitrateを指定する事で、動画のサイズ圧縮を可能にします<br>
　→ソース動画を読み込むとbitrateなどの情報が出るようになっています。表示されたbitrate以下の数を指定してください。<br>
 　　最低でも500k以上を指定した方が画質を維持出来ます。<br>
  →簡易の動画Codec情報閲覧ツールとしても使えなくもないです。


# 今後の更新予定<br>
・任意の画像でWaterMarkを付けられるようにする
<br>
・変換する添え字を変えられるように改良<br>
・GIFアニメ等を付ける<br>
・拡張パラメータシステムの機能追加<br>
・色合いも変更出来るように
