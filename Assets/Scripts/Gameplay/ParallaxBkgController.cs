using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBkgController : MonoBehaviour
{
    static ParallaxBkgController s_instance;
    public static ParallaxBkgController Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<ParallaxBkgController>();
            }
            return s_instance;
        }
    }

    float m_cloudPad;
    float m_moutainPad;
    float m_tree1Pad;
    float m_tree2Pad;
    float m_roadPad;

    [SerializeField]
    GameObject m_cloudLayer;

    [SerializeField]
    GameObject m_mountainLayer;

    [SerializeField]
    GameObject m_tree2Layer;

    [SerializeField]
    GameObject m_tree1Layer;

    [SerializeField]
    GameObject m_roadLayer;


    //static readonly int BGK_WIDTH = 1152;
    static readonly int PARALLAX_WEIGHT = 1148;
	    
    // Use this for initialization
    void Start()
    {
        m_cloudPad = 0;
        m_moutainPad = 0;
        m_tree1Pad = 0;
        m_tree2Pad = 0;
        m_roadPad = 0;        
    }

	public void DoScroll(float speed){
		//float speed = m_scrollSpeed * 500 * Time.deltaTime;
		m_cloudPad += speed * 0.1f;
		m_moutainPad += speed * 0.2f;
		m_tree2Pad += speed * 0.9f;
		m_tree1Pad += speed * 1.0f;
		m_roadPad += speed * 1.0f;

		if (m_cloudPad > PARALLAX_WEIGHT)
			m_cloudPad = 0;
		if (m_moutainPad > PARALLAX_WEIGHT)
			m_moutainPad = 0;
		if (m_tree1Pad > PARALLAX_WEIGHT)
			m_tree1Pad = 0;
		if (m_tree2Pad > PARALLAX_WEIGHT)
			m_tree2Pad = 0;
		if (m_roadPad > PARALLAX_WEIGHT)
			m_roadPad = 0;

		m_cloudLayer.transform.localPosition = new Vector2(576-m_cloudPad, m_cloudLayer.transform.localPosition.y);
		m_mountainLayer.transform.localPosition = new Vector2(576 - m_moutainPad, m_mountainLayer.transform.localPosition.y);
		m_tree1Layer.transform.localPosition = new Vector2(576 - m_tree1Pad, m_tree1Layer.transform.localPosition.y);
		m_tree2Layer.transform.localPosition = new Vector2(576 - m_tree2Pad, m_tree2Layer.transform.localPosition.y);
		m_roadLayer.transform.localPosition = new Vector2(576 - m_roadPad, m_roadLayer.transform.localPosition.y);
	}
    // Update is called once per frame
    void Update()
    {       
    }
}
