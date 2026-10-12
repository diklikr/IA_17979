using UnityEngine;

public enum States_enemy1
{
    Chase,
    Flee,
    Attack,
    Idle
}

[RequireComponent(typeof(Rigidbody))]
public class FSM_enemy1 : MonoBehaviour
{
    public States_enemy1 currentState = States_enemy1.Chase;
    public Transform target;            // drag the player here


    public float chaseSpeed = 3.5f;
    public float fleeSpeed = 5f;
    public float turnSpeed = 10f;

    public float detectRange = 10f;     // Idle -> Chase
    public float loseRange = 14f;       // Chase/Flee -> Idle
    public float stopDistance = 1.5f;   // stop when this close while chasing

    public float health = 100f;
    public float fleeHealth = 25f;      // flee when health is at or below this

    Rigidbody rb;
    Vector3 desiredVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Update()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);
        desiredVelocity = Vector3.zero;

        switch (currentState)
        {
            case States_enemy1.Idle:
                if (health <= fleeHealth && dist < detectRange) ChangeState(States_enemy1.Flee);
                else if (dist < detectRange) ChangeState(States_enemy1.Chase);
                break;

            case States_enemy1.Chase:
                if (health <= fleeHealth) { ChangeState(States_enemy1.Flee); break; }
                if (dist > loseRange) { ChangeState(States_enemy1.Idle); break; }
                if (dist > stopDistance)
                {
                    Vector3 dir = FlatDirection(target.position - transform.position);
                    desiredVelocity = dir * chaseSpeed;
                    Face(dir);
                }
                break;

            case States_enemy1.Attack:

                break;

            case States_enemy1.Flee:
                if (dist > loseRange) { ChangeState(States_enemy1.Idle); break; }
                Vector3 away = FlatDirection(transform.position - target.position);
                desiredVelocity = away * fleeSpeed;
                Face(away);
                break;
        }
    }

    void FixedUpdate()
    {
        var v = rb.linearVelocity;
        v.x = desiredVelocity.x;
        v.z = desiredVelocity.z;
        rb.linearVelocity = v;
    }

    public void ChangeState(States_enemy1 newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    Vector3 FlatDirection(Vector3 v)
    {
        v.y = 0f;
        return v.sqrMagnitude > 0.0001f ? v.normalized : Vector3.zero;
    }

    void Face(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        var look = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
    }
}
