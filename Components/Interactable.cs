using Godot;
using System;

public interface IInteractable
{
    string interaction_prompt { get; set; }
    bool is_interactable { get; set; }
    void _Interact();
}

public partial class Interactable : Node3D, IInteractable
{
    [Export]
    public string interaction_prompt { get; set; } = "Interact";
    public bool is_interactable { get; set; } = true;

    public virtual void _Interact()
    {
        GD.Print("Override");
    }
}
