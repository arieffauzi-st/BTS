using System;

namespace BTS.Models;

public class ChecklistItemModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
}
