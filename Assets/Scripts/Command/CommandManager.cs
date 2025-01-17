using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    private Stack<Command> commandStack = new Stack<Command>();

    private void Awake()
    {
        // Проверяем, существует ли уже экземпляр
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Another instance of CommandManager exists. Destroying this one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void ExecuteCommand(Command command)
    {
        command.Execute();
        commandStack.Push(command);
    }

    public void UndoLastCommand()
    {
        if (commandStack.Count > 0)
        {
            Command lastCommand = commandStack.Pop();
            lastCommand.Undo();
        }
    }
}