using UnityEngine;

public class DragaoDetectionRange : MonoBehaviour
{
    [SerializeField] private Dragao dragao;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("DETECTION ENTER: " + other.name + " tag=" + other.tag);

        if (other.CompareTag("Player"))
        {
            dragao.EntrarRangeDeteccao(other);
            // Debug.Log("Entrou detection");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log("DETECTION EXIT: " + other.name + " tag=" + other.tag);

        if (other.CompareTag("Player"))
        {
            dragao.SairRangeDeteccao(other);
            // Debug.Log("Saiu detection");
        }
    }
}