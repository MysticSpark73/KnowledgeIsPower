using System;

namespace Data
{
    [Serializable]
    public class PositionOnLevel
    {
        public string levelName;
        public Vector3Data position;

        public PositionOnLevel(string levelName, Vector3Data position)
        {
            this.levelName = levelName;
            this.position = position;
        }

        public PositionOnLevel(string levelName)
        {
            this.levelName = levelName;
        }
    }
}