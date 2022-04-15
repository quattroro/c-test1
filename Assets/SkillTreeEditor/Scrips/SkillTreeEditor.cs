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
        DELETENODE
    }


    [SerializeField]
    SkillNode SkillNodeObject;
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
    List<SkillSlot> skillslots = null;




    [Header("내부 데이터")]
    public GameObject Click_Copy_Obj = null;
    public bool Clicked = false;
    public Button CreateMode_Btn = null;
    public Button DeleteMode_Btn = null;
    public EDITMODE NowMode = EDITMODE.NORMAL;

    public void temp()
    {
        Debug.Log("tempsss");
    }
    private void Awake()
    {
        SkillTreePanel skilltreepanel = GetComponentInChildren<SkillTreePanel>(); 
        SkillSlotObject = GetComponentInChildren<SkillSlot>();
        skillslots = skilltreepanel.GetSkillSlotList;
        nSkillSlot = skilltreepanel.nSkillSlot;

        CreateMode_Btn.onClick.AddListener(() => ChangeEditMode(EDITMODE.CREATELINE));
        DeleteMode_Btn.onClick.AddListener(() => ChangeEditMode(EDITMODE.DELETELINE));
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
        ColorBlock colorblock = DeleteMode_Btn.colors;
        if (mode == NowMode)
        {
            NowMode = EDITMODE.NORMAL;

            colorblock.normalColor = new Color32(255, 255, 255, 255);
            colorblock.selectedColor = new Color32(255, 255, 255, 255);
            CreateMode_Btn.colors = colorblock;

            //colorblock.normalColor = new Color32(255, 255, 255, 255);
            //colorblock.selectedColor = new Color32(255, 255, 255, 255);
            DeleteMode_Btn.colors = colorblock;
        }
        else
        {
           
            NowMode = mode;
            if (mode == EDITMODE.CREATELINE)
            {
                colorblock.selectedColor = new Color32(255, 255, 0, 255);
                colorblock.normalColor = new Color32(255, 255, 0, 255);
                CreateMode_Btn.colors = colorblock;

                colorblock.selectedColor = new Color32(255, 255, 255, 255);
                colorblock.normalColor = new Color32(255, 255, 255, 255);
                DeleteMode_Btn.colors = colorblock;
            }
            else
            {
                colorblock.selectedColor = new Color32(255, 255, 0, 255);
                colorblock.normalColor = new Color32(255, 255, 0, 255);

                DeleteMode_Btn.colors = colorblock;
                colorblock.selectedColor = new Color32(255, 255, 255, 255);
                colorblock.normalColor = new Color32(255, 255, 255, 255);
                CreateMode_Btn.colors = colorblock;
            }
            
        }

        Debug.Log($"{mode.ToString()}");
    }

    public void On_Click_SkillList_Road()
    {
        int damage = 0;
        int level = 0;
        int rank = 0;
        string classname = null;
        string skillname = null;
        string explain = null;
        List<Dictionary<int, string>> datalist = FileOpenDialog.GetI.SkillListFileOpen(out classname);
        int x = 0;
        int y = 0;

        for(int i=0;i<datalist.Count;i++)
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
        FileOpenDialog.GetI.TreeFileOpen();
    }

    public void On_Click_SkillTree_Save()
    {
        FileSaveDialog.GetI.FileSave();
    }

    public void HideSlot(int val)
    {
        for(int y=0;y<skillslots.Count;y++)
        {
            //skillslots[y].gameObject.SetActive(false);
            skillslots[y].p_SlotActive = false;

        }

        int index = val * nSkillSlot.x;
        for(int y=0;y<nSkillSlot.x;y++)
        {
            skillslots[index + y].p_SlotActive = true;
            //skillslots[index + y].gameObject.SetActive(true);
        }
    }

    public void MouseDown(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);
        SkillNode skillnode;



        if(NowMode==EDITMODE.NORMAL)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "SkillNode")
                {
                    Click_Copy_Obj = GameObject.Instantiate(a.gameObject);
                    skillnode = Click_Copy_Obj.GetComponent<SkillNode>();
                    if (skillnode.State == SkillNode.NodeState.NOMAL)
                    {
                        Click_Copy_Obj.transform.parent = this.transform;
                        skillnode.IsClicked = true;
                        skillnode.State = SkillNode.NodeState.CLICKED;
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
                    Click_Copy_Obj = GameObject.Instantiate(a.gameObject);
                    skillnode = Click_Copy_Obj.GetComponent<SkillNode>();
                    if (skillnode.State == SkillNode.NodeState.CONNECTED)//연결을 끊어준다.
                    {
                        Click_Copy_Obj.transform.parent = this.transform;
                        skillnode.IsClicked = true;
                        skillnode.State = SkillNode.NodeState.CLICKED;
                    }
                    break;
                }
            }
        }
        else if(NowMode == EDITMODE.DELETENODE)
        {

        }
        else if (NowMode == EDITMODE.DELETELINE)
        {

        }

        Clicked = true;
    }

    

    public void MouseUp(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);
        bool flag = false;

        if(NowMode == EDITMODE.NORMAL)
        {
            foreach (var a in result)
            {
                if (a.gameObject.tag == "SkillSlot")
                {
                    if (a.gameObject.GetComponent<SkillSlot>().SetNode == null)
                    {
                        Click_Copy_Obj.transform.parent = a.gameObject.transform;
                        Click_Copy_Obj.transform.localPosition = new Vector3(0, 0, 0);
                        a.gameObject.GetComponent<SkillSlot>().SetNode = Click_Copy_Obj.GetComponent<SkillNode>();
                        Click_Copy_Obj.GetComponent<SkillNode>().IsClicked = false;
                        Click_Copy_Obj.GetComponent<SkillNode>().State = SkillNode.NodeState.SETTING;
                        flag = true;
                    }

                }
            }
            if (!flag)
                GameObject.Destroy(Click_Copy_Obj);

            Click_Copy_Obj = null;
            for (int y = 0; y < skillslots.Count; y++)
            {
                skillslots[y].p_SlotActive = true;
                //skillslots[y].gameObject.SetActive(true);
            }
        }
        else if (NowMode == EDITMODE.CREATELINE)
        {

        }
        else if (NowMode == EDITMODE.DELETENODE)
        {

        }
        else if (NowMode == EDITMODE.DELETELINE)
        {

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
            if(Click_Copy_Obj!=null)
            {
                Click_Copy_Obj.transform.position = Input.mousePosition;
            }
            
        }
    }
}
