using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSystem : MonoBehaviour
{
    public SkillTreePanel panel;

    public SkillNode nodeobj;

    public string classname;

    public RelationLine lineobj;
    public RelationLine templine;

    public int _nowlevel = 0;

    public int p_NowLevel
    {
        get
        {
            return _nowlevel;
        }
        set
        {
            _nowlevel = value;
            LevelShowText.text = $"NowLevel = {_nowlevel}";
        }
    }
    public Text LevelShowText;


    GraphicRaycaster gr;

    [SerializeField]
    List<SkillNode> skillnodes = new List<SkillNode>();

    [SerializeField]
    List<RelationLine> lines = new List<RelationLine>();

    [SerializeField]
    SkillSlot[,] skillslots = null;

    public void LevelUp_Btn()
    {
        p_NowLevel = p_NowLevel + 1;
    }

    public void Reset_Btn()
    {

    }

    private void Awake()
    {
        p_NowLevel = 0;
        gr = GetComponent<GraphicRaycaster>();
        panel = GetComponentInChildren<SkillTreePanel>();
        nodeobj = Resources.Load<SkillNode>("Prefabs/SkillNode");
        lineobj = Resources.Load<RelationLine>("Prefabs/RelationLine");
    }

    public void On_ClassSelect_Btn(string classname)
    {
        SkillTree_Road(classname);
    }


    public void SkillTree_Road(string classname)
    {
        string filepath = UnityEngine.Application.dataPath + $"/Resources/CSV/{classname}_Relation.csv";
        Debug.Log(filepath);
        List<Dictionary<int, string>> datalist = FileOpenDialog.OpenGetI.TreeFileOpen(filepath,out classname);
        //nodeobj.gameObject.SetActive(true);

        int damage = 0;
        int level = 0;
        int rank = 0;
        //string classname = null;
        string skillname = null;
        string explain = null;
        //List<SkillNode> parentlist;
        //List<SkillNode> childlist;

        List<SkillNode> tempnodelist = new List<SkillNode>();

        for (int i = 0; i < datalist.Count; i++)
        {
            string[] indexstr = datalist[i][(int)EnumTypes.SkillTreeColums.Index].Split(',');
            int indexX = -1;
            int indexY = -1;
            int.TryParse(indexstr[0], out indexX);
            int.TryParse(indexstr[1], out indexY);
            int.TryParse(datalist[i][(int)EnumTypes.SkillTreeColums.SkillDamage], out damage);
            int.TryParse(datalist[i][(int)EnumTypes.SkillTreeColums.ReauireLevel], out level);
            int.TryParse(datalist[i][(int)EnumTypes.SkillTreeColums.SkillRank], out rank);
            skillname = datalist[i][(int)EnumTypes.SkillTreeColums.SkillName];
            explain = datalist[i][(int)EnumTypes.SkillTreeColums.SkillExplain];


            SkillNode copyobj = GameObject.Instantiate<SkillNode>(nodeobj);
            copyobj.name = skillname;
            //copyobj.transform.parent = SkillNodeObject.transform.parent;
            copyobj.Init(classname, skillname, rank, damage, level, explain);
            copyobj.P_NodeActive = false;
            tempnodelist.Add(copyobj);
            skillnodes.Add(copyobj);

            panel.SetTreeNode(new Vector2Int(indexX, indexY), copyobj);//������� ��ų ��带 �ٷ� ���Կ� ���������ش�.

        }

        //������ �� ����� �� ������ ���������� �о�ͼ� ������ �������ش�.
        for (int i = 0; i < datalist.Count; i++)
        {
            ParseRelation(tempnodelist, tempnodelist[i], datalist[i][(int)EnumTypes.SkillTreeColums.Child]);
        }

        if(skillslots==null)
            skillslots = panel.GetSkillSlotList;

        for (int y = 0; y < skillslots.GetLength(0); y++)
        {
            for (int x = 0; x < skillslots.GetLength(1); x++)
            {
                if (skillslots[y, x].SetNode == null)
                {
                    skillslots[y, x].gameObject.SetActive(false);
                }
            }
        }

        //nodeobj.gameObject.SetActive(false);
        //for(int i=0;i<skillslots)
    }

    public void ParseRelation(List<SkillNode> list/*��ü ������ ����Ʈ*/, SkillNode node, string relation/*���� ����� ��������*/)
    {

        List<SkillNode> ret = new List<SkillNode>();
        string[] namelist = relation.Split(',');

        for (int i = 0; i < namelist.Length; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].SkillName == namelist[i])
                {
                    templine = GameObject.Instantiate<RelationLine>(lineobj);
                    templine.transform.parent = panel.lines.transform;
                    templine.SetNode(node, node.transform.position);
                    templine.SetNode(list[j], list[j].transform.position);
                    lines.Add(templine);
                    templine = null;
                    //ret.Add(list[j]);
                    break;
                }
            }
        }
    }

    public void MouseDown(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);
        SkillNode clicknode;
        SkillNode temp;

        foreach (var a in result)
        {
            if(a.gameObject.tag == "SkillNode")
            {
                clicknode = a.gameObject.GetComponent<SkillNode>();
                if (clicknode.P_NodeActive == false && clicknode.RequireLevel <= p_NowLevel)
                {
                    clicknode.P_NodeActive = true;
                    if(clicknode.parentNode.Count>0)
                    {
                        SetParentNodes(clicknode.parentNode);
                    }

                }

            }
        }

    }

    //��¥�� �� ������ �ڽ��� ������ ���� �� �ְ� �θ�� �ϳ� ���̴�.
    public void SetParentNodes(List<SkillNode> parents)
    {
        parents[0].P_NodeActive = true;
        if(parents[0].parentNode==null)
        {
            return;
        }
        else
        {
            SetParentNodes(parents[0].parentNode);
        }

    }

    public void SetChildNodes(List<SkillNode> childs)
    {
        for(int i=0;i<childs.Count;i++)
        {
            if(childs[i].P_NodeActive==true)
            {
                childs[i].P_NodeActive = false;
                if (childs[i].childNode == null)
                {
                    return;
                }
                else
                {
                    SetChildNodes(childs[i].childNode);
                }
            }
        }
    }

    public void MouseDown2(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);
        SkillNode clicknode;

        foreach (var a in result)
        {
            if (a.gameObject.tag == "SkillNode")
            {
                clicknode = a.gameObject.GetComponent<SkillNode>();
                if (clicknode.P_NodeActive == true)
                {
                    clicknode.P_NodeActive = false;
                    if (clicknode.childNode.Count > 0)
                    {
                        SetChildNodes(clicknode.childNode);
                    }

                }
            }
        }

    }

    public void MouseUp(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);

        foreach (var a in result)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            MouseDown2(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown(Input.mousePosition);
        }

    }
}
