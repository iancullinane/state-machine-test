using Godot;

public partial class WorldItem : Interactable
{
    [Export] public string ItemName { get; set; }

    public override void _Interact()
    {
        Item item = GD.Load<Item>("res://Static/Items/" + ItemName + ".tres");
        // Here the `Instance` is the singleton instance of the `Signals` class
        // to make it accessible from any other class  
        Signals.Instance.EmitGivePlayerItem(item);
        QueueFree();
    }
}
