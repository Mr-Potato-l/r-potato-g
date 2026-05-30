using Godot;
using System;

public partial class InventoryGui : Control
{
    public bool IsOpen { get; set; } = false;

    public void Open()
    {
        Visible = true;
        IsOpen = true;
    }

    public void Close()
    {
        Visible = false;
        IsOpen = false;
    }
}
