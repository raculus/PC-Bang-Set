using System;
using System.IO;


class Find
{
    static void Drives()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();

        foreach (DriveInfo drive in drives)
        {
            Console.WriteLine($"Drive: {drive.Name}");
            Console.WriteLine($"  Drive type: {drive.DriveType}");

            if (drive.IsReady)
            {
                Console.WriteLine($"  Volume label: {drive.VolumeLabel}");
                Console.WriteLine($"  File system: {drive.DriveFormat}");
                Console.WriteLine($"  Total space: {drive.TotalSize} bytes");
                Console.WriteLine($"  Available space: {drive.AvailableFreeSpace} bytes");
                Console.WriteLine($"  Used space: {drive.TotalSize - drive.TotalFreeSpace} bytes");
            }
            else
            {
                Console.WriteLine("  Drive is not ready.");
            }

            Console.WriteLine();
        }
    }

    static string Directory(string rootDirectory, string folderName)
    {
        try
        {
            // Search for the folder with the specified name recursively in all subdirectories
            string[] folders = System.IO.Directory.GetDirectories(rootDirectory, folderName, SearchOption.AllDirectories);

            if (folders.Length > 0)
            {
                // Return the first matching folder path
                return folders[0];
            }
            else
            {
                return null; // Folder not found
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    static string File(string directoryPath, string fileName)
    {
        try
        {
            string[] files = System.IO.Directory.GetFiles(directoryPath, fileName, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                // Return the first matching file path
                return files[0];
            }
            else
            {
                return null; // File not found
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
 }
