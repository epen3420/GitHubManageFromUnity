using GitUnity.Repository;
using UnityEditor;
using UnityEngine;

namespace GitUnity.Editor
{
    public class SetRepositoryWindow : EditorWindow
    {
        [MenuItem("Tools/GitUnity/Git Repository Manager")]
        public static void ShowWindow()
        {
            // ツールウィンドウの作成と表示
            GetWindow<SetRepositoryWindow>("Git Repository Manager");
        }

        private GitRepositoryManager gitRepoManager;
        private string repoName = "New_Repository"; // 作成するリポジトリ名
        private string repoDescription = "Description"; // 作成するレポジトリの説明
        private bool isPublic; // レポジトリの公開設定
        private float windowWidth = 600;
        private float windowHeight = 350;

        private void OnEnable()
        {
            var newPosition = new Rect(position.x, position.y, windowWidth, windowHeight);
            position = newPosition;

            gitRepoManager = new GitRepositoryManager(new CommandRunner());
        }

        private void OnGUI()
        {
            GUILayout.Label("Git Repository Manager", EditorStyles.boldLabel);

            // リモートレポジトリの設定画面に行く
            if (GUILayout.Button("User Settings"))
            {
                UserSettingsWindow.ShowWindow();
            }

            // ユーザー入力フィールド
            repoName = EditorGUILayout.TextField("Repository Name", repoName);
            repoDescription = EditorGUILayout.TextField("Description", repoDescription);
            isPublic = EditorGUILayout.Toggle("Publish", isPublic);

            // ボタン押下時にリポジトリを作成
            if (GUILayout.Button("Create Repository"))
            {
                // レポジトリ設定を作成
                var repositorySettings = new RepositorySettings
                {
                    name = repoName,
                    description = repoDescription,
                    @private = !isPublic
                };
                CreateGitRepo(repositorySettings);
            }

            //ローカルレポジトリのディレクトリ選択
            if (GUILayout.Button("Select Local Repository Path"))
            {
                // フォルダ選択ダイアログを表示
                string selectedPath = EditorUtility.OpenFolderPanel("Select Local Repository Folder", "", "");
                // フォルダが選択された場合
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    gitRepoManager.LocalRepoPath = selectedPath;
                }
            }

            // 選択したローカルリポジトリのパスを表示
            EditorGUILayout.LabelField("Local Repository Path:", gitRepoManager.LocalRepoPath);

            // ローカルレポジトリの初期化
            if (GUILayout.Button("Initialize Local Repository"))
            {
                bool success = gitRepoManager.InitLocalRepo(repoName);
                if (!success)
                {
                    EditorUtility.DisplayDialog("エラー", $"レポジトリの初期化に失敗しました。詳細はログファイル({LogUtility.GetLogFilePath()})を確認してください。", "OK");
                }
            }

            // GitHubDesktopの起動
            if (GUILayout.Button("Open GitHubDesktop"))
            {
                if (EditorUtility.DisplayDialog("確認", "GitHubDesktopを開きますか？", "はい", "いいえ"))
                {
                    gitRepoManager.OpenGitHubDesktop();
                }
            }
        }

        /// <summary>
        /// リモートレポジトリの作成をしたあとそのページに行く
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        private async void CreateGitRepo(RepositorySettings settings)
        {
            bool success = await gitRepoManager.CreateRemoteRepo(settings);

            // 作成したレポジトリのページに飛ぶ
            if (success)
            {
                if (EditorUtility.DisplayDialog("レポジトリの作成", "レポジトリの作成に成功しました！\nGitHubのレポジトリページを開きますか？", "はい", "いいえ"))
                {
                    Application.OpenURL($"https://github.com/{TokenManager.GetToken().Split('%')[0]}/{repoName}");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("エラー", $"リポジトリの作成に失敗しました。詳細はログファイル({LogUtility.GetLogFilePath()})を確認してください。", "OK");
            }
        }
    }
}
