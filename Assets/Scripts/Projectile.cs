using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class Projectile : MonoBehaviour {

    [Tooltip ("The target object / marker in the level.")]
    public Transform m_target;
    [Tooltip("The plane the projectile is travelling on.")]
    public Collider m_plane;

    // The distance to the target.
    private float m_distance;
    // The friction coefficient on the plane.  The projectile object does not have any friction on it.
    private float m_frictionCoefficient;
    // The velocity that the projectile going at when the simulation starts.
    private float m_finalVelocity;

    // Reference to the rigidbody.
    private Rigidbody m_rb;

	void Start () 
    {
        // Get the reference to the rigidbody.
        m_rb = GetComponent<Rigidbody>();

        // Calculate the distance between the 2 projectile and the target.
        m_distance = GetDistance(transform, m_target) - 1; // Minus 1 meter between the 2 objects.
        // Get the friction coeficient from the plane.
        m_frictionCoefficient = m_plane.material.dynamicFriction;
        // Calculate  the velocity the set the rigidbody to.
        m_finalVelocity = CalculateFinalVelocity(m_distance, Physics.gravity.y, m_frictionCoefficient);

        Debug.Log("Distance: " + m_distance);
        Debug.Log("Friction: " + m_frictionCoefficient);
        Debug.Log("Final Velocity: " + m_finalVelocity);
	}
	
	void FixedUpdate () 
    {
	    if(Input.GetKeyDown(KeyCode.Space)) // Start the simulation.
        {
            m_rb.velocity = Vector3.right * m_finalVelocity;    // Set the velocity.
        }
	}

    /// <summary>
    /// Calculate the velocity to set the rigidbody to at the begining of the simulation.  Returns 0 if either gravity or the friction is 0.
    /// </summary>
    /// <param name="distance">The distance to travel.</param>
    /// <param name="gravity">Gravity.</param>
    /// <param name="friction">The friction coefficient of the plane the projectile is traveling on.</param>
    /// <returns>Returns the final velocity. (float)</returns>
    private float CalculateFinalVelocity(float distance, float gravity, float friction)
    {
        // Serves as the Initial velocity for stopping, but also the final velocity for slowing down.
        //
        // Use the stopping distance formlua to get the initial velocity of the projectile.
        // d = v^2 / (2*u*g) // NOTE: u and g != 0
        // v = +-(_/2*_/d*_/g*_/u) // NOTE: _/ is square root
        if (gravity != 0 || friction != 0)
        {
            return Mathf.Abs(Mathf.Sqrt(2)) * Mathf.Abs(Mathf.Sqrt(distance)) * Mathf.Abs(Mathf.Sqrt(Mathf.Abs(gravity))) * Mathf.Abs(Mathf.Sqrt(friction));
        }
        else
        {
            return 0.0f;
        }
    }

    /// <summary>
    /// Calculates the X distance between 2 objects.  Takes into account the scale of each object (if traveling from left to right).
    /// </summary>
    /// <param name="pointA">The first object.</param>
    /// <param name="pointB">The second object.</param>
    /// <returns>Returns the x distance between the 2 passed in objects.</returns>
    private float GetDistance(Transform pointA, Transform pointB)
    {
        // Start with 0 vectors.
        Vector3 A = Vector3.zero;
        Vector3 B = Vector3.zero;

        // Calculate and pass in the x positions.
        A.x = pointA.position.x + pointA.localScale.x * 0.5f;
        B.x = pointB.position.x - pointB.localScale.x * 0.5f;

        // Calculate and return the distance.
        return Vector3.Distance(A, B);
    }
}
