using UnityEngine;

public class LoboAttackRange : MonoBehaviour
{
    [SerializeField] private Lobo lobo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lobo.EntrarRangeAtaque();
            // Debug.Log("Entrou attack");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lobo.SairRangeAtaque();
            //Debug.Log("Saiu attack");
        }
    }
}