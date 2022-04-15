using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.IO;

public class FileOpenDialog : MonoBehaviour
{
    private static FileOpenDialog _Instance = null;
    public static FileOpenDialog GetI
    {
        get
        {
            if (_Instance == null)
            {
                //_Instance 
                //_Instance = this;
                return null;
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        _Instance = this;
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
            if (str == null)
            {
                break;
            }

            var tempstr = str.Split(',');
            int colums = (int)EnumTypes.SkillInfoColums.Colummax;

            //List<string> columlist = new List<string>();
            Dictionary<int, string> columsdic = new Dictionary<int, string>();
            for (int i=1;i<=colums; i++)//enum에 정의된 범위를 넘어가는 데이터는 버려진다.
            {
                if(tempstr[i]!=null)
                    columsdic.Add(i-1,tempstr[i]);
            }

            //var Row = new Dictionary<int, List<string>>();
            int RowNum = 0;
            //string classname;
            if(int.TryParse(tempstr[0], out RowNum))//첫번째가 숫자가 아니면 해당 행은 열제목 행이다.
            {
                data.Add(columsdic);
            }
            else
            {
                classname = tempstr[0];
            }


            Debug.Log(str);

        }
        int a = 0;
        return data; 
    }

    public string TreeFileOpen()
    {
        string FilePath = null;
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
        // str;
        while (true)
        {
            string str = sr.ReadLine();
            if (str == null)
            {
                break;
            }
            Debug.Log(str);

        }
        //string str = sr.ReadToEnd();

        //var data_values = data_String.Split(',');
        return FilePath;
        //return null;
    }


    //private void OnGUI()
    //{
    //    if(GUI.Button(new Rect(100,100,100,50),"FileOpen"))
    //    {
    //        string fileName = FileOpen();

    //        if(!string.IsNullOrEmpty(fileName))
    //        {
    //            Debug.Log(fileName);
    //        }
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
