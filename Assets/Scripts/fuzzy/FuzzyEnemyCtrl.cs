using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class FuzzyEnemyCtrl : MonoBehaviour, IEnemy
{
    public float health = 100f;
    public Transform player;
    public float detectionRange = 10f;
    public float fleeRange = 3f;

    private MoveCtrl move;

    void Start()
    {
        move = GetComponent<MoveCtrl>();
    }

    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.position);

        float ruleAttackStrong = AND(HighHealth(health), CloseDistance(distance));
        float ruleAttackMed = AND(MediumHP(health), CloseDistance(distance));
        float ruleFlee = AND(LowHealth(health), CloseDistance(distance));
        float rulePatrol = AND(HighHealth(health), FarDistance(distance));

        float attack = (ruleAttackStrong * 1.0f) + (ruleAttackMed * 0.6f);
        float flee = ruleFlee * 1.0f;
        float patrol = rulePatrol * 1.0f;

        if (attack > flee && attack > patrol)
        {
            move.MoveToward(player.position);
        }
        else if (attack < flee && flee > patrol)
        {
            move.MoveAway(player.position);
        }
        else
        {
            return;
        }
    }

    public void setHP(float health)
    {
        this.health = health;
    }

    float AND(float a , float b)
    {
        return Mathf.Min(a, b);
    }

    float OR(float a, float b)
    {
        return Mathf.Max(a,b);
    }

    float NOT(float a)
    {
        return 1 - a;
    }

    float LowHealth(float health)
    {
        if (health <= 30) return 1;
        if (health >= 60) return 0;
        return (60 - health) / 30;
    }

    float MediumHP(float health)
    {
        if (health <= 30f || health >= 70f) return 0;
        if (health == 50f) return 1;
       if (health < 50) return (health - 30) / 20;
        else
        {
            return(70 - health) / 20;
        }
    }

    float HighHealth(float health)
    {
        if (health <= 50f) return 0;
        if (health >= 80f) return 1;
        return (health - 50f) / 30f;
    }

    float CloseDistance(float distance)
    {
        if(distance <= 5) return 1;
        if (distance >= 15) return 0;
        return(15 - distance) / 10f;
    }

    float FarDistance(float distance)
    {
        if (distance <= 10) return 0;
        if (distance >= 20) return 1;
        return (distance - 10f) / 10f;
    }
}
