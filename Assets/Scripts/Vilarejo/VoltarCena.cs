using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarCena : MonoBehaviour
{
    public KeyCode teclaVoltar = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(teclaVoltar)){
            // ativa retorno da posição
            PlayerPrefs.SetInt("RetornarPosicao", 1);

            string ultimaCena = PlayerPrefs.GetString("UltimaCena");

            SceneManager.LoadScene(ultimaCena);
        }
    }
}