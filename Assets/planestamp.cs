using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class planestamp : MonoBehaviour
{
    private Vector3 stampos;
    private GameObject stamp;
    SerialPort arduino;
    int px;
    int py;
    object[,] correct = { { 8, 3 }, { 2, 6 }, { 4, 4 }, { 1, 1 }, { 5, 3 } };
    int correctamount;
    string serial;
    private float delaybetweenguesses = 5f;
    private float timetorefresh;
    private GameObject wrongguess;
    // B3, N6, G4, P1 F3, the correct is just translated for ease of use
    void Start()
    {
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {               
            arduino = new SerialPort(port, 9600);
            Debug.Log(port);
        }
        stamp = GameObject.Find("stamp");
        arduino.ReadTimeout = 10000;
        arduino.Open();
    }
    void Update()
    {
        if (timetorefresh >= Time.time) {
            arduino.ReadLine();
            goto endearly;
        }
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
                    Instantiate(GameObject.Find("plane right template"), stampos, stamp.transform.rotation, GameObject.Find("stamped").transform);
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
                        wrongguess = Instantiate(GameObject.Find("plane wrong template"), stampos, stamp.transform.rotation, GameObject.Find("stamped").transform);
                        timetorefresh = Time.time + delaybetweenguesses;
                    }
                }
            }
        gohere:
            Debug.Log("right");
        }
    endearly:;
    }
    static object convertx(float x) {
        x = (Mathf.Round((x - .8712f) / 1.7424f) + 5);
        x = (int)x;
        return (int)x;
    }
    static object converty(float y) {
        y = (Mathf.Round((y - .55f) / .98f) + 5);
        y = (int)y;
        return (int)y;
    }
}
