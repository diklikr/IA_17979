using UnityEngine;
   public enum States_enemy1
    {
        Chase,
        Flee,
        Attack,
        Idle
    }
public class FSM_enemy1 : MonoBehaviour
{
    public States_enemy1 currentState = States_enemy1.Idle;

    public void Update()
    {
        switch (currentState)
        {
            case States_enemy1.Idle:
                
                break;
            case States_enemy1.Chase:
                
                break;
            case States_enemy1.Attack:
               
                break;
            case States_enemy1.Flee:
               
                break;
        }
    }
    public void ChangeState(States_enemy1 newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

}
