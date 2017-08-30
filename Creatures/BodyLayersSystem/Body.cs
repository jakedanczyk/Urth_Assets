using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generic Body base for all bodies. Holds a collections of BodyParts

public class Body : MonoBehaviour
{

    private Dictionary<BodyPartTag, BodyPart> _bodyDict;

    /// <summary>
    /// Dictionary containing all stats held within the collection
    /// </summary>
    public Dictionary<BodyPartTag, BodyPart> BodyDict
    {
        get
        {
            if (_bodyDict == null)
            {
                _bodyDict = new Dictionary<BodyPartTag, BodyPart>();
            }
            return _bodyDict;
        }
    }

    private void Awake()
    {
        ConfigureBodyParts();
    }

    protected virtual void ConfigureBodyParts() { }


    /// <summary>
    /// Checks if there is a BodyPart with the given type and id
    /// </summary>
    public bool ContainBodyPart(BodyPartTag bodyPartType)
    {
        return BodyDict.ContainsKey(bodyPartType);
    }

    /// <summary>
	/// Gets the BodyPart with the given BodyPartTag and ID
	/// </summary>
    public BodyPart GetBodyPart(BodyPartTag bodyPartType)
    {
        if (ContainBodyPart(bodyPartType))
        {
            return BodyDict[bodyPartType];
        }
        return null;
    }

    /// <summary>
	/// Gets the BodyPart with the given BodyPartTag and ID as type T
	/// </summary>
    public T GetBodyPart<T>(BodyPartTag type) where T : BodyPart
    {
        return GetBodyPart(type) as T;
    }

    /// <summary>
    /// Creates a new instance of the BodyPart ands adds it to the BodyDict
    /// </summary>
    protected T CreateBodyPart<T>(BodyPartTag bodyPartType) where T : BodyPart
    {
        T bodyPart = System.Activator.CreateInstance<T>();
        BodyDict.Add(bodyPartType, bodyPart);
        return bodyPart;
    }

    /// <summary>
    /// Creates or Gets a BodyPart of type T. Used within the setup method during initialization.
    /// </summary>
    protected T CreateOrGetBodyPart<T>(BodyPartTag bodyPartType) where T : BodyPart
    {
        T bodyPart = GetBodyPart<T>(bodyPartType);
        if (bodyPart == null)
        {
            bodyPart = CreateBodyPart<T>(bodyPartType);
        }
        return bodyPart;
    }

}