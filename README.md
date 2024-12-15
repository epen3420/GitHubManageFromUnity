# GitUnity

GitUnityはUnityのプロジェクト内でGitHubのリポジトリを操作するユーティリティです。
GitHubのリポジトリの作成やクローン作成をサポートし、開発の効率化を図ります。

# Requirement

* gitのインストール
* GitHubのAPIであるPersonal Access Token(以下、PAT)

# Installation

* gitのインストール方法は以下を参照してください  
  [Gitのインストール](https://git-scm.com/book/ja/v2/%E4%BD%BF%E3%81%84%E5%A7%8B%E3%82%81%E3%82%8B-Git%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)

* PATの取得方法は以下を参照してください。なお、PATのアクセススコープは**repo**を選択してください  
  [personal access token (classic) の作成](https://docs.github.com/ja/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#personal-access-token-classic-%E3%81%AE%E4%BD%9C%E6%88%90)

* GitHubDesktopのインストール方法は以下を参照してください（任意）  
  [GitHubDesktopのインストール](https://docs.github.com/en/desktop/installing-and-authenticating-to-github-desktop/installing-github-desktop)

# Usage

#### ①SetRepositoryウィンドウ
![SetRepositoryウィンドウ](https://github.com/user-attachments/assets/934b6e6c-e204-4b5f-a1ed-1f8ab7f5aeb0)  
*SetRepositoryウィンドウは、リポジトリとユーザー設定の管理を行うためのウィンドウです。*

#### ②UserSettingsウィンドウ
![UserSettingsウィンドウ](https://github.com/user-attachments/assets/5671e47d-7dbc-46f8-8874-99c1a2d331db)  
*UserSettingsウィンドウでは、GitHubのユーザー名とPersonal Access Token(PAT)を入力できます。*

## 1. 初期設定(PATの変更後も)
1. UnityのメニューバーTools/GitUnity/Git Repository Managerを選択します。
   すると①のウィンドウが開きます。

2. UserSettingsのボタンを押すと②のウィンドウが開きます。

3. ②ウィンドウに表示される、次の項目を入力してください
   * GitHubのユーザー名
   * GitHubのPAT
   入力し、Saveボタンを押す。

## 2. リモートレポジトリの作成

1. ①のウィンドウに戻ります。
   戻ったら、作成するレポジトリの次の項目を入力してください
   * 名前
   * 説明
   * 公開/非公開

2. 入力し終わったら、CreateRepositoryボタンを押します。

3. すると、レポジトリ作成完了のダイアログとともに作成されたGitHubのレポジトリのページに飛ぶことができます。

## 3. ローカルレポジトリの設定

1. 次にSelect local Repositoryボタンを押し、プッシュしたいフォルダを選択してください。

2. 選択が終わるとLocal Repository Pathに設定されます。

3. 設定されたのを確認したら、InitialiseRepositoryボタンを押してください。
   すると、選択したフォルダパスに作成したローカルレポジトリが利用可能になります。

## 4. GitHubページを開く
Open GitHub Pageボタンを押すと、作成されたレポジトリのページに飛ぶことができます

## 5. コマンドプロンプトかGitHubDesktopの起動

Open your cmd or GitHubDesktopボタンを押すと、cmdかGitHub Desktopでローカルレポジトリを開くことができます。

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
