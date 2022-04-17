using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreePanel : MonoBehaviour
{
    SkillSlot slotobj;

    [Header("Options")]
    public Vector2Int nSkillSlot;
    public Vector2Int SkillSlotPos;

    [SerializeField]
    List<SkillSlot> skillslots = new List<SkillSlot>();

    [SerializeField]
    public List<SkillNode> skillnodes = new List<SkillNode>();// 관계가 설정된 스킬노드들을 가지고 있을 리스트

    //
    public void SetTreeNode(SkillSlot slot, SkillNode node)
    {
        node.transform.parent = slot.gameObject.transform;
        node.transform.localPosition = new Vector3(0, 0, 0);
        slot.SetNode = node;
        skillnodes.Add(node);
        


    }

    public void DeleteTreeNode(SkillNode node)
    {
        node.DeleteNode();
    }


    public List<SkillSlot> GetSkillSlotList
    {
        get
        {
            return skillslots;
        }
    }

    private void Awake()
    {
        slotobj = GetComponentInChildren<SkillSlot>();
        Init(nSkillSlot, SkillSlotPos);
    }


    public void Init(Vector2Int slotnum, Vector2Int slotpos)
    {
        SkillSlot temp;
        for (int y = 0; y < slotnum.y; y++)
        {
            for (int x = 0; x < slotnum.x; x++)
            {
                temp = GameObject.Instantiate<SkillSlot>(slotobj);
                temp.transform.parent = slotobj.transform.parent;
                temp.transform.position = new Vector3(slotobj.transform.position.x + (x * SkillSlotPos.x), slotobj.transform.position.y - (y * SkillSlotPos.y), 0);
                temp.index = new Vector2Int(x, y);
                temp.name = $"Slot({x},{y})";
                skillslots.Add(temp);
            }
        }
        slotobj.gameObject.SetActive(false);
    }


}
