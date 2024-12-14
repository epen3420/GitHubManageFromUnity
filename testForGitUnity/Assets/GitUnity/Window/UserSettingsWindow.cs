using UnityEditor;
using UnityEngine;

namespace GitUnity.Editor
{
    public class UserSettingsWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            // ツールウィンドウの作成と表示
            var window = CreateInstance<UserSettingsWindow>();
            window.ShowAuxWindow();
            window.titleContent = new GUIContent("User Settings");
        }


        private string userName = "User Name";
        private string accessToken = "";
        private float windowWidth = 450;
        private float windowHeight = 200;

        private void OnEnable()
        {
            var newPosition = new Rect(position.x, position.y, windowWidth, windowHeight);
            position = newPosition;

            // 起動時にすでにトークンが保存してあれば表示する
            string tokenValue = TokenManager.GetToken();
            if (tokenValue != null && tokenValue.Contains('%'))
            {
                var tokens = tokenValue.Split('%');
                userName = tokens[0];
                accessToken = tokens[1];
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("UserSettings", EditorStyles.boldLabel);

            // GithubのAPIを入手できるサイトに飛ばす
            if (GUILayout.Button("Get Your GitHub API"))
            {
                Application.OpenURL("https://github.com/settings/tokens");
            }

            // ユーザー入力フィールド
            userName = EditorGUILayout.TextField("User Name", userName);
            accessToken = EditorGUILayout.PasswordField("Access Token", accessToken);

            // アクセストークンの保存
            if (GUILayout.Button("Save"))
            {
                bool isUserNameValid = !string.IsNullOrEmpty(userName);
                bool isAccessTokenValid = !string.IsNullOrEmpty(accessToken) && accessToken.Contains("ghp_");

                if (isUserNameValid && isAccessTokenValid)
                {
                    TokenManager.SaveToken($"{userName}%{accessToken}");
                    this.Close();
                }
                else
                {
                    LogUtility.LogError("Both User Name and a valid Access Token are required.");
                }
            }
        }
    }
}
