using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RepositoryManager
{
    private CommandRunner commandRunner;
    public RepositoryManager(CommandRunner commandRunner)
    {
        this.commandRunner = commandRunner;
    }
    private string localRepoPath;
    public string LocalRepoPath
    {
        get { return localRepoPath; }
        set
        {
            localRepoPath = value;
        }
    }


    /// <summary>
    /// Github上にレポジトリの作成
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public async Task<string> CreateRepository(RepositorySettings settings)
    {
        // GitHub APIエンドポイント
        string url = "https://api.github.com/user/repos";

        // JSONデータを作成
        string json = JsonUtility.ToJson(settings);

        try
        {
            using (var httpClient = new HttpClient())
            {
                // 必要なヘッダーを設定
                httpClient.DefaultRequestHeaders.Add("Authorization", $"token {TokenManager.GetToken().Split('%')[1]}");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "UnityEditorGitHubAPI");

                // リクエストコンテンツを準備
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // POSTリクエストを送信
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return "Repository created successfully!";
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return $"Failed to create repository: {response.StatusCode} - {error}";
                }
            }
        }
        catch (System.Exception ex)
        {
            return $"Error creating repository: {ex.Message}";
        }
    }

    /// <summary>
    /// フォルダパスの初期化
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="repoName"></param>
    public void InitLocalRepo(string repoName)
    {
        string value = TokenManager.GetToken();
        string[] values = value.Split('%');
        string changeDir = $"cd {localRepoPath}";
        string cloneRepo = $"git clone https://github.com/{values[0]}/{repoName}.git {localRepoPath}";

        commandRunner.RunCommand($"{changeDir} & {cloneRepo}");
    }

    /// <summary>
    /// GitHubDesktopの起動
    /// </summary>
    public void OpenGitHubDesktop()
    {
        string openCommand = $"github {localRepoPath}";
        commandRunner.RunCommand(openCommand);
    }
}
