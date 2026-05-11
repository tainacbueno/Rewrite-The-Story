using UnityEngine;

public class RestaurarPosicaoPlayer : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("RetornarPosicao", 0) == 1)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");

            player.transform.position = new Vector2(x, y);

            PlayerPrefs.SetInt("RetornarPosicao", 0);
        }
    }
}