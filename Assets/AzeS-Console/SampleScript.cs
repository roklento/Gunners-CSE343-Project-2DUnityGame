using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    /*
     * autor:AzeS
     * 
     * to set a command use [addCommand(name)] over of a public method
     * and add the GameObject with the script into the list of a ConC(console)
     * 
     */

    [addCommand("HelloWorld")]
    public void Testfunction(string text, int zahl, float kom, bool i)
    {
        Debug.Log("you´r writh is " + text + " and the integer number is " + zahl + " and your float value is  " + kom + "and the boolean is " + i);
    }

    [addCommand("its")]
    public void TestZwei(bool i)
    {
        Debug.Log("it´s" + i);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
