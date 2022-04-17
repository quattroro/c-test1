using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : MonoBehaviour
{
    protected RectTransform imageRectTransform;
    public float lineWidth = 113f;
    public Vector3[] point = new Vector3[2];
    // Use this for initialization
    protected virtual void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    public void SetPosition(int index, Vector3 Pos)
    {
        point[index] = Pos;
    }

    public virtual void LineUpdate()
    {
        Vector3 differenceVector = point[1] - point[0];
        if (differenceVector != new Vector3(0, 0, 0))
        {
            imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            imageRectTransform.pivot = new Vector2(0, 0.5f);
            imageRectTransform.position = point[0];
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        LineUpdate();
    }

}
