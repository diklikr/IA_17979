using UnityEngine;

public class MoveCtrl : MonoBehaviour
{
    public float speed;
    public void MoveToward(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public void MoveAway(Vector3 target)
    {
        Vector3 dir = (transform.position - target).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
}
