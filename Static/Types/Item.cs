using Godot;

[GlobalClass]
public partial class Item : Resource
{
    [Export]
    public string Name { get; set; } = "";
    [Export]
    public Texture2D Icon { get; set; } = null;
    [Export]
    public int MaxStackSize { get; set; } = 12;

    //World Item Scene?

    [Export]
    public bool CanDrop { get; set; } = true;

    public Item()
    {
        Name = "00xx00";
    }

    public Item(string name)
    {
        Name = name;
    }
}
