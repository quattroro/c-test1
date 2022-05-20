using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class SkillNode:MonoBehaviour
    ,IPointerEnterHandler
    ,IPointerExitHandler
{
    //��ų ����� ���¸� �����ϴµ� ���
    public enum NodeState
    {
        NOMAL,//���Կ� ��ġ���� ���� �⺻����
        CLICKED,//Ŭ���� ����
        SETTING,//���Կ� ��ġ�� ����
        CONNECTED,//������輳������ �Ϸ�� ����
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

    [Header("��ų����")]
    public string ClassName;
    public int SkillDamage;
    public int RequireLevel;
    public string SkillName;
    public int SkillRank;
    public string SkillExplain;
    public int IconID;
    public Sprite IconTexture;
    public Sprite[] IconTextures;
    [Header("��������")]
    public List<SkillNode> parentNode;
    public List<SkillNode> childNode;
    public Vector2Int index = Vector2Int.zero;

    [Header("��������")]
    public bool IsClicked;
    public SkillNode clickednode;
    public SkillInfoPanel infopanel;
    public bool CoroutineFlag = false;
    public bool mouseEnter = false;
    public bool PopUpShowed = false;

    public List<RelationLine> relations = new List<RelationLine>();//���� ������ ����Ǿ��ִ� ������ε��� ��尡 ���� ������ �ִ´�. (���� ��带 �����Ҷ� ���ε���� �ѹ��� �����ϱ� ����)


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

    
    //������ ���ְ� ������ �׷��ش�.
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
        Debug.Log($"{name} �˾����");

        infopanel.transform.position= Input.mousePosition;
        Debug.Log($"{Input.mousePosition.x},{Input.mousePosition.y} ����� �̵�");

        Vector2 size = infopanel.GetComponent<RectTransform>().rect.size;
        Debug.Log($"{size.x},{size.y} ������");
        Vector2 pos = infopanel.transform.position;
        Debug.Log($"{pos.x},{pos.y} ������ǥ");

        if ((pos.x+size.x)>=Screen.width)
        {
            //Debug.Log("������ �ɸ�");
            pos.x -= size.x-2;
            Debug.Log($"������ �ɸ� {pos.x}");
        }
        if((pos.y - size.y) <= 0)
        {
            pos.y += size.y+2;
            Debug.Log($"�Ʒ��� �ɸ� {pos.y}");
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
