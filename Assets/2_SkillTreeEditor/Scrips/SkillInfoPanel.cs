using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillInfoPanel : MonoBehaviour
{
    public Image sImage;
    public Text sname;
    public Text SkillRank;
    public Text Damage;
    public Text RequireLevel;
    public Text info2;

    private void Awake()
    {
        // sImage = GetComponentInChildren<Image>();



    }


    public void ShowPopUp(SkillNode node)
    {
        sImage.sprite = node.IconTexture;
        SkillRank.text = string.Format("SkillRank {0}", node.SkillRank);
        Damage.text = string.Format("SkillDamage {0}", node.SkillDamage);
        RequireLevel.text = string.Format("RequireLevel {0}", node.RequireLevel);
        if(SkillTreeSystem.TreeGetI!=null)
        {
            if(SkillTreeSystem.TreeGetI.p_NowLevel <node.RequireLevel)
            {
                RequireLevel.color = Color.red;
            }
            else
            {
                RequireLevel.color = Color.blue;
            }
            
        }
        info2.text = node.SkillExplain;
        sname.text = node.SkillName;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
