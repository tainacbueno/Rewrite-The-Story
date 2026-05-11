using UnityEngine;

public class LoboDetectionRange : MonoBehaviour
{
    [SerializeField] private Lobo lobo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("DETECTION ENTER: " + other.name + " tag=" + other.tag);

        if (other.CompareTag("Player"))
        {
            lobo.EntrarRangeDeteccao(other);
            // Debug.Log("Entrou detection");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log("DETECTION EXIT: " + other.name + " tag=" + other.tag);

        if (other.CompareTag("Player"))
        {
            lobo.SairRangeDeteccao(other);
            // Debug.Log("Saiu detection");
        }
    }
}