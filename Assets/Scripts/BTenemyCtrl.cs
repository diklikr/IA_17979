using UnityEngine;
using Panda;

public class BTenemyCtrl : MonoBehaviour
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

    }

    [Task]
    bool isLowHealth()
    {
        return health <= fleeHP;
    }

    [Task]
    bool isPlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance < detectRange;
    }

    [Task]
    void Patrol()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        Task.current.Succeed();
    }

    [Task]
    void Chase()
    {
        move.MoveToward(player.position);
        Task.current.Succeed();
    }

    [Task]
    void Flee()
    {
        move.MoveAway(player.position);
        Task.current.Succeed();
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
