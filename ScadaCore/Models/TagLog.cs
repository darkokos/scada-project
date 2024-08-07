﻿using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public abstract class TagLog {
    [Key] public int Id { get; set; }
    [Required] public string? TagName { get; set; }
    [Required] public DateTime Timestamp { get; set; }
    
    protected TagLog() { }

    protected TagLog(string tagName, DateTime timestamp) {
        TagName = tagName;
        Timestamp = timestamp;
    }
}