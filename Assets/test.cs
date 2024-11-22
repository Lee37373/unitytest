using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using UnityEngine.UI;

public class test : MonoBehaviour
{
    const int IN = 0;
    const int OUT = 1;
    
    const int LOW = 0;
    const int HIGH = 1;
    
    const int PIN = 20;
    const int POUT2 = 21;
    const int VALUE_MAX = 40;
    const int DIRECTION_MAX = 40;
    
    public Text text;
    
    void Start()
    {
        if(GPIOExport(POUT2) == -1 || GPIOExport(PIN) == 0)
            text.text = "err1";
        if(GPIODirection(POUT2, OUT) == -1 || GPIODirection(PIN, IN) == -1)
            text.text = "err2";
    }

    void Update()
    {
        if(GPIOWrite(POUT2, 1) == -1)
        {
            text.text = "err3";
        }
        else
            text.text = "GPIORead: "+GPIORead(PIN)+"from pin "+PIN;

        if(GPIOUnexport(POUT2) == -1 || GPIOUnexport(PIN) == -1)
            text.text = "err4";

    }

    public void EXIT()
    {
        Application.Quit();
    }
    

    int GPIOExport(int pin)
    {
        string path = "/sys/class/gpio/export";
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));

        if (!directoryInfo.Exists)
        {
            text.text = "Failed to open export for writing!";
            return -1;
        }

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);
        writer.WriteLine(pin);
        writer.Close();
        
        return 0;
    }

    int GPIOUnexport(int pin)
    {
        string path = "/sys/class/gpio/unexport";
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));

        if(!directoryInfo.Exists)
        {
            text.text = "Failed to open unexport for writing!";
            return -1;
        }

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);
        writer.WriteLine(pin);
        writer.Close();
        
        return 0;
    }
    int GPIODirection(int pin, int dir) {
        string[] s_directions_str = {"in","out"};
        string path = "/sys/class/gpio/gpio"+pin+"/direction";

        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
        if(!directoryInfo.Exists)
        {
            text.text = "Failed to open gpio direction for writing!";
            return -1;
        }
        
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);
        writer.WriteLine(s_directions_str[dir]);
        writer.Close();
        return (0);
    }
    int GPIORead(int pin) {
        string path = "/sys/class/gpio/gpio"+pin+"/value";
        
        FileInfo fileInfo = new FileInfo(path);

        if(fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(path);
            string value = reader.ReadToEnd();
            text.text = value;
            reader.Close();
            return 0;
        }
        else
        {
            text.text = "Failed to open gpio value for reading!";
            return -1;
        }
    }
    int GPIOWrite(int pin, int value) {
        string[] s_values_str = {"0","1"};

        string path = "/sys/class/gpio/gpio"+pin+"/value";

        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
        if(!directoryInfo.Exists)
        {
            text.text = "Failed to open gpio value for writing!";
            return -1;
        }
        
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);
        writer.WriteLine(s_values_str[LOW == value ? 0 : 1]);
        writer.Close();
        return 0;
    }

}
