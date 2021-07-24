using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Responsibilities:
/// * Maintains pointer into CommandList
/// * next, prev, top, bottom, goto
/// * play?
/// * SetPlaybackRate?
/// </summary>
public class CommandController : MonoBehaviour
{
    public NotificationList<Command> commands = new NotificationList<Command>();

    int firstVisibleIndex;  // typically the beginning of the current turn
    int lastVisibleIndex;   // typically the last command played back so the player doesn't see future log entries prematurely
    int currentIndex;       // command associated with the currently displayed matchstate

    public UnityAction listeners;

    public void LoadCommandSet(string commandSetText)
    {
        foreach (var item in CommandFactory.MakeMany(commandSetText))
            commands.Add(item);
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
        if (AtEnd() && commands.AreMore())
            commands.Reveal(1);

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
            currentIndex = index;
            Debug.Log($"CC: Informing listeners of index change ({index})");
            listeners?.Invoke();
            Debug.Log($"CC: DONE");
        }
    }
}
