# 2d_scroll_game_2023
2023年開発中アプリゲーム

## 設定
# ターミナルでgit 操作するときの設定
- LF/CRLFの自動変換機能をオフにする。エラーが出る様子とgit configを用いた対処は以下のとおり。
```
satoukentas-iMac:2d_scroll_game_2023[master #]$ git add .
warning: CRLF will be replaced by LF in Library/PackageCache/com.unity.collab-proxy@1.17.7/.signature.
The file will have its original line endings in your working directory
git-lfs filter-process: git-lfs: command not found
fatal: the remote end hung up unexpectedly
satoukentas-iMac:2d_scroll_game_2023[master #]$ git config --local core.autoCRLF false

```