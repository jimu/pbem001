using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;


namespace Bopper.View
{
    /// <summary>
    /// Processes commands from a text input
    /// </summary>
    public class TextInput : MonoBehaviour
    {
        const string VERSION = "v0.0.1";

        [Header("Why?")]        
        [SerializeField] BopperAdapter adapter;
        [SerializeField] QuantumConsole console;
        [SerializeField] CommandController commandController;

        NotificationList<Command> commands;

        public void Start()
        {
            commands = commandController.commands;
            commands.listeners += OnDataChanged;
        }



        public void OnDataChanged(int index, int count)
        {
            //Debug.Log($"OnDataChanged({index},{count}) last:{commands.Count - 1} show:{index == commands.Count - 1}");

            if (count == 1 && index == commands.Count - 1)
                console.LogToConsole($"{commands[index]}");
        }

        [Command("echo", "Echo message for testing")]
        public string echo(string message)
        {
            return message;
        }

        [Command("version", "Returns version string")]
        public string version()
        {
            return VERSION;
        }

        [Command("showlog", "Shows the game log")]
        public string ShowLog()
        {
            string output = "";
            foreach (var command in commands)  // This needs to get at the command list in much the same way that LogWindow/adapter.Data "data helper" does.  If the source changes, both LogWindow.adapter.Data and this thing will be accessing the SAME LIST
                output += command.ToString() + "\n";
            return output;
        }

        [Command("next", "Advances the log")]
        public string Next()
        {
            return "Next() - TODO";
        }
    }
}