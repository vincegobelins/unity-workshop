using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    
    Rigidbody2D playerBody;
    Animator animator;

    // Inputs
    bool _keyRightPressed;
    bool _keyLeftPressed;
    bool _keySpacePressed;
    bool _keySpaceRemoved;

    // Character state
    const uint STATE_IDLE = 0;
    const uint STATE_WALK = 1;
    const uint STATE_JUMP = 2;
    const uint STATE_FALLING = 3;
    const uint STATE_HURT = 4;

    // Public value
    public float walkSpeed = 0;
    public Text text;

    // Temp variables
    string _currentDirection = "right";
    uint _currentAnimationState = STATE_IDLE;
    int score = 0;

    bool _isGrounded = false;
    bool _isJumping = false;
    bool _isFalling = false;
    bool _isHurted = false;

    void Start () {
        animator = this.GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody2D>();
        //text = GetComponent<Text>();
    }

    //*********************************
    //  Register events             
    //*********************************

    void Update()
    {
        _keyRightPressed = Input.GetKey(KeyCode.RightArrow);
        _keyLeftPressed = Input.GetKey(KeyCode.LeftArrow);
        _keySpacePressed = Input.GetKey(KeyCode.Space);
        _keySpaceRemoved = Input.GetKeyUp(KeyCode.Space);

    }

    //*********************************
    //  Update physics             
    //*********************************

    void FixedUpdate()
    {

        // Give velocity 
        Vector2 tmpVelocity = playerBody.velocity;

        if(!_isHurted)
            tmpVelocity.x = walkSpeed * Time.fixedDeltaTime;
        

        // Deplacement
        if (_keyLeftPressed && !_isJumping && !_isFalling  && !_isHurted)
        {
            changeDirection("left");
            changeState(STATE_WALK);
            changeVelocity();
        }
        else if (_keyRightPressed && !_isJumping && !_isFalling && !_isHurted)
        {
            changeDirection("right");
            changeState(STATE_WALK);
            changeVelocity();
        }
        else if (!_keyLeftPressed && !_keyRightPressed && !_isJumping && !_isFalling)
        {
            walkSpeed = 0;
            changeState(STATE_IDLE);
        }

        // Jump
        if (_keySpacePressed && !_isFalling)
        {
            playerBody.AddForce(new Vector2(0, 50));
            changeState(STATE_JUMP);
            _isGrounded = false;
            _isJumping = true;
            _isFalling = false;
        }
        if (_keySpaceRemoved)
        {
            changeState(STATE_FALLING);
            _isGrounded = false;
            _isJumping = false;
            _isFalling = true;
        }

        playerBody.velocity = tmpVelocity;
    } 

    //*********************************
    //  Collision event              
    //*********************************

    void OnCollisionEnter2D(Collision2D coll)
    {
        _isGrounded = true;
        _isJumping = false;
        _isFalling = false;


        // Kill Enemy
        if (coll.gameObject.tag == "Enemy" && coll.relativeVelocity.y < -1)
        {
            playerBody.AddForce(new Vector2(0, 400));            
        }

        // Kill bonus
        if (coll.gameObject.tag == "Jewel")
        {
            Destroy(coll.gameObject);
            addPoint();
        }

        // Hurted
        if (coll.gameObject.tag == "Enemy" && coll.relativeVelocity.y > -1)
        {
            Debug.Log(coll.relativeVelocity.x);
            if(coll.relativeVelocity.x > 0)
                hurt("right");
            if (coll.relativeVelocity.y < 0)
                hurt("left");
        }
    }

    //*********************************
    //  Called when player is hurted              
    //*********************************

    void hurt(string direction)
    {
        _isHurted = true;
        changeState(STATE_HURT);
        if(direction == "left")
        {
            playerBody.AddForce(new Vector2(600, 300));
        }
        else
        {
            playerBody.AddForce(new Vector2(-600, 300));
        }
        Invoke("reborn", 2);
    }

    //*********************************
    //  Called to reborn the player              
    //*********************************

    void reborn()
    {
        _isHurted = false;
    }

    //*********************************
    //  Changing state              
    //*********************************

    void changeState(uint state)
    {
        if (_currentAnimationState == state)
            return;

        animator.SetInteger("state", (int)state);

        _currentAnimationState = state;
    }

    //*********************************
    //  Called to change direction              
    //*********************************

    void changeDirection(string direction)
    {
        if (_currentDirection != direction)
        {
            if (direction == "right")
                transform.Rotate(0, 180, 0);


            else if (direction == "left")
                transform.Rotate(0, -180, 0);

            _currentDirection = direction;
        }
    }

    //*********************************
    //  Give a velocity to player              
    //*********************************

    void changeVelocity()
    {
        if(_currentDirection == "left")
            walkSpeed = -150;

        else if (_currentDirection == "right")
            walkSpeed = 150;
    }

    //*********************************
    //  Update the score             
    //*********************************

    void addPoint()
    {
        score++;
        text.text = score.ToString();
    }
}
