using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void IrParaMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void IrParaFinal()
    {
        SceneManager.LoadScene("Final");
    }

    public void IrParaVilarejo()
    {
        // Se estiver indo do Menu para Vilarejo
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            PlayerPrefs.SetInt("PlayerLeitores", 0);
        }

        SceneManager.LoadScene("Vilarejo");
    }

    public void SairJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
