using Godot;

public partial class Hud : Node
{

    Panel _inventory;

    public override void _Ready()
    {

        _inventory = GetNode<Panel>("Inventory");
        if (_inventory != null)
        {
            GD.Print($"Initial inventory visibility: {_inventory.Visible}");
        }
        ToggleWindow(false);

        // Connect to the global signal
        if (Signals.Instance != null)
        {
            Signals.Instance.GivePlayerItem += OnGivePlayerItem;
            GD.Print("Connected to GivePlayerItem signal");
        }
        else
        {
            GD.PrintErr("Signals.Instance is null! Make sure Signals.cs is added as an autoload.");
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventory"))
        {
            ToggleWindow(!_inventory.Visible);
        }
    }

    // Signal handler for when player receives an item
    private void OnGivePlayerItem()
    {
        GD.Print("Player received an item! Opening inventory...");

        // You can customize this behavior:
        // Option 1: Just show the inventory
        // ToggleWindow(true);

        // Option 2: Add visual feedback, play sound, etc.
        // PlayItemReceivedSound();
        // ShowItemNotification();
    }

    public void ToggleWindow(bool open)
    {
        GD.Print("blahs blahshda");

        if (_inventory == null)
        {
            GD.PrintErr("Inventory panel is null!");
            return;
        }

        _inventory.Visible = open;
        GD.Print($"Set inventory visibility to: {open}, actual visibility: {_inventory.Visible}");

        if (open)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GD.Print("Mouse mode set to: Visible");
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            GD.Print("Mouse mode set to: Captured");
        }

        GD.Print($"Current mouse mode: {Input.MouseMode}");
    }

    // Clean up signal connections when the node is removed
    public override void _ExitTree()
    {
        if (Signals.Instance != null)
        {
            Signals.Instance.GivePlayerItem -= OnGivePlayerItem;
        }
    }
}
