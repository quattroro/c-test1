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
                temp.name = $"Slot({x},{y})";
                skillslots.Add(temp);
            }
        }
        slotobj.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
