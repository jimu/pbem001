using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using RegexMatch = System.Text.RegularExpressions.Match;
using Bopper.Commands;

namespace Bopper
{

	public class LogWindow : MonoBehaviour
    {
		public TMP_InputField chatInputField;
		public UnityEngine.UI.Button chatSubmitButton;
		public BopperAdapter adapter;

		int player_id = 1;

        public void Start()
        {
			chatInputField.onValueChanged.AddListener(OnChatInputChanged);
			chatSubmitButton.onClick.AddListener(OnChatSubmitPressed);
			OnChatInputChanged(chatInputField.text);

			chatInputField.onSubmit.AddListener(OnChatSubmit);

			// LogWindow will be notified of viewholder clicks
			adapter.clickAction += ItemClicked;
			adapter.commandController.logListeners += OnCurrentItemChanged;
		}

		public void OnCurrentItemChanged()
        {
			SetCurrent(adapter.commandController.GetCurrentIndex());
            //Debug.Log($"LogWindow.OnCurrentItemChanged()");
        }

		public void OnChatSubmit(string text)
		{
			if (text.Length > 0)
			{
				adapter.commandController.commands.Add(CommandFactory.Make(text) ?? new CommandSay(player_id, text));
				chatInputField.text = "";
			}
		}

		public void OnChatSubmitPressed()
        {
			OnChatSubmit(chatInputField.text);
		}

		public void OnChatInputChanged(string value)
        {
			chatSubmitButton.interactable = value.Length > 0;
        }

		public void ItemClicked(int index)
        {
			//Debug.Log($"LogWindow.ItemClicked({index})");
			adapter.commandController.Goto(index);
		}



		// This is called when a LogItem is clicked
		public void SetCurrent(int index)
		{
			adapter.SetCurrent(index);
			adapter.UpdateSelection();
		}

	}
}