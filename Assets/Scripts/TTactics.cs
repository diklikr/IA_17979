using System.Drawing;
using UnityEngine;

public class TTactics : MonoBehaviour
{
    public Transform[] points;
    public Transform player;

    [Header("Pesos")]
    public float weightDistance = 1.0f;
    public float weightCover = 1.5f;
    public float weightHeight = 1.0f;

    public Transform GetBestPoint(TAState state)
    {
        Transform bestPoint = null;
        float bestScore = -Mathf.Infinity;

        foreach (Transform point in points)
        {
            float score = EvaluatePoint(point,state);
            if (score > bestScore)
            {
                bestPoint = point;
                bestScore = score;
            }
        }

        return bestPoint;
    }

    public float EvaluatePoint(Transform point, TAState state)
    {
        float distance = Vector3.Distance(point.position, player.position);
        float distanceScore = Mathf.Clamp(10 - distance, 0, 10) / 10;

        float heightDifference = point.position.y - transform.position.y;
        float heightScore = Mathf.Clamp(heightDifference, 0, 10) / 10;

        float coverScore = point.GetComponent<TCoverPoint>().coverValue;
        if (state == TAState.Attack)
        {
            return (weightDistance*(1-distanceScore))+(weightCover*(coverScore*0.5f))+(weightHeight*heightScore);
        }
        else
        {
            return (weightDistance * distanceScore) + (weightCover * coverScore) + (weightHeight * heightScore);
        }
    }
}
