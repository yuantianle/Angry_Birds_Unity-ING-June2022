using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //strip
    public Transform m_center;
    public Transform m_idle_position;

    public LineRenderer[] m_line_renderers;
    public Transform[] m_strip_positions;

    public Vector3 m_current_position; //the position of the end of the strip
    public float m_max_length;  //threshold of the max length of the strip
    public float m_bottom_boundary; //threshold that strip end will not touch the ground

    //bird
    public GameObject m_bird_prefab;
    public float m_bird_position_offset;

    Rigidbody2D m_angry_bird;
    Collider2D m_angry_bird_collider;

    //shoot force
    public float m_shoot_force;

    //mouse control
    bool m_isMouseDown;

    void Start()
    {
        m_line_renderers[0].positionCount = 2;
        m_line_renderers[1].positionCount = 2;
        m_line_renderers[0].SetPosition(0, m_strip_positions[0].position);
        m_line_renderers[1].SetPosition(0, m_strip_positions[1].position);

        CreateBird();
    }

    void Update()
    {
        if (m_isMouseDown)
        {
            Vector3 mouse_position = Input.mousePosition;
            mouse_position.z = 10;

            m_current_position = Camera.main.ScreenToWorldPoint(mouse_position);
            m_current_position = m_center.position 
                + Vector3.ClampMagnitude(m_current_position - m_center.position, m_max_length);
            m_current_position = ClampBoundary(m_current_position);

            SetStrips(m_current_position);

            if(m_angry_bird_collider)
            {
                m_angry_bird_collider.enabled = true;
            }
        }
        else
        {
            ResetStrips();
        }
    }

    //for the strip control
    void ResetStrips()
    {
        m_current_position = m_idle_position.position;
        SetStrips(m_current_position);
    }

    void SetStrips(Vector3 position)
    {

        m_line_renderers[0].SetPosition(1, position);
        m_line_renderers[1].SetPosition(1, position);

        if (m_angry_bird)
        {
            Vector3 dir = position - m_center.position;
            m_angry_bird.transform.position = position + dir.normalized * m_bird_position_offset;
            m_angry_bird.transform.right = -dir.normalized;
        }
    }

    //for the strip to not touch the ground
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

        ResetStrips();
    }

    //shoot force
    void Shoot()
    {
        m_angry_bird.isKinematic = false;
        Vector3 bird_force = (m_current_position - m_center.position) * m_shoot_force * -1;
        m_angry_bird.velocity = bird_force;

        m_angry_bird = null;
        m_angry_bird_collider = null;
        Invoke("CreateBird", 2);
    }

    //for the mouse control
    private void OnMouseDown()
    {
        m_isMouseDown = true;
    }

    private void OnMouseUp()
    {
        m_isMouseDown = false;
        Shoot();
        m_current_position = m_idle_position.position;
    }


}
