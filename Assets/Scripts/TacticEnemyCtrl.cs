using UnityEngine;
using UnityEngine.AI;
using Panda;

public class TacticEnemyCtrl : MonoBehaviour, IEnemy
{
    public Transform player;
    public float health;

    public NavMeshAgent agent;
    public TTactics tactics;

    public float lowHealthThresh = 30f;

    [Task]
    public bool isLowHealth()
    { return health < lowHealthThresh; }

    [Task]
    public void MoveToAttackPoint()
    {
        Transform point = tactics.GetBestPoint(TAState.Attack);
        if (point != null)
        {
            Task.current.Fail();
        }

        agent.SetDestination(point.position);
        if (!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            Task.current.Succeed();
        }

    }
    [Task]
    public void MoveToSafePoint()
    {
        Transform point = tactics.GetBestPoint(TAState.Flee);
        if(point != null) { Task.current.Fail(); }
        agent.SetDestination(point.position);
        if(!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            Task.current.Succeed();
        }
    }
    [Task]
    public void Attack()
    {
        Debug.Log("Atacando");
        Task.current.Succeed();
    }
    [Task]
    public void Idle()
    {
        Task.current.Succeed();
    }
}
