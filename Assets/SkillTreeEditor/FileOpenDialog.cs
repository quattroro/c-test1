using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.IO;

public class FileOpenDialog : MonoBehaviour
{
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

    public string FileOpen()
    {
        string FilePath = null;
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
            Debug.Log(str);

        }
        //string str = sr.ReadToEnd();

        //var data_values = data_String.Split(',');
        return FilePath; 
        //return null;
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100,100,100,50),"FileOpen"))
        {
            string fileName = FileOpen();

            if(!string.IsNullOrEmpty(fileName))
            {
                Debug.Log(fileName);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
