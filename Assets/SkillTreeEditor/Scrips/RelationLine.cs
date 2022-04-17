using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationLine : UILineRenderer
{
    public SkillNode UpNode;
    public SkillNode DownNode;

    public List<SkillNode> tempnode = new List<SkillNode>();

    public bool SetNode(SkillNode node)
    {
        tempnode.Add(node);
        if (tempnode.Count >= 2)
        {
            //상위에서 하위로의 관계만 허용한다.
            if (tempnode[0].SkillRank < tempnode[1].SkillRank)
            {
                SetRelation(tempnode[0], tempnode[1]);
                return true;
            }
        }
        else if(tempnode.Count ==1)
        {
            SetPosition(0, tempnode[0].transform.position);
        }
        return false;
        //tempnode.Clear();
    }

    public override void LineUpdate()
    {
        Vector3 differenceVector = point[1] - point[0];
        if (tempnode.Count>=2)
        {
            imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            imageRectTransform.pivot = new Vector2(0, 0.5f);
            imageRectTransform.position = point[0];
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetRelation(SkillNode UpNode, SkillNode DownNode)
    {
        this.UpNode = UpNode;
        UpNode.SetRelation(DownNode);
        UpNode.State = SkillNode.NodeState.CONNECTED;
        UpNode.relations.Add(this);
        this.DownNode = DownNode;
        DownNode.SetRelation(UpNode);
        DownNode.State = SkillNode.NodeState.CONNECTED;
        DownNode.relations.Add(this);
    }

    public void DeleteRelation()
    {
        UpNode.DeleteRalation(DownNode);
        UpNode.relations.Remove(this);
        DownNode.DeleteRalation(UpNode);
        DownNode.relations.Remove(this);
        GameObject.Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        LineUpdate();
    }
}
