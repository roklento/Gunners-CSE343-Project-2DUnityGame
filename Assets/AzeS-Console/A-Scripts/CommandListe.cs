using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommandListe : MonoBehaviour
{
    [Tooltip("The List of GameObjects thats have a Monobehaviour Script with commands")]
    public List<GameObject> CommandObject = new List<GameObject>();

    [Tooltip("The Monobehaviour script thas move your player / camera")]
    public MonoBehaviour MovementScript;

    public KeyCode key = KeyCode.LeftAlt;
    public GameObject input;


    void Update()
    {
        if (Input.GetKeyDown(key))
        {

            ConsoleSystem.menuAktive = !ConsoleSystem.menuAktive;
            input.GetComponent<ConsoleSystem>().activate(ConsoleSystem.menuAktive);
            input.SetActive(ConsoleSystem.menuAktive);
        }
    }
}
