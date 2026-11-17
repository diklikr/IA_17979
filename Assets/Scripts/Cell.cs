using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    void Start()
    {
        
    }

    public void SetState(bool alive)
    {
        isAlive = alive;
        rend.material.color = alive ? Color.white : Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
