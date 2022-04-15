using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillInfoPanel : MonoBehaviour
{
    public Image sImage;
    public Text sname;
    public Text info1;
    public Text info2;

    public void ShowPopUp(SkillNode node)
    {
        sImage.sprite = node.IconTexture;
        
        info1.text = string.Format("Rank {0}\nDamage {1}\nRequire Level{2}", node.SkillRank,node.SkillDamage,node.RequireLevel);
        info2.text = node.SkillExplain;
        sname.text = node.SkillName;
    }

    private void Awake()
    {
       // sImage = GetComponentInChildren<Image>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
