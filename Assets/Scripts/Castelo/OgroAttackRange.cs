using UnityEngine;

public class OgroAttackRange : MonoBehaviour
{
    [SerializeField] private Ogro ogro;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ogro.EntrarRangeAtaque();
            // Debug.Log("Entrou attack");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ogro.SairRangeAtaque();
            //Debug.Log("Saiu attack");
        }
    }
}