using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillSlot : MonoBehaviour
    ,IPointerEnterHandler
    ,IPointerExitHandler
{
    [SerializeField]
    private bool isactive;

    public bool p_SlotActive
    {
        get
        {
            return isactive;
        }
        set
        {
            Color color = GetComponent<Image>().color;
            if (value)
            {
                color.a = 255f;
                GetComponent<Image>().color = color;
            }
            else
            {
                color.a = 10f;
                GetComponent<Image>().color = color;
            }
            isactive = value;
        }
    }

    public SkillNode _NowNode;
    public SkillNode SetNode
    {
        get
        {
            return _NowNode;
        }
        set
        {
            _NowNode = value;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        p_SlotActive = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
