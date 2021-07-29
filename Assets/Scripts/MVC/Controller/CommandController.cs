using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bopper;
using Bopper.Commands;

/// <summary>
/// The CommandController is responsible for maintaining the Command list, controlling the current command index, and excuting commands
/// </summary>
/// <remarks>
/// Responsibilities:
/// * Maintains pointer into CommandList
/// * next, prev, top, bottom, goto
/// * play?
/// * SetPlaybackRate?
/// </ remarks>
public class CommandController : MonoBehaviour
{
    public NotificationList<Command> commands = new NotificationList<Command>();

    int firstVisibleIndex;  // typically the beginning of the current turn
    int lastVisibleIndex;   // typically the last command played back so the player doesn't see future log entries prematurely
    int currentIndex;       // command associated with the currently displayed matchstate

    public UnityAction logListeners;

    public void LoadCommandSet(string commandSetText)
    {
        foreach (var item in CommandFactory.MakeMany(commandSetText))
            commands.Add(item);
        currentIndex = -1;
        First();
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    public int EndIndex()
    {
        return commands.Count;
    }

    public void Next()
    {
        NextWithReveal();
    }
    public void NextWithReveal()
    {
        if (AtEnd() && commands.AnyHiddenAtEnd())
            commands.RevealAtEnd(1);

        Goto(currentIndex + 1);
    }

    public bool AtEnd()
    {
        return currentIndex == EndIndex() - 1;
    }

    public void Prev()
    {
        Goto(currentIndex - 1);
    }

    public void First()
    {
        Goto(0);
    }


    public void Last()
    {
       Goto(EndIndex() - 1);
    }

    public void Goto(int index)
    {
        if (currentIndex != index && index >= 0 && index < EndIndex())
        {
            PlayTo(index);
            currentIndex = index;
            //Debug.Log($"CC: Informing listeners of index change ({index})");
            logListeners?.Invoke();
            //Debug.Log($"CC: DONE");
        }
    }

    public void PlayTo(int index)
    {
        if (index >= currentIndex)
            PlayForwardTo(index);
        else
            PlayBackTo(index);
    }

    public void PlayForwardTo(int index)
    {
        while (currentIndex < index)
        {
            currentIndex++;
            //Debug.Log($"PlayForwardTo({commands[currentIndex]})");
            GameManager.instance.matchstate = commands[currentIndex].Execute(GameManager.instance.matchstate);
            logListeners?.Invoke();
        }
    }


    public void PlayBackTo(int index)
    {
        while (currentIndex > index)
        {
            GameManager.instance.matchstate = commands[currentIndex].Undo(GameManager.instance.matchstate);
            currentIndex--;
            // todo - we could start animations and display status messages for the current record (which is already reflected in the state)
            logListeners?.Invoke();
        }
    }
}
