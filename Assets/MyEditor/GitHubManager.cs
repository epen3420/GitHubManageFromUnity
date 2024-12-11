using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Diagnostics;

public class GitHubManager : EditorWindow
{
    [MenuItem("File/GitHubManager")]
    private static void ShowWindow()
    {
        var window = GetWindow<GitHubManager>("GitHub Manager");
        window.titleContent = new GUIContent("GitHub");
        window.Show();
    }

    [SerializeField]
    private VisualTreeAsset visualTreeAsset;
    [SerializeField]
    private StyleSheet styleSheet;



    private void CreateGUI()
    {
        visualTreeAsset.CloneTree(rootVisualElement);
        rootVisualElement.styleSheets.Add(styleSheet);

        var repoSettings = CreateButton(rootVisualElement);

        string localRepoDir = SelectFolderButton(rootVisualElement);

        ConnectButton(rootVisualElement, localRepoDir);
        ConnectLocalToRemoteRepo(localRepoDir, repoSettings);
    }

    private RepositorySettings CreateButton(VisualElement visualElement)
    {
        var createButton = visualElement.Q<Button>("Create");
        var repositorySetting = new RepositorySettings();
        createButton.clicked += () =>
        {
            // フィールドから値を取得
            string accessToken = visualElement.Q<TextField>("AccessToken").value;
            repositorySetting = new RepositorySettings
            {
                name = visualElement.Q<TextField>("RepositoryName").value,
                description = visualElement.Q<TextField>("Description").value,
                @private = visualElement.Q<Toggle>("Private").value
            };

            // 入力チェック
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(repositorySetting.name))
            {
                UnityEngine.Debug.LogError("Access Token and Repository Name are required.");
                return;
            }

            // コマンドを実行
            CreateRepository(accessToken, repositorySetting);
        };
        return repositorySetting;
    }

    private void CreateRepository(string accessToken, RepositorySettings settings)
    {
        string json = JsonUtility.ToJson(settings);

        json = json.Replace("\"", "\\\"");

        // curlコマンドの作成
        string command = "curl -s -X POST https://api.github.com/user/repos " +
                         $"-H \"Authorization: token {accessToken}\" " +
                         "-H \"Content-Type: application/json\" " +
                         $"-d \"{json}\"";

        RunCommand(command);
    }

    private string SelectFolderButton(VisualElement visualElement)
    {
        var selectFolderButton = visualElement.Q<Button>("SelectFolder");
        string folderPath = null;
        selectFolderButton.clicked += () =>
        {
            folderPath = EditorUtility.OpenFolderPanel("select", "", "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                UnityEngine.Debug.Log("Selected folder: " + folderPath);
            }
        };
        return folderPath;
    }

    private void SetLocalRepository(string localRepoPath, string user, string repoName)
    {
        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(repoName))
        {
            UnityEngine.Debug.LogError("UserName and Repository Name are required.");
            return;
        }

        string connectRemoteRepositoryCommand = $"git remote add origin https://github.com/{user}/{repoName}.git";

        RunCommand(connectRemoteRepositoryCommand);
    }

    private void RunCommand(string command)
    {
        // プロセス設定
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe", // コマンドプロンプトを使用
            Arguments = $"/c {command}", // コマンド実行
            RedirectStandardOutput = true, // 標準出力をリダイレクト
            RedirectStandardError = true, // 標準エラーもリダイレクト
            UseShellExecute = false, // シェル実行を無効に
            CreateNoWindow = true // ウィンドウ非表示
        };

        Process process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();

        // 標準出力と標準エラーの読み取り
        string output = process.StandardOutput.ReadToEnd();
        string errorOutput = process.StandardError.ReadToEnd();

        process.WaitForExit();
        process.Close();


        // コマンドと結果をログに表示
        UnityEngine.Debug.Log("Command: " + command);
        UnityEngine.Debug.Log("Output: " + output);

        if (!string.IsNullOrEmpty(errorOutput))
        {
            UnityEngine.Debug.LogError("Error: " + errorOutput);
        }
    }

    private void ConnectButton(VisualElement visualElement, string folderPath, RepositorySettings repoSettings)
    {
        var connectButton = visualElement.Q<Button>("Connect");
        connectButton.clicked += () =>
        {
            string userName = visualElement.Q<TextField>("RepositoryName").value;
            if (string.IsNullOrEmpty(userName))
            {
                UnityEngine.Debug.LogError("UserName is required.");
                return;
            }
            ConnectLocalToRemoteRepo(folderPath, userName, repoSettings);
        };
    }

    private void ConnectLocalToRemoteRepo(string folderPath, string userName, RepositorySettings repoSettings)
    {
        string command = $"cd {folderPath}";

        RunCommand(command);

        string command2 = $"git remote add origin https://github.com/{userName}/{repoSettings.name}.git";
    }
}

[Serializable]
public class RepositorySettings
{
    public string name;
    public string description;
    public string gitignore_template = "Unity";
    public bool @private;
}
