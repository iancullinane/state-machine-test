using Godot;

public partial class Signals : Node
{
    public static Signals Instance { get; private set; }

    [Signal]
    public delegate void GivePlayerItemEventHandler(Item item);

    public override void _Ready()
    {
        Instance = this;
    }

    // Convenience method for emitting the signal
    public void EmitGivePlayerItem(Item item)
    {
        EmitSignal(SignalName.GivePlayerItem, item);
    }
}