# GitUnity

GitUnityはUnityのプロジェクト内でGitHubのリポジトリを操作するユーティリティです。
GitHubのリポジトリの作成やクローン作成をサポートし、開発の効率化を図ります。

# Requirement

* gitのインストール
* GitHubのAPIであるPersonal Access Token(以下、PAT)

# Installation

* gitのインストール方法は以下を参照してください  
  [Gitのインストール](https://git-scm.com/book/ja/v2/%E4%BD%BF%E3%81%84%E5%A7%8B%E3%82%81%E3%82%8B-Git%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)

* PATの取得方法は以下を参照してください  
  [personal access token (classic) の作成]([https://docs.github.com/ja/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#personal-access-token-classic](https://docs.github.com/ja/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#personal-access-token-classic-%E3%81%AE%E4%BD%9C%E6%88%90))

* GitHubDesktopのインストール方法は以下を参照してください（任意）
  [GitHubDesktopのインストール](https://docs.github.com/en/desktop/installing-and-authenticating-to-github-desktop/installing-github-desktop)

# Usage

### 1. 初期設定(PATの変更後も)

1. UnityのメニューバーTools/GitUnity/Set Repositoryを選択します。
   すると①のウィンドウが開きます。

2. UserSettingsのボタンを押すと②のウィンドウが開きます。

3. ウィンドウに表示される、次の項目を入力してください
   * GitHubのユーザー名
   * GitHubのPAT
   入力し、Saveボタンを押す。

### 2. リモートレポジトリの作成

1. ①のウィンドウに戻ります。
   戻ったら、作成するレポジトリの次の項目を入力してください
   * レポジトリ名
   * 説明
   * 公開/非公開

2. 入力し終わったら、CreateRepositoryボタンを押します。

3. 少し待つと、レポジトリの作成とともに作成したレポジトリのページが開きますので、確認してください。

### 3. ローカルレポジトリの設定

1. 次にSelect local Repositoryボタンを押し、プッシュしたいフォルダを選択してください。

2. 選択が終わるとLocal Repository patに設定されます。

3. 設定されたのを確認したら、InitialiseRepositoryボタンを押してください。
   すると、選択したフォルダパスに作成したリモートレポジトリがクローンされ、利用可能になります。

### 4. GitHubDesktopの起動(任意)

Open GitHubDesktopボタンを押すと、GitHub Desktopでの利用ができるようになります。

# Note

GitUnityは以下の操作をコマンドプロンプトを使用して行います。
* ローカル環境でのgit操作(レポジトリの初期化、クローン)
* GitHubDesktopの起動

また、セキュリティ対策はなされていません。
アクセストークンの漏洩などの責任は一切負えないので、使用時には十分な注意が必要です。
# Author

* epen3420

# License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
