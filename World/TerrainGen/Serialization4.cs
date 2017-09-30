using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization4
{
    public static string saveFolderName = "voxelGameSaves";

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(WorldPos chunkLocation)
    {
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }

    public static void SaveChunk4(Chunk4 chunk4)
    {
        Save4 save4 = new Save4(chunk4);
        if (save4.block4s.Count == 0)
            return;

        string saveFile = SaveLocation(chunk4.world4.worldName);
        saveFile += FileName(chunk4.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save4);
        stream.Close();

    }

    public static bool Load(Chunk4 chunk4)
    {
        string saveFile = SaveLocation(chunk4.world4.worldName);
        saveFile += FileName(chunk4.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save4 save4 = (Save4)formatter.Deserialize(stream);

        foreach (var block4 in save4.block4s)
        {
            chunk4.block4s[block4.Key.x, block4.Key.y, block4.Key.z] = block4.Value;
        }

        stream.Close();
        return true;
    }
}