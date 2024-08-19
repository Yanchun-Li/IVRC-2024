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

## 共有について
-位置情報（positionとrotation）はscriptableobjectに保存（Transform Dataに入れてある）
-開始時にリセットされる
-およそ0.1秒ごとに新しい位置情報が追加される
-自分のデータが更新されるのは確認済みだが、他人の位置情報が更新されるのはまだ見ていない（だめなら別の方法を試す）

## ゲーム開始時について
-ゲーム開始時にPlayerMake.csによってResources内のAvatar1およびAvatar2が生成される（OnJoinedRoom）

## Avatarの構造について
-OVRPlayerControllerが元になっている
-
