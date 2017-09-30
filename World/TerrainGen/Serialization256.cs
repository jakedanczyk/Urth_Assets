using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization256
{
    public static string saveFolderName = "terrain256saves";

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(WorldPos chunk256Location)
    {
        string fileName = chunk256Location.x + "," + chunk256Location.y + "," + chunk256Location.z + ".bin";
        return fileName;
    }

    public static void SaveChunk256(Chunk256 chunk256)
    {
        Save256 save256 = new Save256(chunk256);
        if (save256.blocks.Count == 0)
            return;

        string saveFile = SaveLocation(chunk256.world256.worldName);
        saveFile += FileName(chunk256.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save256);
        stream.Close();

    }

    public static bool Load256(Chunk256 chunk256)
    {
        string saveFile = SaveLocation(chunk256.world256.worldName);
        saveFile += FileName(chunk256.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save256 save256 = (Save256)formatter.Deserialize(stream);

        foreach (var block256 in save256.blocks)
        {
            chunk256.block256s[block256.Key.x, block256.Key.y, block256.Key.z] = block256.Value;
        }

        stream.Close();
        return true;
    }
}