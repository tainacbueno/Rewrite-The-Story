using UnityEngine;

public class OgroDetectionRange : MonoBehaviour
{
    [SerializeField] private Ogro ogro;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("DETECTION ENTER: " + other.name + " tag=" + other.tag);

        if (other.CompareTag("Player"))
        {
            ogro.EntrarRangeDeteccao(other);
            // Debug.Log("Entrou detection");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log("DETECTION EXIT: " + other.name + " tag=" + other.tag);

        if (other.CompareTag("Player"))
        {
            ogro.SairRangeDeteccao(other);
            // Debug.Log("Saiu detection");
        }
    }
}