/// <summary>
/// Represents a command history for the WindowsBeLike interface.
/// </summary>
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsBeLike
{
    /// <summary>
    /// Represents a command history that stores and retrieves previously executed commands.
    /// </summary>
    public class CommandHistory : List<string>
    {
        private const int MAX_HISTORY_SIZE = 100; // Set the max history size as desired
        private int currentCommandIndex;

        /// <summary>
        /// Initializes a new instance of the CommandHistory class.
        /// </summary>
        public CommandHistory() : base(MAX_HISTORY_SIZE)
        {
            currentCommandIndex = -1;
        }

        /// <summary>
        /// Adds a command to the command history.
        /// </summary>
        /// <param name="command">The command to add.</param>
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
        /// Gets the previous command in the history.
        /// </summary>
        /// <returns>The previous command.</returns>
        public string GetPrevious()
        {
            if (this.Count == 0) return null;

            currentCommandIndex--;
            currentCommandIndex = currentCommandIndex < 0 ? 0 : currentCommandIndex;

            return GetCurrent();
        }

        /// <summary>
        /// Gets the next command in the history.
        /// </summary>
        /// <returns>The next command.</returns>
        public string GetNext()
        {
            if (this.Count == 0) return null;

            currentCommandIndex++;
            currentCommandIndex = currentCommandIndex >= this.Count ? this.Count - 1 : currentCommandIndex;

            return GetCurrent();
        }

        /// <summary>
        /// Gets the command at the current index in the history.
        /// </summary>
        /// <returns>The command at the current index.</returns>
        public string GetCurrent()
        {
            if (currentCommandIndex < 0 || currentCommandIndex >= this.Count)
            {
                return null;
            }

            return this[currentCommandIndex];
        }
    }
}
