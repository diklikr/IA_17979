using UnityEngine;

public class UtilityEnemyCtrl : MonoBehaviour, IEnemy
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
        float patrolScore = EvaluatePatrol();
        float chaseScore = EvaluateChase();
        float fleeScore = EvaluateFlee();

        Debug.Log($"Patrol: {patrolScore} | Chase: {chaseScore} | Flee: {fleeScore}");

        float maxScore = Mathf.Max( patrolScore, chaseScore, fleeScore );

        if (maxScore == fleeScore)
        {
            Flee();
        }
        else if (maxScore == chaseScore)
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

    float GetNormalizedDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return Mathf.Clamp01(distance/detectRange);
    }

    float EvaluatePatrol()
    {
        float distance = GetNormalizedDistance();

        return distance;
    }

    float EvaluateChase()
    {
        float distance = GetNormalizedDistance();
        float healthFactor = health / 100f;

        return (1-distance)*healthFactor;
    }

    float EvaluateFlee()
    {
        float distance = GetNormalizedDistance();
        float healthFactor = 1 - (health / 100f);

        return healthFactor * (1 - distance);
    }

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
