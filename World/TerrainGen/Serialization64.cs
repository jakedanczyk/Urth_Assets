using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization64
{
    public static string saveFolderName = "terrain64saves";

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(WorldPos chunk64Location)
    {
        string fileName = chunk64Location.x + "," + chunk64Location.y + "," + chunk64Location.z + ".bin";
        return fileName;
    }

    public static void SaveChunk64(Chunk64 chunk64)
    {
        Save64 save64 = new Save64(chunk64);
        if (save64.blocks.Count == 0)
            return;

        string saveFile = SaveLocation(chunk64.world64.worldName);
        saveFile += FileName(chunk64.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save64);
        stream.Close();

    }

    public static bool Load64(Chunk64 chunk64)
    {
        string saveFile = SaveLocation(chunk64.world64.worldName);
        saveFile += FileName(chunk64.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save64 save64 = (Save64)formatter.Deserialize(stream);

        foreach (var block64 in save64.blocks)
        {
            chunk64.block64s[block64.Key.x, block64.Key.y, block64.Key.z] = block64.Value;
        }

        stream.Close();
        return true;
    }
}