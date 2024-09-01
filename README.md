# IVRC-2024

## PUNについて
-基本的には同じシーン内のプレイヤー同士での通信を担う
-PUNを用いるときはクラスMonoBehaviourPunCallbacksを用いる（PlayerMake.csなどを参考）
-appIDは7977312e-2eb9-498b-8bd7-29944e310b41
-現在各プレイヤーの位置座標の同期はとっていない（PhotonTransformViewを付ければよいだけ）

## Dungeonについて
-Assets内のSimple Modular Dungeon内のPrefabsを組み合わせて作る
-基本的な部屋構造はAssets内のRoom Prefabsに入れてある
-宝箱についてはChestで調べれば出てくる
-player1がplayer2に介入する際、直接player2 roomに行くのではなく、player2 room copy（アクセス時に作成）に行く
-20240829追記:layerを設定し、一部の衝突判定をなくしました

## 共有について
-位置情報（positionとrotation）はscriptableobjectに保存（Transform Dataに入れてある）
-開始時にリセットされる
-およそ0.1秒ごとに新しい位置情報が追加される（ObjectTransformSave.csで実行、RecordIntervalで記録時間を設定）
-自分のデータが更新されるのは確認済みだが、他人の位置情報が更新されるのはまだ見ていない（だめなら別の方法を試す）
-20240829追記：TransformSaveからGameManager内のPlayerDataControllerで保存するように変更しました

## ゲーム開始時について
-ゲーム開始時にPlayerMake.csによってResources内のAvatar1およびAvatar2が生成される（OnJoinedRoom）
-20240829追記：3人目以降は「他のプレイヤーがプレイ中ですという表示がでるようにした」

## Avatarの構造について
-OVRPlayerControllerが元になっている
-移動はJoyStickMove.csで設定（movespeedで移動速度管理）→20240829:OVRPlayerControllerで管理（Avatar1の速度を0.1から0.2に変更）
-Transformが変わるのはOVRCameraRigの中のEyeAnchor
-デフォルトだとCenterEyeAnchorにAudioListenerがついてくるが、それをEarに変更
-位置情報についてはAvatarの位置情報が変わる（EyeAnchorのlocalPositionも少し変わるけど誤差？）

## AudioControllerについて
-各プレイヤーのAudioSourceはtagで識別している
-基本的にはlocalPosition（親となるAvatarとの相対位置）ではなくposition（Earの絶対位置）を管理（rotaitonも同様）
-各音源との距離を計算し、近い二つのみ鳴らし、残り七つはオフにする
-指定された入力があったとき（現在はキーボードでpを入力）に相手にアクセスする形になっているが、これを時間によってコントロールする形にしたい
-相手にアクセスする場合はpositionDataやrotationDataからデータを取得しているが、どのインデックスの情報をとるかについてはまだ適当（RecordIntervalと合わせておよそ何番目以降という形で設定したい）
-読みだす間隔はDuration内のWaitForSecondの引数になっている（これが保存の時と違う形なのがやりずらい）

## ワールドの複製について（ObjectDuplicator.cs）
-プレイヤー2のワールドとプレイヤー2本人を複製
-位置の更新はMovableとタグのついているもののみ行う
-20240829追記：今確認するのは位置・アクティブかどうか・レンダー

## インタラクションについて(20240829追記)
-宝箱についてはトリガーを引くことで消すことが可能
-壁についても現在は宝箱と同様（理想は色を変えて移動できるようにしたい、おそらくPlayerSpecificWallInteraction内のTryRemoveWallを書き換えれば良さそう）
-壁との衝突判定が小さい（下の真ん中らへんのみ？）なのでこれも何とかしたい

## PlayerStopについて(20240829追記)
-PlayerStop/Player2Stopは二人同時にアクセスするまで、移動・位置の保存・タイマーを止める
-二人そろうとGame Start!!の文字が出て動けるようになる
-一人でのテストプレイをする際にはこのスクリプトのチェックを外すことで動かすことができる

## その他(20240829追記)
-Avatarの作成はMakeHumanでfbxを適当に作ってavatarのPrefabと組み合わせるのが良さそう
-AvatarMovementLimiterがどこに使われているか不明→まだ試していない？
-音はもう少し変えても良さそう

## アクセスのタイミングについて
-ObjectDuplicator.csのInspectorで設定
-使うスクリプト：AccessCopyWorld.cs（インデックス）、ObjectDuplicator.cs（インデックス、秒）
-indexlistでアクセスするインデックスを設定し、updateindextimeで更新する時間を決める
-updateindextimeはアクセスした時間の2倍の時刻（アクセスするタイミングを固定化してもいいかも→今はAボタンになっている）
-indexlistはこちらで固定化

##player2への遷移画面について
UIの通り、スライダーでplayer2の何分何秒に行けるかを操作。今はスクリプト(playerBtime.cs)内でplayer1/2の速さの比率を決めている（現状2倍）。
timeはObjectDuplicator.csのtimeを参照。
playerBtimeで現在時刻（time）より右側にはスライダーを動かせない仕様。
AボタンのRayドラッグでスライダーを動かせる。
AbuttonMainToPast.csで、メイン画面でAボタンを押すと遷移画面に移る仕様。（objectduplicator.csとaccesscopyworld.csとかぶってるので直す）
