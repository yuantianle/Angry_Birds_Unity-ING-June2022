using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoints : MonoBehaviour
{
    public GameObject[] m_path_templates;

    public static PathPoints m_instance;

    public List<GameObject> m_last_points;

    public float m_time_interval;

    int m_last_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;
        m_last_points = new List<GameObject>();
    }

    public void CreateCurrentPathPoint(Vector3 position)
    {
        GameObject point = Instantiate(m_path_templates[m_last_index], position, Quaternion.identity, transform);
        point.SetActive(true);
        m_last_points.Add(point);

        m_last_index++;

        if (m_last_index == m_path_templates.Length)
            m_last_index = 0;
    }

    public void Clear()
    {
        m_last_points.ForEach((obj) => Destroy(obj));
        m_last_points.Clear();
        m_last_index = 0;
    }

}
