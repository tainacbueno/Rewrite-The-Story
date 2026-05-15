using UnityEngine;

public class DragaoAttackRange : MonoBehaviour
{
    [SerializeField] private Dragao dragao;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dragao.EntrarRangeAtaque();
            // Debug.Log("Entrou attack");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dragao.SairRangeAtaque();
            //Debug.Log("Saiu attack");
        }
    }
}