using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using TMPro;

public class planestamp : MonoBehaviour
{
    private Vector3 stampos;
    private GameObject stamp;
    SerialPort arduino;
    int px;
    int py;
    object[,] correct = { {1, 2}, {2, 6}, {4, 4}, {6, 7}, {8, 3} };
    int correctamount;
    int wrongamount;
    string serial;
    float delaybetweenwrong = 5f;
    private float timetorefresh;
    GameObject temp;
    TMP_Text reboottext;
    // B2, J2, F4, N6, D8 the correct is just translated for ease of use
    void Start()
    {
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {               
            arduino = new SerialPort(port, 9600);
            Debug.Log(port);
        }
        stamp = GameObject.Find("stamp");
        Debug.Log(stamp);
        arduino.ReadTimeout = 10000;
        arduino.Open();
        reboottext = GameObject.Find("reboot").GetComponent<TextMeshProUGUI>();
        Debug.Log(reboottext);
    }
    void Update()
    {
        GameObject.Find("spin").transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, -1);
        if (timetorefresh >= Time.time) {
            reboottext.text = "Rebooting " + (Mathf.Round((timetorefresh - Time.time)*10)/10).ToString();
            arduino.ReadLine();
            goto endearly;
        }
        reboottext.text = string.Empty;
        serial = arduino.ReadLine();
        if (serial == "up") GameObject.Find("stamp").transform.position += new Vector3(0, 0.8f, 0);
        if (serial == "down") GameObject.Find("stamp").transform.position -= new Vector3(0, 0.8f, 0);
        if (serial == "left") GameObject.Find("stamp").transform.position += new Vector3(0.8f, 0, 0);
        if (serial == "right") GameObject.Find("stamp").transform.position -= new Vector3(0.8f, 0, 0);
        if (serial == "guess")
        {
            stampos = stamp.transform.position;
            px = (int)convertx(stampos.x);
            py = (int)converty(stampos.y);
            for (int i = 0; i <= 4; i++)
            {
                int cx = (int)correct[i, 0];
                int cy = (int)correct[i, 1];
                if (px == cx && py == cy)
                {
                    temp = Instantiate(GameObject.Find("plane right template"), stampos, stamp.transform.rotation, GameObject.Find("stamped").transform);
                    correctamount += 1;
                    if (correctamount == 5)
                    {
                        arduino.Write("1");
                    }
                    goto gohere;
                }
                else
                {
                    if (i == 4)
                    {
                        temp = Instantiate(GameObject.Find("plane wrong template"), stampos, stamp.transform.rotation, GameObject.Find("stamped").transform);
                        temp.tag = "wrong";
                        wrongamount += 1;
                        if (wrongamount >= 3)
                        {
                            clearscreen();
                        }
                    }
                }
            }
        gohere:;
        }
    endearly:;
    }
    static object convertx(float x) {
        x = (Mathf.Round(x*1.25f + 4.01f));
        x = (int)x;
        return (int)x;
    }
    static object converty(float y) {
        y = (Mathf.Round(y*1.25f + 4.01f));
        y = (int)y;
        return (int)y;
    }
    void clearscreen()
    {
        GameObject[] deletethis = GameObject.FindGameObjectsWithTag("wrong");
        Debug.Log(deletethis);
        foreach (GameObject getridofit in deletethis)
            Destroy(getridofit);
        wrongamount = 0;
        timetorefresh = delaybetweenwrong + Time.time;
        delaybetweenwrong += 5f;
        Debug.Log(delaybetweenwrong);
    }
}
