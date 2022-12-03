using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class addCommand : Attribute
{
    private string name;

    public virtual string callName
    {
        get
        {
            return name;
        }
    }

    public addCommand(string call)
    {
        name = call;
       
    }



}

public class ConsoleSystem : MonoBehaviour
{
    List<GameObject> CommandObject = new List<GameObject>(); 
    MonoBehaviour Controller;
    InputField input;
    GameObject voschau;
    GameObject interactfield;
    RectTransform interactfieldRec;
    Text interactText;
    float minx = -860, maxx = 5;
    public float InforBlendSpeed = 5000;
    RectTransform vorschauView;
    Text vorschauText;
    public static bool menuAktive;
    private bool cheatOn = false,commandAktiv;
    float timer;
    bool ist;

    string comS = "<b><color=navy>", ckE = "</color></b>";  //aks = "<b><color=lightblue>"
    float offmin;
    
    List<string> commands = new List<string>()
    {
        "//Commands", "//Position"
    };

    public class commandList
    {

        public string name;
        public object target;
        public MethodInfo mi;

        public commandList(string name, MethodInfo mi, object target)
        {
            this.target = target;
            this.name = name;
            this.mi = mi;
        }
    }

    public List<commandList> cList = new List<commandList>();

    // Start is called before the first frame update
    void Start()
    {
     
        input = GetComponent<InputField>();
        voschau = transform.GetChild(2).gameObject;
        vorschauView = transform.GetChild(2).gameObject.GetComponent<RectTransform>();
        vorschauText = transform.GetChild(2).gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Text>();
        interactfield = transform.GetChild(3).gameObject;
        interactfieldRec = transform.GetChild(3).gameObject.GetComponent<RectTransform>();
        interactText = transform.GetChild(3).gameObject.transform.GetChild(0).transform.GetComponent<Text>();
        offmin = vorschauView.sizeDelta.y;

        if (transform.GetComponentInParent<CommandListe>().MovementScript != null) Controller = transform.GetComponentInParent<CommandListe>().MovementScript;
        else Debug.Log("No camera or movement script are setted");

        if (transform.GetComponentInParent<CommandListe>().CommandObject != null)
        {
            var l = transform.GetComponentInParent<CommandListe>().CommandObject;
            for (int i = 0; i < l.Count; i++) CommandObject.Add(l[i]);
        }
        else Debug.Log("Command Gameobject list is Empty");

        getConsolMethods();
    }

    void getConsolMethods()
    {
        List<string> tp = new List<string>();
        List<MonoBehaviour> mon = new List<MonoBehaviour>();
        for (int x = 0; x < CommandObject.Count; x++)
        {
            for (int y = 0; y < CommandObject[x].GetComponents<MonoBehaviour>().Length; y++)
            {
                mon.Add(CommandObject[x].GetComponents<MonoBehaviour>()[y]);
                tp.Add(CommandObject[x].GetComponents<MonoBehaviour>()[y].GetType().Name);

            }
        }

        for (int i = 0; i < mon.Count; i++)
        {
            MethodInfo[] m = mon[i].GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            
            for (int j = 0; j < m.Length; j++)
            {

                if (m[j].GetCustomAttribute<addCommand>() != null)
                {
                    string s = m[j].GetCustomAttribute<addCommand>().callName;

                    for (int v = 0; v < CommandObject.Count; v++) {
                        if (CommandObject[v].GetComponent(tp[i])) {
                            cList.Add(new commandList(s, m[j], CommandObject[v].GetComponent(tp[i])));
                            Debug.Log("add command " + s);
                            commands.Add("//" + s);
                        }
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        
        if (input.text.Length > 0 && !voschau.activeInHierarchy) voschau.SetActive(true);
        string s = "";

        if (input.text.Contains("//"))
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].Contains(input.text))
                {
                    s += commands[i] + "\n";
                }
            }
            vorschauText.text = s;
        }
        else
        {
            vorschauText.text = "";
            if(vorschauView.gameObject.activeInHierarchy) voschau.SetActive(false);
        }
        
    }

