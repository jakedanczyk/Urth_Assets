using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization16
{
    public static string saveFolderName = "terrain16saves";

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(WorldPos chunk16Location)
    {
        string fileName = chunk16Location.x + "," + chunk16Location.y + "," + chunk16Location.z + ".bin";
        return fileName;
    }

    public static void SaveChunk16(Chunk16 chunk16)
    {
        Save16 save16 = new Save16(chunk16);
        if (save16.blocks.Count == 0)
            return;

        string saveFile = SaveLocation(chunk16.world16.worldName);
        saveFile += FileName(chunk16.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save16);
        stream.Close();

    }

    public static bool Load16(Chunk16 chunk16)
    {
        Debug.Log(chunk16.world16.worldName + "," + chunk16.pos.x + chunk16.pos.y + chunk16.pos.z);

        string saveFile = SaveLocation(chunk16.world16.worldName);
        saveFile += FileName(chunk16.pos);
        Debug.Log("newChunkAlmostSerialized1");

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save16 save16 = (Save16)formatter.Deserialize(stream);

        foreach (var block16 in save16.blocks)
        {
            chunk16.block16s[block16.Key.x, block16.Key.y, block16.Key.z] = block16.Value;
        }
        Debug.Log("newChunkAlmostSerialized2");

        stream.Close();
        return true;
    }
}