using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class SimpleSerialization
{
    public static string simpleSaveFolderName = "simpleTerrainSaves";

    public static string SimpleSaveLocation(string simpleWorldName)
    {
        string simpleSaveLocation = simpleSaveFolderName + "/" + simpleWorldName + "/";

        if (!Directory.Exists(simpleSaveLocation))
        {
            Directory.CreateDirectory(simpleSaveLocation);
        }

        return simpleSaveLocation;
    }

    public static string FileName(WorldPos simpleChunkLocation)
    {
        string fileName = simpleChunkLocation.x + "," + simpleChunkLocation.y + "," + simpleChunkLocation.z + ".bin";
        return fileName;
    }

    public static void SaveSimpleChunk(SimpleChunk simpleChunk)
    {
        SimpleSave simpleSave = new SimpleSave(simpleChunk);
        if (simpleChunk.isEmpty)
            return;

        string simpleSaveFile = SimpleSaveLocation(simpleChunk.simpleWorld.worldName);
        simpleSaveFile += FileName(simpleChunk.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(simpleSaveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, simpleSave);
        stream.Close();

    }

    public static bool Load(SimpleChunk simpleChunk)
    {
        string simpleSaveFile = SimpleSaveLocation(simpleChunk.simpleWorld.worldName);
        simpleSaveFile += FileName(simpleChunk.pos);

        if (!File.Exists(simpleSaveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(simpleSaveFile, FileMode.Open);

        Save simpleSave = (Save)formatter.Deserialize(stream);

        stream.Close();
        return true;
    }
}