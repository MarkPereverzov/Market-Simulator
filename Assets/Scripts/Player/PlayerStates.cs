using UnityEngine;
public abstract class PlayerState
{
    protected PlayerController player;

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    public virtual void HandleInput()
    {
        IInteractable interactable = player.FindInteractable();
        if (interactable != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InteractWithObject(interactable);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CommandManager.Instance.UndoLastCommand();
        }
    }
    public abstract void InteractWithObject(IInteractable interactable);
}

public class FreeState : PlayerState
{
    public FreeState(PlayerController player) : base(player) { }

    //public override void HandleInput()
    //{
    //    //IInteractable interactable = player.FindInteractable();
    //    //if (interactable != null)
    //    //{
    //        //if (Input.GetKeyDown(KeyCode.F))
    //        //{
    //        //    CommandManager.Instance.ExecuteCommand(new DropCommand(player, player.HoldItem));
    //        //    //InteractWithObject(interactable);
    //        //}
    //    //}
    //}

    public override void InteractWithObject(IInteractable interactable)
    {
        interactable.Interact(player);
    }
}


public class HoldingItemState : PlayerState
{
    public HoldingItemState(PlayerController player) : base(player) { }

    public override void HandleInput()
    {
        IInteractable interactable = player.FindInteractable();
        if (interactable != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InteractWithObject(interactable);
            }
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CommandManager.Instance.ExecuteCommand(new DropCommand(player, player.HoldItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CommandManager.Instance.UndoLastCommand();
        }
        
    }

    public override void InteractWithObject(IInteractable interactable)
    {
        interactable.Interact(player);
    }
}

