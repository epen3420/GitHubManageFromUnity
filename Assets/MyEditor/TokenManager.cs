using UnityEditor;

public class TokenManager
{
    private const string TokenKey = "GitHubAccessToken";

    /// <summary>
    /// トークンを保存
    /// </summary>
    /// <param name="token"></param>
    public static void SaveToken(string token)
    {
        EditorPrefs.SetString(TokenKey, token);
    }

    /// <summary>
    /// トークンを取得
    /// </summary>
    /// <returns></returns>
    public static string GetToken()
    {
        return EditorPrefs.HasKey(TokenKey) ? EditorPrefs.GetString(TokenKey) : null;
    }

    /// <summary>
    /// トークンを削除
    /// </summary>
    public static void DeleteToken()
    {
        if (EditorPrefs.HasKey(TokenKey))
        {
            EditorPrefs.DeleteKey(TokenKey);
        }
    }
}
