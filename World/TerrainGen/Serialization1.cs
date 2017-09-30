using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization1
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

    public static void SaveChunk1(Chunk1 chunk1)
    {
        Save1 save1 = new Save1(chunk1);
        if (save1.block1s.Count == 0)
            return;

        string saveFile = SaveLocation(chunk1.world1.worldName);
        saveFile += FileName(chunk1.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save1);
        stream.Close();

    }

    public static bool Load(Chunk1 chunk1)
    {
        string saveFile = SaveLocation(chunk1.world1.worldName);
        saveFile += FileName(chunk1.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save1 save1 = (Save1)formatter.Deserialize(stream);

        foreach (var block1 in save1.block1s)
        {
            chunk1.block1s[block1.Key.x, block1.Key.y, block1.Key.z] = block1.Value;
        }

        stream.Close();
        return true;
    }
}