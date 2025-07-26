using System;
using Godot;

public partial class Slot : Panel
{
    private Color _normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); // Normal brightness
    private Color _hoverColor = new Color(1.3f, 1.3f, 1.3f, 1.0f);

    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    private void OnMouseEntered()
    {
        Modulate = _hoverColor;
    }

    private void OnMouseExited()
    {
        Modulate = _normalColor;
    }
}
