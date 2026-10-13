using UnityEngine;

public class EnemyCtrl : MonoBehaviour, IEnemy
{
    public Transform player;
    public float detectRange = 10f;
    public float fleeRange = 3f;

    public float health = 100f;
    public float fleeHP = 30f;

    private MoveCtrl move;

    private void Start()
    {
        move = GetComponent<MoveCtrl>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (health < fleeHP && distance < detectRange)
        {
            Flee();
            Debug.Log(health);
        }
        else if( distance < detectRange)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    public void setHealth(float health)
        { this.health -= health; }

    void Patrol()
    {

    }

    void Chase()
    {
        move.MoveToward(player.position);
    }

    void Flee()
    {
        move.MoveAway(player.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);

    }
}
