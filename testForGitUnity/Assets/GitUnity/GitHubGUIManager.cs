using UnityEditor;
using UnityEngine;

public class GitHubGUIManager : EditorWindow
{
    private RepositoryManager repositoryManager;
    private Rect dropDownButtonRect;
    private string repositoryName = "New_Repository"; // 作成するリポジトリ名
    private string description = "Description"; // 作成するレポジトリの説明
    private bool isPublishing; // レポジトリの公開設定
    private string helpMessage;

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
        if (GUILayout.Button("User Settings"))
        {
            if (Event.current.type == EventType.Repaint)
                dropDownButtonRect = GUILayoutUtility.GetLastRect();
            UserSettingManager.ShowWindow(dropDownButtonRect);
        }

        // ユーザー入力フィールド
        repositoryName = EditorGUILayout.TextField("Repository Name", repositoryName);
        description = EditorGUILayout.TextField("Description", description);
        isPublishing = EditorGUILayout.Toggle("Publish", isPublishing);

        // ボタン押下時にリポジトリを作成
        if (GUILayout.Button("Create Repository"))
        {
            // レポジトリ設定を作成
            var repositorySettings = new RepositorySettings
            {
                name = repositoryName,
                description = description,
                @private = !isPublishing
            };
            CreateRepository(repositorySettings);
        }

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

        if (GUILayout.Button("Initialize Local Repository"))
        {
            repositoryManager.InitLocalRepo(repositoryName);
        }

        // GitHubDesktopの起動
        if (GUILayout.Button("Open GitHubDesktop"))
        {
            repositoryManager.OpenGitHubDesktop();
        }

        EditorGUILayout.HelpBox(helpMessage, MessageType.Info);
    }

    /// <summary>
    /// レポジトリの作成と初期化
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="userName"></param>
    /// <param name="repoName"></param>
    private async void CreateRepository(RepositorySettings settings)
    {
        helpMessage = await repositoryManager.CreateRepository(settings);

        Application.OpenURL($"https://github.com/{TokenManager.GetToken().Split('%')[0]}/{repositoryName}");
    }
}
