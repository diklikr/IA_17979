using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class TCoverPoint : MonoBehaviour
{
    public float coverValue;
    public Transform player;
    public float maxCheckDistance = 50f;

    private void Update()
    {
        EvaluateCover();
    }

    void EvaluateCover()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, direction, out hit, maxCheckDistance))
        {
            if(hit.transform == player)
            {
                coverValue = 0;
            }
            else
            {
                coverValue = 1;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(player == null) return;
        Gizmos.color = (coverValue > 0.5f) ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, player.position);
    }
}
