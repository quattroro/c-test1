using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class SkillNode:MonoBehaviour
    ,IPointerEnterHandler
    ,IPointerExitHandler
{
    //스킬 노드의 상태를 구분하는데 사용
    public enum NodeState
    {
        NOMAL,//슬롯에 배치되지 않은 기본상태
        CLICKED,//클릭된 상태
        SETTING,//슬롯에 배치된 상태
        CONNECTED,//연결관계설정까지 완료된 상태
    }

    [SerializeField]
    bool _isactive = true;

    public bool P_NodeActive
    {
        get
        {
            return _isactive;
        }
        set
        {
            _isactive = value;
            if(value)
            {
                GetComponent<Image>().sprite = IconTextures[0];
                Color color = GetComponent<Image>().color;
                color.a = 1f;
                GetComponent<Image>().color = color;
            }
            else
            {
                GetComponent<Image>().sprite = IconTextures[1];
                Color color = GetComponent<Image>().color;
                color.a = 0.2f;
                GetComponent<Image>().color = color;
            }
        }
    }
        

    [SerializeField]
    private NodeState _state = NodeState.NOMAL;

    public NodeState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }
    }

    [Header("스킬정보")]
    public string ClassName;
    public int SkillDamage;
    public int RequireLevel;
    public string SkillName;
    public int SkillRank;
    public string SkillExplain;
    public int IconID;
    public Sprite IconTexture;
    public Sprite[] IconTextures;
    [Header("연결정보")]
    public List<SkillNode> parentNode;
    public List<SkillNode> childNode;
    public Vector2Int index = Vector2Int.zero;

    [Header("내부정보")]
    public bool IsClicked;
    public SkillNode clickednode;
    public SkillInfoPanel infopanel;
    public bool CoroutineFlag = false;
    public bool mouseEnter = false;
    public bool PopUpShowed = false;

    public List<RelationLine> relations = new List<RelationLine>();//현재 나한테 연결되어있는 관계라인들을 노드가 직접 가지고 있는다. (이후 노드를 삭제할때 라인들까지 한번에 삭제하기 위해)


    public void DeleteNode()
    {
        if(relations.Count>0)
        {
            for(int i=0;i<relations.Count;i++)
            {
                relations[i].DeleteRelation();
            }
            relations.Clear();
        }
        GameObject.Destroy(this.gameObject);
    }

    public void Init(string cName,string sName, int rank, int Damage, int rLevel, string Explain)
    {
        ClassName = cName;
        SkillName = sName;
        SkillDamage = Damage;
        RequireLevel = rLevel;
        SkillExplain = Explain;
        SkillRank = rank;
        //IconTexture = Resources.Load<Sprite>($"SkillIcons/{ClassName}/{SkillName}");
        IconTextures = Resources.LoadAll<Sprite>($"SkillIcons/{ClassName}/{SkillName}");
        IconTexture = IconTextures[0];
        GetComponent<Image>().sprite = IconTexture;

    }

    
    //연결을 해주고 라인을 그려준다.
    public void SetRelation(SkillNode node)
    {
        if (node.SkillRank == this.SkillRank)
            return;
        if(node.SkillRank<this.SkillRank)
        {
            //node.childNode.Add(this);
            this.parentNode.Add(node);
        }
        else
        {
            this.childNode.Add(node);
            //node.parentNode.Add(node);
        }
    }

    public void DeleteRalation(SkillNode node, RelationLine line)
    {
        if (node.SkillRank < this.SkillRank)//node -> parent
        {
            parentNode.Remove(node);
        }
        else
        {
            childNode.Remove(node);
        }
        relations.Remove(line);
        if(relations.Count<=0)
        {
            State = NodeState.SETTING;
        }
    }
    
    IEnumerator IShowPopUpCount()
    {
        CoroutineFlag = true;
        yield return new WaitForSeconds(0.5f);
        if (!CoroutineFlag)
            yield break;
        PopUpShowed = true;
        CoroutineFlag = false;
        ShowPopUp();
    }

    public void ShowPopUp()
    {
        if (!PopUpShowed)
        {
            return;
        }


        if (infopanel.gameObject.activeSelf == false)
            infopanel.gameObject.SetActive(true);
        Debug.Log($"{name} 팝업띄움");

        infopanel.transform.position= Input.mousePosition;
        Debug.Log($"{Input.mousePosition.x},{Input.mousePosition.y} 여기로 이동");

        Vector2 size = infopanel.GetComponent<RectTransform>().rect.size;
        Debug.Log($"{size.x},{size.y} 사이즈");
        Vector2 pos = infopanel.transform.position;
        Debug.Log($"{pos.x},{pos.y} 로컬좌표");

        if ((pos.x+size.x)>=Screen.width)
        {
            //Debug.Log("오른쪽 걸림");
            pos.x -= size.x-2;
            Debug.Log($"오른쪽 걸림 {pos.x}");
        }
        if((pos.y - size.y) <= 0)
        {
            pos.y += size.y+2;
            Debug.Log($"아래쪽 걸림 {pos.y}");
        }
        infopanel.transform.position = pos;

        infopanel.ShowPopUp(this);
    }

    public void ClosePopUp()
    {
        if(PopUpShowed)
        {
            return;
        }

        if (infopanel.gameObject.activeSelf == true)
            infopanel.gameObject.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsClicked)
            return;
        if (State == NodeState.CLICKED)
            return;

        if (!CoroutineFlag)
            StartCoroutine(IShowPopUpCount());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //mouseEnter = false;
        if (CoroutineFlag)
        {
            StopCoroutine(IShowPopUpCount());
            CoroutineFlag = false;
        }
        PopUpShowed = false;
        ClosePopUp();
        
    }


    private void Start()
    {
        if(infopanel==null)
        infopanel = GameObject.Find("InfoPanel").GetComponent<SkillInfoPanel>();
    }

    private void Update()
    {
        //ShowPopUp();
    }
}
