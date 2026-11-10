using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CordEnemyCtrl : MonoBehaviour
{
 public enum EnemyBehavior
    {
        Attacker, Surrounder, Waiter, Follow
    }

    public Transform player;
    public float speed = 4f;

    public float seperatRadius = 3f;
    public float seperatForce = 3f;

    public int enemyIndex;
    public int totalEnemy = 4;
    public float surroundRadius = 4f;

    public float stopDistance = 0.5f;
    public EnemyBehavior behavior;
    public float preferredDistance = 6f;

     Vector3 GetSurroundPosition()
    {
        float angle = (360f / totalEnemy) * enemyIndex;
        float radians = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)); 

        return player.position + offset;
    }

    void FollowBehavior()
    {
        Vector3 targetPosition = GetSurroundPosition();

        if (targetPosition.magnitude > stopDistance)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            Vector3 seperation = Separet() * seperatForce;
            Vector3 finalDirection = direction + seperation;

            finalDirection.y = 0;
            transform.position += finalDirection.normalized * speed * Time.deltaTime;
        }
    }

    Vector3 Separet()
    {
        Vector3 force = Vector3.zero;
        Collider[] neighbors = Physics.OverlapSphere(transform.position, seperatRadius);

        foreach (Collider col in neighbors)
        {
            if (col.gameObject != gameObject && col.CompareTag("Enemy"))
            {
                Vector3 away = transform.position - col.transform.position;

                float strength = Mathf.Clamp01((seperatRadius - away.magnitude) / seperatRadius);
                force += away.normalized * strength;
            }

        }
        return force;
    }

    void SurroundBehavior()
    {
        Vector3 targetPosition = GetSurroundPosition();

        Vector3 toTarget = targetPosition - transform.position;

        if (toTarget.magnitude > stopDistance)
        {
            Vector3 direction = toTarget.normalized;

            Vector3 seperation = Separet() * seperatForce;
            Vector3 finalDirection = direction + seperation;

            finalDirection.y = 0;
            Vector3.ClampMagnitude(finalDirection, 1f);
            transform.position += finalDirection.normalized * speed * Time.deltaTime;
        }
    }

    void AttackBehavior()
    {
            Vector3 direction = (player.position - transform.position).normalized;

            Vector3 seperation = Separet() * seperatForce;
            Vector3 finalDirection = direction + seperation;
            finalDirection.y = 0;
            Vector3.ClampMagnitude(finalDirection, 1f);

            transform.position += finalDirection.normalized * speed * Time.deltaTime;

    }

    void WaitBehavior()
    {
        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        Vector3 direction = Vector3.zero;

        if (distance < preferredDistance-1f)
        {
            direction = -toPlayer.normalized;
        }
        else if(distance > preferredDistance + 1f)
        {
            direction = toPlayer.normalized;
        }
        else
        {
            return;
        }

        Vector3 seperation = Separet() * seperatForce;
        Vector3 finalDirection = direction + seperation;

        finalDirection.y = 0;
        Vector3.ClampMagnitude(finalDirection, 1f);
        transform.position += finalDirection.normalized * speed * Time.deltaTime;
    }

    private void Update()
    {
        switch (behavior)
        {
            case EnemyBehavior.Attacker:
                AttackBehavior();
                break;
            case EnemyBehavior.Waiter:
                WaitBehavior();
                break;
            case EnemyBehavior.Surrounder:
                SurroundBehavior();
                break;
            case EnemyBehavior.Follow:
                FollowBehavior();
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //separacion
        Gizmos.DrawWireSphere(transform.position, seperatRadius);

        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.position);

        if (behavior == EnemyBehavior.Surrounder || behavior == EnemyBehavior.Follow)
        {
            Vector3 surroundPos = GetSurroundPosition();

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(surroundPos, 0.3f);

            Gizmos.DrawLine(transform.position, surroundPos);
        }

        if (behavior == EnemyBehavior.Waiter)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, preferredDistance);
        }
    }

}
