using UnityEngine;

public class BayCtrl : MonoBehaviour, IEnemy
{

    public float health = 100;
    public Transform player;

    public float visionRange = 10f;
    public float priorAttackProb = 0.5f;
    public float attackProb;

    private float distanceToTarget;
    private bool playerVisable;

    private MoveCtrl moveCtrl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveCtrl = GetComponent<MoveCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        ObservePlayer();

        CalculateBayInferance();

        DecideAction();
    }

    void ObservePlayer()
    {
        distanceToTarget = Vector3.Distance(transform.position, player.position);

        playerVisable = distanceToTarget <= visionRange;
    }

    void CalculateBayInferance()
    {
        float pDistanceGiveAttack;
        float pVisibleGivenAttack;
        float pHealthGivenAttack;

        if(distanceToTarget < 4f)
        {
            pDistanceGiveAttack = 0.8f;
        }
        else if(distanceToTarget < 8f)
        {
            pDistanceGiveAttack = 0.5f;
        }
        else { pDistanceGiveAttack = 0.2f; }

        pVisibleGivenAttack = playerVisable ? 0.7f : 0.2f;

        if (health < 30f)
        {
            pHealthGivenAttack = 0.3f;
        }
        else { pHealthGivenAttack = 0.7f; }

        attackProb = priorAttackProb * pDistanceGiveAttack * pHealthGivenAttack * pVisibleGivenAttack;
            priorAttackProb = Mathf.Clamp01(attackProb*3f);
    }

    void DecideAction()
    {
        if(attackProb > 0.65f)
        {
            Debug.Log("Attack");
        }
        else if(attackProb > 0.35f)
        {
            moveCtrl.MoveToward(player.position);
        }
        else
        {
            moveCtrl.MoveAway(player.position);
        }
    }

    public void setHealth(float health)
    {
        this.health = health;
    }
}
