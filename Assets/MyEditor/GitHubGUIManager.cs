using UnityEditor;
using UnityEngine;

public class GitHubGUIManager : EditorWindow
{
    private RepositoryManager repositoryManager;
    private string userName = "User Name"; // GitHubのユーザ名
    private string token; // トークン
    private string repositoryName = "New_Repository"; // 作成するリポジトリ名
    private string description = "Description"; // 作成するレポジトリの説明
    private bool isPublishing; // レポジトリの公開設定
    private Rect dropDownButtonRect;


    [MenuItem("Tools/GitHub/Create Repository (HTTP)")]
    public static void ShowWindow()
    {
        // ツールウィンドウの作成と表示
        GetWindow<GitHubGUIManager>("Create GitHub Repository (HTTP)");
    }

    private void OnEnable()
    {
        repositoryManager = new RepositoryManager(new CommandRunner());
    }

    private void OnGUI()
    {

        GUILayout.Label("GitHub Repository Creator", EditorStyles.boldLabel);

        // GithubのAPIを入手できるサイトに飛ばす
        if (GUILayout.Button("Get Your GitHub API"))
        {
            if (Event.current.type == EventType.Repaint)
                dropDownButtonRect = GUILayoutUtility.GetLastRect();
            SetGitHubUserSettings.ShowWindow(dropDownButtonRect);
        }

        // ユーザー入力フィールド
        userName = EditorGUILayout.TextField("User Name", userName);
        token = EditorGUILayout.PasswordField("Access Token", token);
        repositoryName = EditorGUILayout.TextField("Repository Name", repositoryName);
        description = EditorGUILayout.TextField("Description", description);
        isPublishing = EditorGUILayout.Toggle("Publish", isPublishing);

        //ローカルレポジトリのディレクトリ選択
        if (GUILayout.Button("Select Local Repository Path"))
        {
            // フォルダ選択ダイアログを表示
            string folderPath = EditorUtility.OpenFolderPanel("Select Local Repository Folder", "", "");
            // フォルダが選択された場合
            if (!string.IsNullOrEmpty(folderPath))
            {
                // パスを表示
                repositoryManager.LocalRepoPath = folderPath;
                Debug.Log("Selected folder: " + folderPath);
            }
        }

        // 選択したローカルリポジトリのパスを表示
        EditorGUILayout.LabelField("Local Repository Path:", repositoryManager.LocalRepoPath);

        // ボタン押下時にリポジトリを作成
        if (GUILayout.Button("Create & Set Repository"))
        {
            TokenManager.SaveToken(token);
            // レポジトリ設定を作成
            var repositorySettings = new RepositorySettings
            {
                name = repositoryName,
                description = description,
                @private = !isPublishing
            };
            CreateAndSetRepository(repositorySettings, userName, repositoryName);
        }

        // GitHubDesktopの起動
        if (GUILayout.Button("Open GitHubDesktop"))
        {
            repositoryManager.OpenGitHubDesktop();
        }
    }

    /// <summary>
    /// レポジトリの作成と初期化
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="userName"></param>
    /// <param name="repoName"></param>
    private async void CreateAndSetRepository(RepositorySettings settings, string userName, string repoName)
    {
        await repositoryManager.CreateRepository(settings);

        repositoryManager.SetRepository(userName, repoName);
    }
}
