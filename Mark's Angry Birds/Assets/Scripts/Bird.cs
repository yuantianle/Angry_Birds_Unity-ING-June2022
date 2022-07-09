using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //public bool m_collided;
    //
    //public void Release()
    //{
    //    PathPoints.m_instance.Clear();
    //    StartCoroutine(CreatePathPoints());
    //}
    //
    //IEnumerator CreatePathPoints()
    //{
    //    while (true)
    //    {
    //        if (m_collided) break;
    //        PathPoints.m_instance.CreateCurrentPathPoint(transform.position);
    //        yield return new WaitForSeconds(PathPoints.m_instance.m_time_interval);
    //    }
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    m_collided = true;
    //}



    //strip information
    public Transform m_center;

    public float m_max_length;  //threshold of the max length of the strip
    public float m_bottom_boundary; //threshold that strip end will not touch the ground


    //shoot force
    public float m_shoot_force;

    //mouse control
    private bool m_isMouseDown = false;

    //bird
    public GameObject m_bird_prefab;
    Rigidbody2D m_angry_bird;
    Collider2D m_angry_bird_collider;

    private SpringJoint2D m_spring;

    void Start()
    {
        //CreateBird();
    }

    void Update()
    {
        if (m_isMouseDown)
        {
            //change the position of the bird according to the mouse click position
            Vector3 mouse_position = Input.mousePosition;
            transform.position = Camera.main.ScreenToWorldPoint(mouse_position);  
            transform.position += new Vector3(0, 0, 10);

            transform.position = m_center.position
                + Vector3.ClampMagnitude(transform.position - m_center.position, m_max_length);
            transform.position = ClampBoundary(transform.position);


            if (m_angry_bird_collider)
            {
                m_angry_bird_collider.enabled = true;
            }
        }
    }

    //for the strip end to not touch the ground
    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, m_bottom_boundary, 1000);  //ground and sky
        return vector;
    }

    //bird generation
    void CreateBird()
    {
        m_angry_bird = Instantiate(m_bird_prefab).GetComponent<Rigidbody2D>();
        m_angry_bird_collider = m_angry_bird.GetComponent<Collider2D>();
        
        m_angry_bird_collider.enabled = false;
        m_angry_bird.isKinematic = true;
    }

    //shoot force
    void Shoot()
    {
        m_angry_bird.isKinematic = false;
        Vector3 bird_force = (transform.position - m_center.position) * m_shoot_force * -1;
        m_angry_bird.velocity = bird_force;

        m_angry_bird = null;
        m_angry_bird_collider = null;
        Invoke("CreateBird", 2);
    }

    //awake spring
    private void Awake()
    {
        m_spring = GetComponent<SpringJoint2D>(); //get the member's initialized value from GUI
    }

    //for the mouse control
    private void OnMouseDown()
    {
        m_isMouseDown = true;
    }
    private void OnMouseUp()
    {
        m_isMouseDown = false;
        m_spring.enabled = false;
        Shoot();
    }
}
