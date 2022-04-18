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

    public Vector2Int index;
    public bool p_SlotActive
    {
        get
        {
            return isactive;
        }
        set
        {
            isactive = value;
            
            if (isactive)
            {
                Color color = GetComponent<Image>().color;
                color.a = 255f;
                GetComponent<Image>().color = color;
                Debug.Log($"{name}불투명");
            }
            else
            {
                Color color = GetComponent<Image>().color;
                color.a = 10f;
                GetComponent<Image>().color = color;
                //Debug.Log($"{name}투명");
            }
            
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
            value.index = this.index;
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
