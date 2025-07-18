using Godot;
using System;

public partial class InteractionController : RayCast3D
{

    Label interaction_prompt;

    public override void _Ready()
    {
        interaction_prompt = GetNode<Label>("PromptLabel");
    }

    public override void _Process(double delta)
    {
        GodotObject game_object = GetCollider();
        interaction_prompt.Text = "";

        if (game_object != null && game_object is IInteractable interactable)
        {
            interaction_prompt.Text = "[E] " + interactable.interaction_prompt;

            if (Input.IsActionJustPressed("interact"))
            {
                interactable._Interact();
            }
        }
    }
}
