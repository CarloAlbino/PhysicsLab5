using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class SlideProjectile : MonoBehaviour {

    public Collider m_floor;
    public Transform m_target;

    private float m_kineticFriction;
    private float m_staticFriction;
    private float m_distance;

    private float m_kineticFrictionForce;
    private float m_staticFrictionForce;

    private float m_acceleration;
    private Rigidbody m_rb;

	void Start () 
    {
        m_rb = GetComponent<Rigidbody>();
        m_kineticFriction = m_floor.material.dynamicFriction;
        m_staticFriction = m_floor.material.staticFriction;
        m_distance = Vector3.Distance(transform.position, m_target.position);

        m_kineticFrictionForce = CalculateOvercomeFrictionForce(m_rb.mass, Physics.gravity.y, m_kineticFriction);
        m_staticFrictionForce = CalculateOvercomeFrictionForce(m_rb.mass, Physics.gravity.y, m_staticFriction);

        m_acceleration = CalculateAcceleration(m_rb.mass, Mathf.Abs(m_kineticFrictionForce/* + m_staticFrictionForce*/), Mathf.Abs(Physics.gravity.y)//);
                            + CalculateAcceleration(m_rb.mass, Mathf.Abs(m_staticFrictionForce), Mathf.Abs(Physics.gravity.y)));
        Debug.Log(m_acceleration);
	}

	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            Slide();
        }
	}

    private void Slide()
    {
        //m_rb.velocity = Vector3.right * CalculateInitialVelocity(m_acceleration, m_distance);
        m_rb.AddForce(m_rb.mass * m_acceleration * Vector3.right, ForceMode.Impulse);
    }

    private float CalculateAcceleration(float mass, float force, float gravity)
    {
        return (mass / force) * gravity;
    }

    private float CalculateOvercomeFrictionForce(float mass, float gravity, float friction)
    {
        return mass * gravity * friction;
    }

    private float CalculateInitialVelocity(float acceleration, float distance)
    {
        return Mathf.Abs(Mathf.Sqrt(2 * acceleration * distance));
    }
}
