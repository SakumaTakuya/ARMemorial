# シーンについて
`Assets/Scenes`においてある。
 - `Main.unity`
   - 全部の画面が入っている
   - モデルの登録ができるようになっているが見かけのみ
   - Editor上でしか編集することができない仕様
 - `ForBuild.unity`
   - Vuforiaの部分のみが含まれている
   - Editor上であらかじめモデルを登録してビルドする必要がある


# 設定
## Vuforiaモデルの追加
1. [こんな感じで](https://www.atmarkit.co.jp/ait/articles/1702/14/news023_2.html)プロジェクトにモデルを追加
2. `ForBuild`シーン上の`ARCamera/ObjectTarget`の`Object Target Behaviour`の設定を変更
3. とりあえずエディタ上で動作確認

## ビルド
現在はStandalone向けのビルド設定になっているのでAndoridに変更してビルド。その際に、ForBuildシーンが追加されていることを確認。

# 機能一覧
- 撮影モード
- モデルの新規追加機能(Editorのみ、見かけのみ)
- モデルの修正機能(Editorのみ、見かけのみ)
- 設定機能(見かけのみ)
![](https://i.imgur.com/m1dCNMx.png)
![](https://i.imgur.com/zOKX3DL.png)
![](https://i.imgur.com/3sSEZlA.jpg)
