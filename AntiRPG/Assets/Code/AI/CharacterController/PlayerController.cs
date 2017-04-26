using UnityEngine;

/// <summary>
/// Controller for the Player
/// </summary>
public class PlayerController : CharacterController
{

	// Use this for initialization
	void Start()
	{
	    Speed = 3.0f;
	}

	// Update is called once per frame
	void Update()
	{
		MouseInput();
	    MovementInput();
	}

    protected override void Die()
    {
        base.Die();
        Debug.Break();
    }
    /// <summary>
    /// Processes Input that results to character movement
    /// </summary>
    void MovementInput()
    {
        //Get walking direction
        var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            direction += transform.forward;
        if (Input.GetKey(KeyCode.S))
            direction -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            direction += transform.right;
        if (Input.GetKey(KeyCode.A))
            direction -= transform.right;
        //walk
        Move(direction);

        //teleport
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var oldSpeed = Speed;
            Speed = 100.0f;
            Move(direction);
            Speed = oldSpeed;
        }

    }

    /// <summary>
    /// Processes Mouse input eg: Position
    /// </summary>
    void MouseInput()
    {
        //Get Position
        var mousePosition = Input.mousePosition;
        mousePosition.z = mousePosition.y;
        mousePosition.y = 0;
        var screenCenter = new Vector3(Screen.width / 2, 0, Screen.height / 2);

        //Rotate to position
        Rotate(mousePosition - screenCenter);
    }
}
