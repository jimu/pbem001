using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using Bopper.Commands;


namespace Bopper.View
{
    /// <summary>
    /// Processes commands from a text input
    /// </summary>
    public class TextView : MonoBehaviour
    {
        const string VERSION = "v0.0.1";

        [SerializeField] QuantumConsole console;
        [SerializeField] CommandController commandController;

        NotificationList<Command> commands;

        private void Awake()
        {
            commands = commandController.commands;
            commands.listeners += OnDataChanged;
            commandController.logListeners += OnIndexChanged;
        }

        public void OnDataChanged(int index, int count)
        {
            //Debug.Log($"OnDataChanged({index},{count}) last:{commands.Count - 1} show:{index == commands.Count - 1}");
            /*
            if (count == 1 && index == commands.Count - 1)
                console.LogToConsole($"{commands[index]}");
            */
        }

        public void OnIndexChanged()
        {
            //Debug.Log($"OnIndexChanged()");

            console.LogToConsole($"{commands[commandController.GetCurrentIndex()]}");
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
            commandController.Next();
            return "Next...";
        }

        [Command("prev", "Rewinds log one command")]
        public string Prev()
        {
            commandController.Prev();
            return "Prev...";
        }

        [Command("first", "Rewinds log to beginning")]
        public string First()
        {
            commandController.First();
            return "First...";
        }

        [Command("last", "Fast forwards the log to the end")]
        public string Last()
        {
            commandController.Last();
            return "Last...";
        }
    }
}