using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.IO;
using System.Text;

public class FileSaveDialog : MonoBehaviour
{
    public List<string[]> rowData = new List<string[]>();
    private static FileSaveDialog _SaveInstance = null;
    public static FileSaveDialog SaveGetI
    {
        get
        {
            if(_SaveInstance == null)
            {
                //_Instance 
                //_Instance = this;
                return null;
            }
            return _SaveInstance;
        }
    }
    private void Awake()
    {
        _SaveInstance = this;
    }


    VistaSaveFileDialog SaveDialog;
    Stream saveStream = null;

    

    // Start is called before the first frame update
    void Start()
    {
        SaveDialog = new VistaSaveFileDialog();
        SaveDialog.Filter = "csv files (*.csv) |*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*";
        SaveDialog.FilterIndex = 0;
        SaveDialog.OverwritePrompt = true;
        SaveDialog.Title = "Save Skill Tree Info";
        //saveFileDialog.InitialDirectory = document;
        // saveFileDialog.FileName = DateTime.Now.ToString("yyyy") + ".ini";
    }

    

    string getRelationString(SkillNode node, bool parent)
    {
        string tempstr = null;
        List<SkillNode> templist = null;
       
        if(parent)
        {
            templist = node.parentNode;
        }
        else
        {
            templist = node.childNode;
        }
        int num = templist.Count;

        if(num==0)
        {
            return "{}";
        }

        tempstr = "{";

        for (int i=0;i<templist.Count;i++)
        {
            if(i==templist.Count-1)
            {
                string temp = string.Format("{0}", templist[i].SkillName);
                tempstr += temp;
                tempstr += "}";
            }
            else
            {
                string temp = string.Format("{0},", templist[i].SkillName);
                tempstr += temp;
            }
            
        }
        Debug.Log("연결정보"+tempstr);
        return tempstr;

    }
    public string FileSave(string classname, List<SkillNode> skillnodes)
    {
        string FilePath = null;

        string[] tempRows = new string[(int)EnumTypes.SkillTreeColums.TreeColumMax];
        tempRows[(int)EnumTypes.SkillTreeColums.SerialNum] = string.Format("{0}_Relation", classname);
        for (EnumTypes.SkillTreeColums i = EnumTypes.SkillTreeColums.SkillName; i < EnumTypes.SkillTreeColums.TreeColumMax; i++)
        {
            tempRows[(int)i] = $"{i.ToString()}";
        }
        rowData.Add(tempRows);


        int RowNum = skillnodes.Count;

        for (int i = 0; i < RowNum; i++)
        {
            tempRows = new string[(int)EnumTypes.SkillTreeColums.TreeColumMax];
            tempRows[(int)EnumTypes.SkillTreeColums.SerialNum] = i.ToString();
            tempRows[(int)EnumTypes.SkillTreeColums.SkillName] = skillnodes[i].SkillName;
            tempRows[(int)EnumTypes.SkillTreeColums.SkillRank] = skillnodes[i].SkillRank.ToString();
            tempRows[(int)EnumTypes.SkillTreeColums.SkillDamage] = skillnodes[i].SkillDamage.ToString();
            tempRows[(int)EnumTypes.SkillTreeColums.ReauireLevel] = skillnodes[i].RequireLevel.ToString();
            tempRows[(int)EnumTypes.SkillTreeColums.SkillExplain] = skillnodes[i].SkillExplain;
            tempRows[(int)EnumTypes.SkillTreeColums.Parent] = getRelationString(skillnodes[i], true);
            tempRows[(int)EnumTypes.SkillTreeColums.Child] = getRelationString(skillnodes[i], false);
            tempRows[(int)EnumTypes.SkillTreeColums.Index] = string.Format("{{{0},{1}}}", skillnodes[i].index.x, skillnodes[i].index.y);
            rowData.Add(tempRows);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
            Debug.Log("row ->"+rowData[i]);
        }


        //FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Write);
        FilePath = UnityEngine.Application.dataPath + "/SkillInfo/" + string.Format("{0}_Relation.csv",classname);
        StreamWriter sr = System.IO.File.CreateText(FilePath);
        //StreamWriter sr = new StreamWriter(fs);
        int length = output.GetLength(0);
        string delimiter = ",";


        StringBuilder sb = new StringBuilder();
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        Debug.Log(sb.ToString());


        sr.WriteLine(sb);

        sr.Close();

        return FilePath;
    }


}
