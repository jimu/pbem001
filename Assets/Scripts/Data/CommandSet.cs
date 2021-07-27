using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

[CreateAssetMenu(fileName = "data\\CommandSets\\New CommandSet", menuName = "Command Set")]
public class CommandSet : ScriptableObject
{
    public string commandSet;

    public void Load()
    {
        GameManager.instance.LoadSampleData(commandSet);
    }
}
