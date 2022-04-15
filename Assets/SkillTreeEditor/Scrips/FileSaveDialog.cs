using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.IO;


public class FileSaveDialog : MonoBehaviour
{
    private static FileSaveDialog _Instance = null;
    public static FileSaveDialog GetI
    {
        get
        {
            if(_Instance==null)
            {
                //_Instance 
                //_Instance = this;
                return null;
            }
            return _Instance;
        }
    }

    

    VistaSaveFileDialog SaveDialog;
    Stream saveStream = null;
    // Start is called before the first frame update
    void Start()
    {
        SaveDialog = new VistaSaveFileDialog();
        SaveDialog.Filter = "csv files (*.csv) |*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*";
        SaveDialog.FilterIndex = 0;
        SaveDialog.Title = "Save Skill Tree Info";
    }

    private void Awake()
    {
        _Instance = this;
    }
    public string FileSave()
    {
        string FilePath = null;
        if (SaveDialog.ShowDialog() == DialogResult.OK)
        {
            if ((saveStream = SaveDialog.OpenFile()) != null)
            {
                //return OpenDialog.FileName;
                FilePath = SaveDialog.FileName;
            }
        }
        if (FilePath == null)
        {
            return null;
        }
        FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);

        StreamWriter sr = new StreamWriter(fs);
        // str;
        while (true)
        {
            string str = "";
            sr.WriteLine(str);
            if (str == null)
            {
                break;
            }
            Debug.Log(str);

        }

        sr.Close();

        return FilePath;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
