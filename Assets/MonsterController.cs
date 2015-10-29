using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

    // Character state
    const uint STATE_WALK = 0;
    const uint STATE_DIE = 1;

    // Public value
    public float walkSpeed = 3;

    // Temp variables
    string _currentDirection = "right";
    uint _currentAnimationState = STATE_WALK;

    Rigidbody2D monsterBody;
    Animator animator;

    void Start () {
        animator = this.GetComponent<Animator>();
        monsterBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        // Give velocity
        GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    //*********************************
    //  Collision event              
    //*********************************

    void OnCollisionEnter2D(Collision2D coll)
    { 
       
        Debug.Log("bim");

        if (coll.gameObject.name != "mb")
        {
            if (_currentDirection == "right")
                changeDirection("left");
            else if (_currentDirection == "left")
                changeDirection("right");

            if (coll.relativeVelocity.y > 1)
            {
                selfKill();
            }
        }
        
    }

    //*********************************
    //  Called to kill this game object            
    //*********************************

    void selfKill()
    {
        changeState(STATE_DIE);
        Destroy(gameObject, 0.5f);
        Debug.Log("destroy");
    }


    //*********************************
    //  Change state             
    //*********************************

    void changeState(uint state)
    {
        if (_currentAnimationState == state)
            return;

        animator.SetInteger("stateMonster", (int)state);

        _currentAnimationState = state;
    }


    //*********************************
    //  Change direction              
    //*********************************

    void changeDirection(string direction)
    {
        if (_currentDirection != direction)
        {
            walkSpeed = walkSpeed * -1;

            if (direction == "right")
            {
                transform.Rotate(0, 180, 0);
            }
                

            else if (direction == "left")
            {
                transform.Rotate(0, -180, 0);
            }

            _currentDirection = direction;
        }
    }
}
