using UnityEngine;

public class PlayerFollower : MonoBehaviour
{

	public Rigidbody PlayerTarget;



	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		transform.position = PlayerTarget.transform.position + 
			Vector3.up*(PlayerTarget.velocity.magnitude*0.2f + 5f);
	}
}
