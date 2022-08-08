using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEnums;

public class SelectScript : MonoBehaviour
{
    Transform top;
    Material m_top;
    Transform left;
    Material m_left;
    Transform right;
    Material m_right;

    Tool currentTool;
    MenuChoice currentChoice = MenuChoice.None;

    // Start is called before the first frame update
    void Start()
    {
        left = transform.GetChild(0);
        m_left = left.GetComponent<Renderer>().material;
        top = transform.GetChild(1);
        m_top = top.GetComponent<Renderer>().material;
        right = transform.GetChild(2);
        m_right = right.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // Update color
        m_top.color = (currentTool == Tool.Locomotion) ? Color.green : Color.grey;
        m_left.color = (currentTool == Tool.Tranformation) ? Color.blue : Color.grey;
        m_right.color = (currentTool == Tool.Examine) ? Color.red : Color.grey;
        // Update lift
        float delta_top_y = Mathf.Lerp(top.localPosition.y, (currentChoice == MenuChoice.Locomotion) ? 0.2f : 0, 0.5f) - top.localPosition.y;
        top.localPosition += new Vector3(0, delta_top_y, 0);
        float delta_left_y = Mathf.Lerp(left.localPosition.y, (currentChoice == MenuChoice.Tranformation) ? 0.2f : 0, 0.5f) - left.localPosition.y;
        left.localPosition += new Vector3(0, delta_left_y, 0);
        float delta_right_y = Mathf.Lerp(right.localPosition.y, (currentChoice == MenuChoice.Examine) ? 0.2f : 0, 0.5f) - right.localPosition.y;
        right.localPosition += new Vector3(0, delta_right_y, 0);
    }

    public void setCurrent(Tool tool)
    {
        currentTool = tool;      
    }

    public void setChoosing(MenuChoice choice)
    {
        currentChoice = choice;
    }
}
