using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCController : CharacterController
{

    public List<CharacterController> Enemies;
    protected List<CharacterController> Allies; 

    // Use this for initialization
    void Start () {
	
    }
	
    // Update is called once per frame
    void Update () {
	
    }

    /// <summary>
    /// Detects if an enemy is within the given radius
    /// </summary>
    /// <param name="radius">Within this Radius enemies will be seaked</param>
    /// <returns>True if at least one enemy is found</returns>
    protected bool DetectEnemies(float radius)
    {
        var collisions = Physics.OverlapSphere(transform.position, radius);

        return collisions.Where(collision => collision.GetComponent<CharacterController>() != null)
            .Any(collision => Enemies.Contains(collision.GetComponent<CharacterController>()));
    }

    protected CharacterController GetNearestEnemy(float radius)
    {
        var collisions = Physics.OverlapSphere(transform.position, radius);

        return collisions.Where(collision => collision.GetComponent<CharacterController>() != null)
            .Where(collision => Enemies.Contains(collision.GetComponent<CharacterController>()))
            .OrderBy(collision => Mathf.Abs((collision.gameObject.transform.position - transform.position).magnitude))
            .First().GetComponent<CharacterController>();
    }
    
}

