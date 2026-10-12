using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public enum State { Idle, Move, Jump, Attack, Dash }

    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 7f;
    public float turnSpeed = 15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;

    [Header("Attack")]
    public float attackDuration = 0.4f;

    public State Current { get; private set; }

    Rigidbody rb;
    Vector3 moveDir;
    bool jumpPressed, attackPressed;
    float stateTimer;

    bool Moving => moveDir.sqrMagnitude > 0.0001f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Start() => ChangeState(State.Idle);

    void Update()
    {
        // A/D = X, W/S = Z
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        jumpPressed = Input.GetButtonDown("Jump");
        attackPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        stateTimer += Time.deltaTime;
        UpdateState();
        FaceMoveDirection();
    }

    void FixedUpdate()
    {
        switch (Current)
        {
            case State.Idle:
            case State.Attack:
                SetHorizontal(Vector3.zero);
                break;
            case State.Move:
            case State.Jump:
                SetHorizontal(moveDir * moveSpeed);
                break;
            case State.Dash:
                break;
        }
    }

    void UpdateState()
    {
        switch (Current)
        {
            case State.Idle:
                if (attackPressed) ChangeState(State.Attack);
                else if (jumpPressed && IsGrounded()) ChangeState(State.Jump);
                else if (Moving) ChangeState(State.Move);
                break;

            case State.Move:
                if (attackPressed) ChangeState(State.Attack);
                else if (jumpPressed && IsGrounded()) ChangeState(State.Jump);
                else if (!Moving) ChangeState(State.Idle);
                break;

            case State.Jump:
                if (stateTimer > 0.1f && IsGrounded())
                    ChangeState(Moving ? State.Move : State.Idle);
                break;

            case State.Attack:
                if (stateTimer >= attackDuration)
                    ChangeState(Moving ? State.Move : State.Idle);
                break;
            case State.Dash:
                break;
        }
    }

    void ChangeState(State next)
    {
        OnExit(Current);
        Current = next;
        stateTimer = 0f;
        OnEnter(next);
    }

    void OnEnter(State s)
    {
        switch (s)
        {
            case State.Jump:
                var v = rb.linearVelocity;
                v.y = 0f;
                rb.linearVelocity = v;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                break;
            case State.Attack:
                Debug.Log("Attack!");
                break;
        }
    }

    void OnExit(State s)
    {

    }

    void SetHorizontal(Vector3 horizontal)
    {
        var v = rb.linearVelocity;
        v.x = horizontal.x;
        v.z = horizontal.z;
        rb.linearVelocity = v;
    }

    void FaceMoveDirection()
    {
        if (!Moving || Current == State.Attack) return;
        var target = Quaternion.LookRotation(moveDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, turnSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundRadius, LayerMask.GetMask("Ground"));
    }
}