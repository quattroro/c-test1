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

    private NodeState _state = NodeState.NOMAL;

    public NodeState State
    {
        get;
        set;
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

    [Header("연결정보")]
    public List<SkillNode> parentNode;
    public List<SkillNode> childNode;

    [Header("내부정보")]
    public bool IsClicked;
    public SkillNode clickednode;
    public SkillInfoPanel infopanel;
    public bool CoroutineFlag = false;
    public bool mouseEnter = false;
    public bool PopUpShowed = false;


    public void Init(string cName,string sName, int rank, int Damage, int rLevel, string Explain)
    {
        ClassName = cName;
        SkillName = sName;
        SkillDamage = Damage;
        RequireLevel = rLevel;
        SkillExplain = Explain;
        SkillRank = rank;
        IconTexture = Resources.Load<Sprite>($"SkillIcons/{ClassName}/{SkillName}");
        GetComponent<Image>().sprite = IconTexture;

    }

    

    public void SetRelation(SkillNode node)
    {

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

        infopanel.transform.position = Input.mousePosition;


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
    }

    private void Update()
    {
        //ShowPopUp();
    }
}
