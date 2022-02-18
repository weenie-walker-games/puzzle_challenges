using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    [System.Serializable]
    public enum DirectionEnum
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    [System.Serializable]
    public struct CorrectLines
    {
        public List<CircleObject> objectsInLine;
    }

    [System.Serializable]
    public struct ConnectedObjects
    {
        public DirectionObject fromObject;
        public DirectionObject toObject;
    }

    [System.Serializable]
    public struct DirectionObject
    {
        public CircleObject circleObject;
        public DirectionEnum directionEnum;
    }
}
