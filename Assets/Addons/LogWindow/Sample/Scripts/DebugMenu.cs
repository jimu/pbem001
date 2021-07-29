using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bopper.Commands;

namespace Bopper {
    public class DebugMenu : MonoBehaviour
    {
        public LogWindow logWindow;
        [SerializeField] CommandController commandController;
        int player_id = 1;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                commandController.Prev();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                commandController.Next();
            if (Input.GetKeyDown(KeyCode.Home))
                commandController.First();
            if (Input.GetKeyDown(KeyCode.End))
                commandController.Last();

        }
        public void TogglePlayer()
        {
            player_id = player_id % 2 + 1;
        }

        //public void OnInit() { logWindow.ResetItemsPressed(); }
        public void ResetItemsPressed()
        {
            //adapter.SetCurrentTop();      // TODO (is this important???)
            //adapter.data.Clear();
            //commandController.commands.AddRange(BopperData.commands);
            //adapter.UpdateSelection();
        }
        public void OnAddCommand() { commandController.commands.Add(new CommandDeploy(1, UnitType.JB, Random.Range(1000, 10000))); }
        public void OnAddChat()    { commandController.commands.Add(new CommandSay(player_id, $"This is a sample chat message from the computer to the computer so how do you like that?")); }
        public void OnAddPhase()   { commandController.commands.Add(new CommandPhase(0, $"Phase {Random.Range(1, 100)}")); }
        public void OnFirst() { commandController.First(); }
        public void OnPrev() { commandController.Prev(); }
        public void OnNext() { commandController.Next(); }
        public void OnLast() { commandController.Last(); }
        public void OnPlayer() { TogglePlayer(); }
        public void OnUndo()
        {
            commandController.commands.RemoveAt(commandController.commands.Count - 1);
            commandController.Last();
        }
    }
}