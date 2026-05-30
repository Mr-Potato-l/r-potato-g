using Godot;
using System;

public partial class UserInterface : Control
{
    [Export]
    public InventoryGui inventory;

    public override void _Ready()
    {
        inventory = GetNode<InventoryGui>("InventoryGui");
        inventory.Close();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("inventory"))
        {
            if (inventory.IsOpen)
            {
                inventory.Close();
            }
            else
            {
                inventory.Open();
            }
        }
    }
}
