using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using WebApi;
using Bopper.View.Unity;


public enum GameState { Invalid, Title, Login, Play, Help };

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [Obsolete("loginPanel is deprecated, please use login instead.")]
    [SerializeField] GameObject loginPanel;
    [SerializeField] Login login;
    [SerializeField] GameObject helpPanel;
    [SerializeField] Button fetchButton;
    [SerializeField] public Material redMaterial;
    [SerializeField] Bopper.LogWindow logWindow;

    public Matchstate matchstate = new Matchstate();

    public static GameManager instance;
    [HideInInspector]
    public GameData gameData;
    public NotificationList<Command> commands;
    public CommandController commandController;

    public GameState state;
    public bool draggingGizmo = false;

    private void Awake()
    {
        commands = commandController.commands;
    }


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetState(GameState.Title);
        gameData = GetComponent<GameData>();
        Command.view = GetComponent<GameView>();
    }

    public void SetStateTitle()
    {
        SetState(GameState.Title);
    }

    public void SetStateHelp()
    {
        SetState(GameState.Help);
    }

    public void Play()
    {
        AuthenticateAndPlay();
    }
    
    private void AuthenticateAndPlay()
    {
        login.Authenticate(AuthenticateAndPlayCallback);
    }

    private void AuthenticateAndPlayCallback(bool success)
    {
        if (success)
            SetState(GameState.Play);
        //SetState(success ? GameState.Play : GameState.Login);
    }

    public void SetState(GameState state)
    {
        this.state = state;

        titlePanel.SetActive(state == GameState.Title);
        loginPanel.SetActive(state == GameState.Login);
        helpPanel.SetActive(state == GameState.Help);

        fetchButton.interactable = WebApi.AuthToken.HasToken();
    }


    public void LoginAndPlayCallback()
    {

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

    /// <summary>
    /// We have a commandString (which is apparently a list of commands [of which Chat's are commands too])
    /// </summary>
    /// <param name="commandString"></param>
    public void xLoadMatchstateCommands(string commandSetText)
    {
        commandController.LoadCommandSet(commandSetText);
        /*
        using (var sr = new StringReader(commandSetText))
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
                    //matchstate = command.Execute(matchstate);
                    // Create the text for a LogCommand
                    // TODO:   These commands are being discarded. They should be loaded into Log.  Execute should not be run here (at least not for the new turn)
                    //Bopper.LogItem logitem = LogItemFactory.MakeLogItem(command);
                    logWindow.Add(command);
                    count++;
                }
            }

            Debug.Log($"Commands: {count}/{numlines} parsed\n");
        }
        */
    }


    public void LoadSampleData(string commandSet = null)
    {
        commandController.LoadCommandSet(commandSet ?? Bopper.BopperData.commandset);
    }
}
