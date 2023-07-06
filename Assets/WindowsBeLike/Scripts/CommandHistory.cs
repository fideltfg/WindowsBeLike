using System;
using System.Collections.Generic;
using System.Diagnostics;

public class CommandHistory : List<string>
{
    private const int MAX_HISTORY_SIZE = 100; // Set the max history size as desired
    private int currentCommandIndex;

    public CommandHistory() : base(MAX_HISTORY_SIZE)
    {
        currentCommandIndex = -1;
    }

    public void AddCommand(string command)
    {
        // If command is identical to the last one, reject it
        if (this.Count > 0 && this[this.Count - 1] == command)
            return;

        // If the history is full, remove the oldest command
        if (this.Count >= MAX_HISTORY_SIZE)
        {
            this.RemoveAt(0);
        }

        this.Add(command);
        // Reset the index to the end of the history
        currentCommandIndex = this.Count;
         
    }

    /// <summary>
    /// method to return the previous command in the history
    /// </summary>
    /// <returns></returns>
    public string GetPrevious()
    {
        if (this.Count == 0) return null;

        currentCommandIndex--;
        currentCommandIndex = currentCommandIndex < 0 ? 0 : currentCommandIndex;

        return GetCurrent();
    }

    /// <summary>
    /// method to return the next command in the history
    /// </summary>
    /// <returns></returns>
    public string GetNext()
    {
        if (this.Count == 0) return null;

        currentCommandIndex++;
        currentCommandIndex = currentCommandIndex >= this.Count ? this.Count - 1 : currentCommandIndex;
        
        return GetCurrent();

        //return this[currentCommandIndex];
    }

    /// <summary>
    /// method to retun the command at the current index
    /// </summary>
    /// <returns></returns>
    public string GetCurrent()
    {
        if (currentCommandIndex < 0 || currentCommandIndex >= this.Count)
        {
            return null;
        }

        return this[currentCommandIndex];
    }
}
