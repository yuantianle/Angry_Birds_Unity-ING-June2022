using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //strip
    public Transform m_idle_position;

    public LineRenderer[] m_line_renderers;
    public Transform[] m_strip_positions;

    public Vector3 m_current_position; //the position of the end of the strip

    private bool m_isMouseDown = false;

    void Start()
    {
        m_line_renderers[0].positionCount = 2;
        m_line_renderers[1].positionCount = 2;
        m_line_renderers[0].SetPosition(0, m_strip_positions[0].position);
        m_line_renderers[1].SetPosition(0, m_strip_positions[1].position);
    }

    void Update()
    {
        if (m_isMouseDown)
        {
            Vector3 mouse_position = Input.mousePosition;
            m_current_position = Camera.main.ScreenToWorldPoint(mouse_position);
            m_current_position += new Vector3(0, 0, 10);

            SetStrips(m_current_position);
        }
        else 
        {
            ResetStrips();
        }
    }
    
    //for the strip control
    public void ResetStrips()
    {
        m_current_position = m_idle_position.position;
        SetStrips(m_current_position);
    }

    void SetStrips(Vector3 position)
    {
    
        m_line_renderers[0].SetPosition(1, position);
        m_line_renderers[1].SetPosition(1, position);
    
    }

    private void OnMouseDown()
    {
        m_isMouseDown = true;
    }
    private void OnMouseUp()
    {
        m_isMouseDown = false;
        ResetStrips();
    }










}
