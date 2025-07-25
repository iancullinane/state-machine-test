using Godot;
using System;

public partial class Pedestal : Interactable
{

    Node3D light;

    public override void _Ready()
    {
        light = GetNode<Node3D>("LightBulb");
    }

    public override void _Interact()
    {
        light.Visible = !light.Visible;
    }
}
