using UnityEngine;

public class Hp : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currHealth;
    void Start()
    {
        Restart();
    }
    
    public void TakeDamage(float damage)
    {
        currHealth = currHealth - damage;
        if(currHealth <= 0)
        {
            //die
        }
    }

    public void Restart()
    {
        currHealth = maxHealth;
    }
}
