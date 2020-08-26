# Sudoku

C / C# / Go / Rust / Java / Python / Ruby で数独を解くプログラム

## 実行環境

お手元のマシンで動かすか、または Vagrant をご利用ください。

```
vagrant up
vagrant ssh
```

## 使用するデータ

[Kaggle から数独のデータセット](https://www.kaggle.com/bryanpark/sudoku/version/3)をダウンロードして、sudoku.csv として配置してください。

## 実行方法

```sh
# 実行
make run

# コンパイラ・ランタイムのバージョン表示
make version
```
