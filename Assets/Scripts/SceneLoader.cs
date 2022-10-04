using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