    void drawInteract(string text)
    {
        if (!interactfield.activeInHierarchy) interactfield.SetActive(true);
        var r = interactfieldRec.anchoredPosition;
        r.x = minx;
        interactfieldRec.anchoredPosition = r;
        interactText.text = text;
        timer = 0;
        ist = true;

    }

    void drawingInter()
    {
        timer += Time.deltaTime;
        
        var r = interactfieldRec.anchoredPosition;

        if (r.x < maxx)
        {
            r.x += Time.deltaTime * InforBlendSpeed;
            interactfieldRec.anchoredPosition = r;
        }
        else
        {
            if (r.x != maxx)
            {
                r.x = maxx;
                interactfieldRec.anchoredPosition = r;
            }
        }

        if (timer % 60 >= 5)
        {
            ist = false;
            interactfield.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ist) drawingInter();

        if (menuAktive)
        {

            if(Controller != null)if (Controller.enabled)
            {
                Controller.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }


            foreach (string com in commands)
            {
                if (input.text.Equals(com))
                {
                    input.text = input.text.Replace(com, comS + com + ckE);
                    input.caretPosition = input.text.Length;
                    commandAktiv = true;
                }

                if (input.text.Equals((comS + com + ckE).Remove((comS + com + ckE).Length - 1)))
                {
                    input.text = input.text.Replace((comS + com + ckE).Remove((comS + com + ckE).Length - 1), com.Remove(com.Length - 1));
                    input.caretPosition = input.text.Length;
                    commandAktiv = true;
                }

                if (commandAktiv)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (!cheatOn)
                        {
                            if (input.text.ToLower().Equals((comS + "//Commands" + ckE + " true").ToLower()))
                            {
                                cheatOn = true;
                                drawInteract("Commands Activate");
                            }
                        }
                        else
                        {
                            if (input.text.ToLower().Equals((comS + "//Commands" + ckE + " false").ToLower()))
                            {
                                cheatOn = false;
                                drawInteract("Commands Deactivate");
                            }
                            else if (input.text.ToLower().Equals((comS + "//Position" + ckE).ToLower()))
                            {
                                if (Controller != null)
                                {
                                    drawInteract("X = " + Controller.transform.position.x + "  Y = " + Controller.transform.position.y + "  Z = " + Controller.transform.position.z);
                                } 
                            }
                        }

                        if (cheatOn)
                        {
                            for (int i = 0; i < commands.Count; i++)
                            {
                                if (input.text.ToLower().Contains((comS + commands[i] + ckE).ToLower()))
                                {
                                   
                                    for (int j = 0; j < cList.Count; j++)
                                    {
                                        if (cList[j].name.Contains(commands[i].Substring(2)))
                                        {
                                          
                                            int para = cList[j].mi.GetParameters().Length;

                                            string[] t = input.text.Split(' ');

                                            if (t.Length - 1 == para)
                                            {
                                                
                                                List<object> o = new List<object>();

                                                for (int x = 1; x < t.Length; x++)
                                                {

                                                    int ii;
                                                    float ff;
                                                    bool bb;

                                                    if (int.TryParse(t[x], out ii)) o.Add(int.Parse(t[x]));
                                                    else if (float.TryParse(t[x], out ff)) o.Add(float.Parse(t[x]));
                                                    else if (bool.TryParse(t[x], out bb)) o.Add(bool.Parse(t[x]));
                                                    else o.Add(t[x]);

                                                }

                                                cList[j].mi.Invoke(cList[j].target, o.ToArray());
                                                drawInteract(commands[i].Substring(2) + "  Used");

                                            }
                                        }
                                    }
                                }
                            }
                        }
                     

                        input.text = " ";
                        input.Select();
                        input.ActivateInputField();
                    }
                }
            }
        }
    }

    public void activate(bool i)
    {
        if (Controller != null && !i)
        {
            if (!Controller.enabled)
            {
                Debug.Log("Player on");
                Controller.enabled = true;
            }
        }
    }

    public void onend()
    {
        if(!commandAktiv)input.text = "";
    }
}
