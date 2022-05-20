using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeEditor : MonoBehaviour
{
    public enum EDITMODE
    {
        NORMAL,
        CREATELINE,
        DELETELINE,
        DELETENODE,
        EMAX
    }


    [SerializeField]
    SkillNode SkillNodeObject;
    [SerializeField]
    SkillSlot SkillSlotObject;


    [Header("Options")]
    [SerializeField]
    Vector2Int nSkillSlot;
    [SerializeField]
    Vector2Int SkillSlotPos;



    [Header("입출력용 데이터")]
    [SerializeField]
    List<SkillNode> skillnodes = new List<SkillNode>();
    [SerializeField]
    //List<SkillSlot> skillslots = null;
    SkillSlot[,] skillslots = null;
    [SerializeField]
    List<RelationLine> relationlines = new List<RelationLine>();
    [SerializeField]
    List<SkillNode> relationnode = new List<SkillNode>();



    
    public string classname;


    [Header("내부 데이터")]
    public GameObject Click_Copy_Obj = null;
    public bool Clicked = false;

    public Button[] ControlBtns = null;

    public Button CreateMode_Btn = null;
    public Button DeleteLine_Btn = null;
    public Button DeleteNode_Btn = null;
    public EDITMODE NowMode = EDITMODE.NORMAL;

    public RelationLine lineobj = null;
    public RelationLine templine = null;

    public SkillTreePanel treepanel = null;


    public void temp()
    {
        Debug.Log("tempsss");
    }
    private void Awake()
    {
        treepanel = GetComponentInChildren<SkillTreePanel>(); 
        SkillSlotObject = GetComponentInChildren<SkillSlot>();
        skillslots = treepanel.GetSkillSlotList;
        nSkillSlot = treepanel.nSkillSlot;

        ControlBtns = new Button[(int)EDITMODE.EMAX];


        for (EDITMODE i = EDITMODE.CREATELINE; i < EDITMODE.EMAX; i++)
        {
            ControlBtns[(int)i] = GameObject.Find($"{i.ToString()}").GetComponent<Button>();
            //ControlBtns[(int)i].onClick.AddListener(() => ChangeEditMode(i));
            //ControlBtns[(int)i].onClick.AddListener(delegate (EDITMODE i) { ChangeEditMode(i); });
        }

        ControlBtns[(int)EDITMODE.CREATELINE].onClick.AddListener(() => ChangeEditMode(EDITMODE.CREATELINE));
        ControlBtns[(int)EDITMODE.DELETELINE].onClick.AddListener(() => ChangeEditMode(EDITMODE.DELETELINE));
        ControlBtns[(int)EDITMODE.DELETENODE].onClick.AddListener(() => ChangeEditMode(EDITMODE.DELETENODE));

        SkillNodeObject.gameObject.SetActive(false);

         lineobj = Resources.Load<RelationLine>("Prefabs/RelationLine");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 시작");
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 끝");
        //throw new System.NotImplementedException();
    }
    
    public void ChangeEditMode(EDITMODE mode)
    {
        ColorBlock colorblock = ControlBtns[1].colors;
        if (mode == NowMode)
        {
            NowMode = EDITMODE.NORMAL;

            colorblock.normalColor = new Color32(255, 255, 255, 255);
            colorblock.selectedColor = new Color32(255, 255, 255, 255);


            for (int i = 1; i < (int)EDITMODE.EMAX; i++)
            {
                ControlBtns[i].colors = colorblock;
            }
        }
        else
        {
           
            NowMode = mode;

            for(int i=1;i<(int)EDITMODE.EMAX;i++)
            {
                if(i==(int)mode)
                {
                    colorblock.selectedColor = new Color32(255, 255, 0, 255);
                    colorblock.normalColor = new Color32(255, 255, 0, 255);
                    ControlBtns[i].colors = colorblock;
                }
                else
                {
                    colorblock.selectedColor = new Color32(255, 255, 255, 255);
                    colorblock.normalColor = new Color32(255, 255, 255, 255);
                    ControlBtns[i].colors = colorblock;
                }
            }

        }

        Debug.Log($"{NowMode.ToString()}");
    }

    public void On_Click_SkillList_Road()
    {
        int damage = 0;
        int level = 0;
        int rank = 0;
        //string classname = null;
        string skillname = null;
        string explain = null;
        List<Dictionary<int, string>> datalist = FileOpenDialog.OpenGetI.SkillListFileOpen(out classname);
        int x = 0;
        int y = 0;
        SkillNodeObject.gameObject.SetActive(true);

        for (int i=0;i<datalist.Count;i++)
        {
            int.TryParse(datalist[i][(int)EnumTypes.SkillInfoColums.SkillDamage], out damage);
            int.TryParse(datalist[i][(int)EnumTypes.SkillInfoColums.RequireLevel], out level);
            int.TryParse(datalist[i][(int)EnumTypes.SkillInfoColums.SkillRank], out rank);
            skillname = datalist[i][(int)EnumTypes.SkillInfoColums.SkillName];
            explain = datalist[i][(int)EnumTypes.SkillInfoColums.SkillExplain];
            
            SkillNode copyobj = GameObject.Instantiate<SkillNode>(SkillNodeObject);
            copyobj.name = skillname;
            copyobj.transform.parent = SkillNodeObject.transform.parent;
            copyobj.Init(classname, skillname, rank, damage, level, explain);
            copyobj.transform.position = new Vector3(SkillNodeObject.transform.position.x +(x*130),
                                                     SkillNodeObject.transform.position.y-(y* 130), 0);
            x++;
            if(x>3)
            {
                x = 0;
                y++;
            }
            skillnodes.Add(copyobj);
            
        }
        SkillNodeObject.gameObject.SetActive(false);
    }

    public void On_Click_SkillTree_Road()
    {
        List<Dictionary<int, string>> datalist = FileOpenDialog.OpenGetI.TreeFileOpen(out classname);
        SkillNodeObject.gameObject.SetActive(true);

        int damage = 0;
        int level = 0;
        int rank = 0;
        //string classname = null;
        string skillname = null;
        string explain = null;
        List<SkillNode> parentlist;
        List<SkillNode> childlist;

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


            SkillNode copyobj = GameObject.Instantiate<SkillNode>(SkillNodeObject);
            copyobj.name = skillname;
            //copyobj.transform.parent = SkillNodeObject.transform.parent;
            copyobj.Init(classname, skillname, rank, damage, level, explain);

            tempnodelist.Add(copyobj);

            treepanel.SetTreeNode(new Vector2Int(indexX, indexY), copyobj);//만들어준 슼킬 노드를 바로 슬롯에 장착시켜준다.

        }


        //노드들을 다 만들어 준 다음에 연결정보를 읽어와서 노드들을 연결해준다.
        for (int i = 0; i < datalist.Count; i++)
        {
            ParseRelation(tempnodelist, tempnodelist[i], datalist[i][(int)EnumTypes.SkillTreeColums.Child]);
        }


        SkillNodeObject.gameObject.SetActive(false);
    }

    //현재 노드와 child열결정보만 가지고 관계를 설정해준다.
    public void ParseRelation(List<SkillNode> list/*전체 노드들의 리스트*/,SkillNode node, string relation/*현재 노드의 연결정보*/)
    {
        
        List<SkillNode> ret = new List<SkillNode>();
        string[] namelist = relation.Split(',');

        for (int i=0;i<namelist.Length;i++)
        {
            for (int j = 0; j < list.Count;j++)
            {
                if(list[j].SkillName == namelist[i])
                {
                    templine = GameObject.Instantiate<RelationLine>(lineobj);
                    templine.transform.parent = this.transform;
                    templine.SetNode(node,node.transform.position);
                    templine.SetNode(list[j],list[j].transform.position);
                    templine = null;
                    //ret.Add(list[j]);
                    break;
                }
            }
        }
    }

    public void On_Click_SkillTree_Save()
    {
        relationnode = treepanel.skillnodes;
        FileSaveDialog.SaveGetI.FileSave(classname, relationnode);
    }

    public void HideSlot(int val)
    {
        if (skillslots == null)
            skillslots = treepanel.GetSkillSlotList;
        //int num = skillslots.GetLength(0) * skillslots.GetLength(1);
        int maxY = skillslots.GetLength(0);
        int maxX = skillslots.GetLength(1);

        for(int x=0;x<maxX;x++)
        {
            for(int y=0;y<maxY;y++)
            {
                skillslots[y,x].p_SlotActive = false;
                //skillslots[y, x].gameObject.SetActive(false);
            }
        }

        for (int x = 0; x < maxX; x++)
        {
            skillslots[val,x].p_SlotActive = true;
            //skillslots[val, x].gameObject.SetActive(true);
        }
        Debug.Log($"숨길 인덱스{val}");
    }

    public void MouseDown(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);
        SkillNode skillnode;
        GameObject temp = null;


        if(NowMode==EDITMODE.NORMAL)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "SkillNode")
                {
                    //temp = GameObject.Instantiate(a.gameObject);
                    skillnode = a.gameObject.GetComponent<SkillNode>();
                    if (skillnode.State == SkillNode.NodeState.NOMAL)
                    {
                        Click_Copy_Obj = GameObject.Instantiate(a.gameObject);
                        Click_Copy_Obj.transform.parent = this.transform;
                        Click_Copy_Obj.GetComponent<SkillNode>().IsClicked = true;
                        Click_Copy_Obj.GetComponent<SkillNode>().State = SkillNode.NodeState.CLICKED;
                        //skillnode.IsClicked = true;
                        //skillnode.State = SkillNode.NodeState.CLICKED;
                    }
                    break;
                }
            }
            //각 스킬의 랭크에 따라 놓을수 있는 슬롯이 정해져 있다. 
            //스킬의 트리구조 설정을 쉽게 하기 위해서 이렇게 구성
            if (Click_Copy_Obj != null) 
                HideSlot(Click_Copy_Obj.gameObject.GetComponent<SkillNode>().SkillRank - 1);

        }
        else if(NowMode == EDITMODE.CREATELINE)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "SkillNode")
                {
                    //temp = GameObject.Instantiate(a.gameObject);
                    skillnode = a.gameObject.GetComponent<SkillNode>();
                    if (skillnode.State == SkillNode.NodeState.SETTING|| skillnode.State == SkillNode.NodeState.CONNECTED)
                    {
                        templine = GameObject.Instantiate<RelationLine>(lineobj);
                        templine.transform.parent = this.transform;
                        //templine.SetPosition(0, skillnode.transform.position);
                        templine.SetNode(skillnode);

                    }
                    break;
                }
            }
        }
        //else if(NowMode == EDITMODE.DELETENODE)
        //{

        //}
        //else if (NowMode == EDITMODE.DELETELINE)
        //{

        //}

        Clicked = true;
    }

    

    public void MouseUp(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);
        bool flag = false;
        SkillNode skillnode;
        GameObject temp = null;

        if (NowMode == EDITMODE.NORMAL)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "SkillSlot")
                {
                    if (a.gameObject.GetComponent<SkillSlot>().SetNode == null)
                    {
                        if (a.gameObject.GetComponent<SkillSlot>().p_SlotActive == true)
                        {
                            if(Click_Copy_Obj!=null)
                            {
                                //Click_Copy_Obj.transform.parent = a.gameObject.transform;
                                //Click_Copy_Obj.transform.localPosition = new Vector3(0, 0, 0);
                                //a.gameObject.GetComponent<SkillSlot>().SetNode = Click_Copy_Obj.GetComponent<SkillNode>();

                                treepanel.SetTreeNode(a.gameObject.GetComponent<SkillSlot>(), Click_Copy_Obj.GetComponent<SkillNode>());
                                Click_Copy_Obj.GetComponent<SkillNode>().State = SkillNode.NodeState.SETTING;
                                Click_Copy_Obj.GetComponent<SkillNode>().IsClicked = false;
                                flag = true;
                            }
                            
                        }
                        
                    }

                }
            }
            if (!flag)
                GameObject.Destroy(Click_Copy_Obj);

            Click_Copy_Obj = null;

            if(skillslots ==null)
                skillslots = treepanel.GetSkillSlotList;

            for (int x = 0; x < skillslots.GetLength(1); x++)
            {
                for (int y = 0; y < skillslots.GetLength(0); y++)
                {
                    skillslots[y, x].p_SlotActive = true;
                    //skillslots[y, x].gameObject.SetActive(true);
                }
            }

            //for (int y = 0; y < skillslots.Count; y++)
            //{
            //    skillslots[y].p_SlotActive = true;
            //}
        }
        else if (NowMode == EDITMODE.CREATELINE)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "SkillNode")
                {
                    skillnode = a.gameObject.GetComponent<SkillNode>();
                    if (skillnode.State == SkillNode.NodeState.SETTING|| skillnode.State == SkillNode.NodeState.CONNECTED)
                    {
                        flag = templine.SetNode(skillnode);
                    }
                    
                }
                break;
            }
            if (flag == false)
            {
                if(templine!=null)
                    GameObject.Destroy(templine.gameObject);
            }
            templine = null;
        }
        else if (NowMode == EDITMODE.DELETENODE)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "SkillNode")
                {
                    skillnode = a.gameObject.GetComponent<SkillNode>();
                    if(skillnode.State == SkillNode.NodeState.SETTING || skillnode.State == SkillNode.NodeState.CONNECTED)//없어져야 할 정보들. 1. 에디터에서 들고있는 노드리스트, 2. 노드에 연결되어 있는 라인들, 3. 노드 자체
                    {
                        treepanel.DeleteTreeNode(skillnode);
                        //skillnode.DeleteNode();

                    }
                }
                break;
            }

        }
        else if (NowMode == EDITMODE.DELETELINE)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "RelationLine")
                {
                    RelationLine relation = a.gameObject.GetComponent<RelationLine>();
                    relation.DeleteRelation();
                }
                break;
            }
        }




        Clicked = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        gr = GetComponent<GraphicRaycaster>();
    }

    GraphicRaycaster gr;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MouseDown(Input.mousePosition);
            //Debug.Log("마우스눌립");
        }

        if(Input.GetMouseButtonUp(0))
        {
            MouseUp(Input.mousePosition);
        }
        if(Clicked)
        {
            if(NowMode == EDITMODE.NORMAL)
            {
                if (Click_Copy_Obj != null)
                {
                    Click_Copy_Obj.transform.position = Input.mousePosition;
                }
            }
            else if(NowMode == EDITMODE.CREATELINE)
            {
                if(templine != null)//해당 모드에서 슬롯에 세팅된 노드를 누르면 라인을 생성해 줄꺼기 때문에 이렇게 판단
                {
                    templine.SetPosition(1, Input.mousePosition);
                }
            }
        }
    }
}
