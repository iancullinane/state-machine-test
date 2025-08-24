using System;
using Godot;

public partial class Slot : Panel
{

    Item Item;
    int Quantity = 0;
    Texture2D Icon;
    TextureRect IconRect;
    Label QuantityLabel;

    private Color _normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); // Normal brightness
    private Color _hoverColor = new Color(1.3f, 1.3f, 1.3f, 1.0f);

    public override void _Ready()
    {
        IconRect = GetNode<TextureRect>("IconRect");
        QuantityLabel = GetNode<Label>("Quantity");
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        GuiInput += OnGuiInput;
        UpdateDisplay();
    }

    private void OnMouseEntered()
    {
        Modulate = _hoverColor;
    }

    private void OnMouseExited()
    {
        Modulate = _normalColor;
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                GD.Print($"Slot clicked! Item: {(Item != null ? Item.Name : "Empty")}");
            }
        }
    }

    public Item GetItem()
    {
        return Item;
    }

    public int GetQuantity()
    {
        return Quantity;
    }

    public void SetItem(Item item)
    {
        Item = item;
        Quantity++;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        GD.Print("UpdateDisplay");
        if (Item != null)
        {
            Icon = Item.Icon;
            IconRect.Texture = Icon;
            IconRect.Visible = true;
            QuantityLabel.Visible = true;
            QuantityLabel.Text = Quantity.ToString();
        }
        else
        {
            IconRect.Visible = false;
            QuantityLabel.Visible = false;
        }
    }

}
