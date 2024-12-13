using UnityEditor;
using UnityEngine;

public class UserSettingManager : EditorWindow
{
    public static void ShowWindow(Rect buttonRect)
    {
        // ツールウィンドウの作成と表示
        var window = CreateInstance<UserSettingManager>();
        window.ShowAuxWindow();
    }

    private string userName = "User Name";
    private string token = "";


    private void OnEnable()
    {
        // 起動時にすでにトークンが保存してあれば表示する
        string value = TokenManager.GetToken();
        if (value != null && value.Contains('%'))
        {
            var values = value.Split('%');
            userName = values[0];
            token = values[1];
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("User Remote Repository Settings", EditorStyles.boldLabel);

        // GithubのAPIを入手できるサイトに飛ばす
        if (GUILayout.Button("Get Your API"))
        {
            Application.OpenURL("https://github.com/settings/tokens");
        }

        // ユーザー入力フィールド
        userName = EditorGUILayout.TextField("User Name", userName);
        token = EditorGUILayout.PasswordField("Access Token", token);

        // アクセストークンの保存
        if (GUILayout.Button("Save"))
        {
            if (!(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token)))
            {
                TokenManager.SaveToken(userName + '%' + token);
                this.Close();
            }
            else
            {
                Debug.LogError("User Name and Access Token are required");
            }
        }
    }
}
