using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;


public enum GameState { Invalid, Title, Login, Play, Help };

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject helpPanel;
    [SerializeField] Button fetchButton;
    [SerializeField] public Material redMaterial;

    public Matchstate matchstate = new Matchstate();

    public static GameManager instance;
    public GameData gameData;

   
    public GameState state;
    public bool draggingGizmo = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetState(GameState.Title);
        gameData = GetComponent<GameData>();
        Command.view = GetComponent<View>();
    }

    public void SetStateTitle()
    {
        SetState(GameState.Title);
    }
    public void SetStateHelp()
    {
        SetState(GameState.Help);
    }
    public void SetStatePlay()
    {
        SetState(GameState.Login);
    }

    public void SetState(GameState state)
    {
        this.state = state;

        titlePanel.SetActive(state == GameState.Title);
        loginPanel.SetActive(state == GameState.Login);
        helpPanel.SetActive(state == GameState.Help);

        fetchButton.interactable = WebApi.AuthToken.HasToken();
    }

    public void FetchMatchstate()
    {
        Debug.Log($"Fetch Matchstate");
        GetMatchstate.Get(1);
    }

    public void DumpState()
    {
        Debug.Log($"Dump State");
        matchstate.Dump();
    }

    public void LoadMatchstateCommands(string commandString)
    {
        using (var sr = new StringReader(commandString))
        {
            string line;
            int count = 0;
            int numlines = 0;
            while ((line = sr.ReadLine()) != null)
            {
                numlines++;
                Command command = CommandFactory.Make(line);
                if (command != null)
                {
                    matchstate = command.Execute(matchstate);
                    count++;
                }
            }

            Debug.Log($"Commands: {count}/{numlines} parsed\n");
        }

    }
}
