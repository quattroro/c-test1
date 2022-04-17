using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.IO;

public class FileOpenDialog : MonoBehaviour
{
    private static FileOpenDialog _OPenInstance = null;
    public List<string> originalstr = null; 
    public static FileOpenDialog OpenGetI
    {
        get
        {
            if (_OPenInstance == null)
            {
                //_Instance 
                //_Instance = this;
                return null;
            }
            return _OPenInstance;
        }
    }

    private void Awake()
    {
        _OPenInstance = this;
    }

    VistaOpenFileDialog OpenDialog;
    Stream openStream = null;

    // Start is called before the first frame update
    void Start()
    {
        OpenDialog = new VistaOpenFileDialog();
        OpenDialog.Filter = "csv files (*.csv) |*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*";
        OpenDialog.FilterIndex = 0;
        OpenDialog.Title = "Skill Tree Info";
        
    }



    
    public List<Dictionary<int, string>> SkillListFileOpen(out string classname)
    {
        originalstr.Clear();
        List<Dictionary<int, string>> data = new List<Dictionary<int, string>>();
        string FilePath = null;
        classname = null;
        if(OpenDialog.ShowDialog() == DialogResult.OK)
        {
            if((openStream = OpenDialog.OpenFile())!=null)
            {
                //return OpenDialog.FileName;
                FilePath = OpenDialog.FileName;
            }
        }
        if(FilePath==null)
        {
            return null;
        }


        FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
       
        StreamReader sr = new StreamReader(fs);
        // str;
        while(true)
        {
            string str = sr.ReadLine();
            originalstr.Add(str);
            if (str == null)
            {
                break;
            }

            var tempstr = str.Split(',');
            int colums = (int)EnumTypes.SkillInfoColums.Colummax;

            Dictionary<int, string> columsdic = new Dictionary<int, string>();
            for (int i=1;i<=colums; i++)//enum에 정의된 범위를 넘어가는 데이터는 버려진다.
            {
                if(tempstr[i]!=null)
                    columsdic.Add(i-1,tempstr[i]);
            }

            int RowNum = 0;

            if(int.TryParse(tempstr[0], out RowNum))//첫번째가 숫자가 아니면 해당 행은 열제목 행이다.
            {
                data.Add(columsdic);
            }
            else
            {
                classname = tempstr[0];
            }


        }
        int a = 0;
        return data; 
    }

    //스킬정보와 스킬트리정보를 받아온다.
    public List<Dictionary<int, string>> TreeFileOpen(out string classname)
    {
        originalstr.Clear();
        List<Dictionary<int, string>> data = new List<Dictionary<int, string>>();
        string FilePath = null;
        classname = null;
        if (OpenDialog.ShowDialog() == DialogResult.OK)
        {
            if ((openStream = OpenDialog.OpenFile()) != null)
            {
                //return OpenDialog.FileName;
                FilePath = OpenDialog.FileName;
            }
        }
        if (FilePath == null)
        {
            return null;
        }


        FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

        StreamReader sr = new StreamReader(fs);

        while (true)
        {
            string str = sr.ReadLine();
            originalstr.Add(str);
            if (str == null || str.Length == 0)
            {
                break;
            }
            char[] temp = new char[str.Length];
            bool flag = false;

            Dictionary<int, string> columsdic = new Dictionary<int, string>();
            int Dicindex = 0;
            int index = 0;
            //string temp;
            for (int i=0;i<str.Length;i++)
            {
                if (str[i] == '{')
                {
                    flag = true;//여는중괄호가 나오면 닫는중괄호가 나올때까지 앞으로 나오는 , 는 그냥 집어넣는다.  
                    continue;
                }
                else if (str[i] == ',')
                {
                    if (!flag)
                    {
                        temp[index] = '\0';
                        string tt = string.Join("", temp);
                        columsdic.Add(Dicindex++, tt.Split('\0')[0]);
                        temp = new char[str.Length];
                        index = 0;
                        continue;
                    }
                }
                else if (i == str.Length - 1)
                {
                    if(str[i]!='}')
                        temp[index++] = str[i];

                    string tt = string.Join("", temp);
                    columsdic.Add(Dicindex++, tt.Split('\0')[0]);
                    temp = new char[str.Length];
                    index = 0;
                    break;
                }
                else if (str[i] == '}') 
                {
                    flag = false;
                    continue;
                }
                
                
                temp[index++] = str[i];

            }


            int RowNum = 0;

            if (int.TryParse(columsdic[0], out RowNum))//첫번째가 숫자가 아니면 해당 행은 열제목 행이다. 리스트에 넣지 않는다.
            {
                data.Add(columsdic);
            }
            else
            {
                classname = columsdic[0];
            }

        }
        int a = 10;
        return data;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
