using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class XMLFileSystem
{
    XElement rootDirectory;
    XElement currentDirectory;

    public XMLFileSystem(XElement rootDirectory)
    {
        this.rootDirectory = rootDirectory;
    }


    /// <summary>
    /// Navigates to the specified directory in the file structure represented by the XML file.
    /// </summary>
    /// <param name="path">The path of the directory to navigate to</param>
    /// <returns>A string indicating the result of the operation, "Success" or an error message</returns>
    public string ChangeDirectory(string directoryName)
    {
        XElement newDirectory = currentDirectory.Element("Directory")
            .Elements("Directory")
            .Where(d => d.Attribute("name").Value == directoryName)
            .FirstOrDefault();

        if (newDirectory == null)
        {
            return $"Directory not found: {directoryName}";
        }

        currentDirectory = newDirectory;
        return currentDirectory.Attribute("name").Value;
    }

    /// <summary>
    /// Lists the contents of the current directory in the file structure represented by the XML file.
    /// </summary>
    /// <returns>A string representation of the contents of the current directory</returns>
    public string ListContents()
    {
        IEnumerable<XElement> directories = currentDirectory.Elements("Directory");
        IEnumerable<XElement> files = currentDirectory.Elements("File");
        string result = "";
        result += "Directories: \n";
        foreach (XElement directory in directories)
        {
            result += $"  {directory.Attribute("name").Value}\n";
        }

        result += "Files:\n";
        foreach (XElement file in files)
        {
            result += $"  {file.Attribute("name").Value}\n";
        }
        return result;
    }


    /// <summary>
    /// Renames a file or directory in the file structure represented by the XML file.
    /// </summary>
    /// <param name="currentName">The current name or full path of the file or directory to be renamed</param>
    /// <param name="newName">The new name for the file or directory</param>
    /// <returns>A string indicating the result of the operation, "Success" or an error message</returns>
    public string Rename(string currentName, string newName)
    {
        XElement item;
        if (currentName.Contains("/"))
        {
            string[] pathParts = currentName.Split('/');
            XElement current = rootDirectory;
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                current = current.Elements("Directory")
                    .Where(d => d.Attribute("name").Value == pathParts[i])
                    .FirstOrDefault();

                if (current == null)
                {
                    return "No such directory found in the specified path";
                }
            }

            item = current.Elements("File")
                .Where(f => f.Attribute("name").Value == pathParts[pathParts.Length - 1])
                .FirstOrDefault();
            if (item == null)
            {
                item = current.Elements("Directory")
                    .Where(d => d.Attribute("name").Value == pathParts[pathParts.Length - 1])
                    .FirstOrDefault();
            }
        }
        else
        {
            item = currentDirectory.Elements("File")
                .Where(f => f.Attribute("name").Value == currentName)
                .FirstOrDefault();
            if (item == null)
            {
                item = currentDirectory.Elements("Directory")
                    .Where(d => d.Attribute("name").Value == currentName)
                    .FirstOrDefault();
            }
        }

        if (item == null)
        {
            return "No such file or directory found in the current directory";
        }

        if (currentDirectory.Elements("File").Any(f => f.Attribute("name").Value == newName) ||
            currentDirectory.Elements("Directory").Any(d => d.Attribute("name").Value == newName))
        {
            return "A file or directory with the same name already exists in the current directory";
        }

        item.Attribute("name").Value = newName;
        return $"File {currentName} renamed to {newName}";
    }

    /// <summary>
    /// Returns the full path of the current directory in the file structure represented by the XML file.
    /// </summary>
    /// <returns>A string representation of the full path of the current directory</returns>
    public string PrintWorkingDirectory()
    {
        string path = currentDirectory.Attribute("name").Value;
        XElement parentDirectory = currentDirectory.Parent;

        while (parentDirectory != null)
        {
            path = parentDirectory.Attribute("name").Value + "/" + path;
            parentDirectory = parentDirectory.Parent;
        }

        return "/" + path;
    }

    /// <summary>
    /// Creates a new directory in the current directory of the file structure represented by the XML file.
    /// </summary>
    /// <param name="path">The path or name of the directory to be created</param>
    /// <returns>A string indicating the result of the operation, "Success" or an error message</returns>
    public string MakeDirectory(string path)
    {
        XElement parent;
        if (path.Contains("/"))
        {
            string[] pathParts = path.Split('/');
            XElement current = rootDirectory;
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                current = current.Elements("Directory")
                    .Where(d => d.Attribute("name").Value == pathParts[i])
                    .FirstOrDefault();

                if (current == null)
                {
                   return "No such directory found in the specified path";
                }
            }

            parent = current;
            path = pathParts[pathParts.Length - 1];
        }
        else
        {
            parent = currentDirectory;
        }

        if (parent.Elements("File").Any(f => f.Attribute("name").Value == path) ||
            parent.Elements("Directory").Any(d => d.Attribute("name").Value == path))
        {
            return "A file or directory with the same name already exists in the specified directory";
        }

        XElement directory = new XElement("Directory");
        directory.SetAttributeValue("name", path);
        directory.SetAttributeValue("permissions", "rwxrwxrwx");
        directory.SetAttributeValue("owner", "root");
        directory.SetAttributeValue("group", "root");
        parent.Add(directory);

        return $"Directory Created: {directory.Attribute("name").Value}";
    }


    /// <summary>
    /// Creates a new file in the current directory of the file structure represented by the XML file.
    /// </summary>
    /// <param name="path">The path or name of the file to be created</param>
    /// <returns>A string indicating the result of the operation, "Success" or an error message</returns>
    public string MakeFile(string path)
    {
        XElement parent;
        if (path.Contains("/"))
        {
            string[] pathParts = path.Split('/');
            XElement current = rootDirectory;
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                current = current.Elements("Directory")
                    .Where(d => d.Attribute("name").Value == pathParts[i])
                    .FirstOrDefault();

                if (current == null)
                {
                    return "No such directory found in the specified path";
                }
            }

            parent = current;
            path = pathParts[pathParts.Length - 1];
        }
        else
        {
            parent = currentDirectory;
        }

        if (parent.Elements("File").Any(f => f.Attribute("name").Value == path) ||
            parent.Elements("Directory").Any(d => d.Attribute("name").Value == path))
        {
            return "A file or directory with the same name already exists in the specified directory";
        }

        XElement file = new XElement("File");
        file.SetAttributeValue("name", path);
        file.SetAttributeValue("mime-type", "application/unknown");
        file.SetAttributeValue("size", 0);
        file.SetAttributeValue("permissions", "rw-rw-rw-");
        file.SetAttributeValue("owner", "root");
        file.SetAttributeValue("group", "root");
        parent.Add(file);

        return $"File Created: {file.Attribute("name").Value}";
    }

    /// <summary>
    /// Deletes a file or directory from the file structure represented by the XML file.
    /// </summary>
    /// <param name="path">The path or name of the file or directory to be deleted</param>
    /// <returns>A string indicating the result of the operation, "Success" or an error message</returns>
    public string Delete(string path)
    {
        XElement element;
        if (path.Contains("/"))
        {
            string[] pathParts = path.Split('/');
            XElement current = rootDirectory;
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                current = current.Elements("Directory")
                    .Where(d => d.Attribute("name").Value == pathParts[i])
                    .FirstOrDefault();

                if (current == null)
                {
                    return "No such directory found in the specified path";
                }
            }

            string name = pathParts[pathParts.Length - 1];
            element = current.Elements("Directory")
                .Where(d => d.Attribute("name").Value == name)
                .FirstOrDefault();
            if (element == null)
            {
                element = current.Elements("File")
                    .Where(f => f.Attribute("name").Value == name)
                    .FirstOrDefault();
            }
        }
        else
        {
            element = currentDirectory.Elements("Directory")
                .Where(d => d.Attribute("name").Value == path)
                .FirstOrDefault();
            if (element == null)
            {
                element = currentDirectory.Elements("File")
                    .Where(f => f.Attribute("name").Value == path)
                    .FirstOrDefault();
            }
        }

        if (element == null)
        {
            return "No such file or directory found in the current directory or specified path";
        }

        element.Remove();
        return "";
    }


}
