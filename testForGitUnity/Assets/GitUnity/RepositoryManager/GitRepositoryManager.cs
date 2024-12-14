using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GitUnity.Repository
{
    public class GitRepositoryManager
    {
        private CommandRunner commandRunner;
        public GitRepositoryManager(CommandRunner commandRunner)
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
        private const string GIT_INIT = "git init"; // ローカルレポジトリの初期化
        private const string GIT_FETCH = "git fetch origin"; // 半強制的にmainからpullする
        private const string GIT_CHECKOUT = "git checkout main"; // ローカルのデフォルトブランチ名がmasterだった時用
        private const string OPEN_GITHUBDESKTOP = "github"; // GitHubDesktopを開くコマンド


        /// <summary>
        /// Github上にレポジトリの作成
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<bool> CreateRemoteRepo(RepositorySettings settings)
        {
            // GitHub APIエンドポイント
            string url = "https://api.github.com/user/repos";

            // JSONデータを作成
            string json = JsonUtility.ToJson(settings);

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"token {TokenManager.GetToken().Split('%')[1]}");
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "UnityEditorGitHubAPI");

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        LogUtility.Log($"Repository '{settings.name}' created successfully.");
                        return true;
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        LogUtility.LogError($"Failed to create repository '{settings.name}': {response.StatusCode} - {error}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogError($"Error creating repository '{settings.name}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// リモートレポジトリからクローン
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        public bool InitLocalRepo(string repoName)
        {
            try
            {
                string tokenValue = TokenManager.GetToken();
                string[] tokens = tokenValue.Split('%');
                string remoteRepoUrl = $"https://github.com/{tokens[0]}/{repoName}.git";

                string[] commands =
                {
                    GIT_INIT,
                    $"git remote add origin {remoteRepoUrl}",
                    GIT_FETCH,
                    GIT_CHECKOUT,
                };

                foreach (var command in commands)
                {
                    commandRunner.RunCommand(localRepoPath, command);
                }
                return true;
            }
            catch (Exception ex)
            {
                // エラーハンドリング
                LogUtility.LogError($"Failed to initialize local repository: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// GitHubDesktopの起動
        /// </summary>
        public void OpenGitHubDesktop()
        {
            commandRunner.RunCommand(localRepoPath, OPEN_GITHUBDESKTOP);
        }
    }
}
