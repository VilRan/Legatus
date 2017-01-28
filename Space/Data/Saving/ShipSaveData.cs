using Microsoft.Xna.Framework;
using Space.Battle;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace Space.Data.Saving
{
    public class ShipSaveData
    {
        public List<ModuleSaveData> Modules;
        public Vector2 Position;
        public Vector2 Velocity;

        public ShipSaveData()
        {

        }

        public ShipSaveData(BattleShip ship)
        {
            Modules = new List<ModuleSaveData>();
            foreach (BattleModule module in ship.Modules)
            {
                Modules.Add(new ModuleSaveData(module));
            }
            Position = ship.Position;
            Velocity = ship.Velocity;
        }

        public void Save(string directory, string fileName)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            FileStream fileStream = null;
            GZipStream gzipStream = null;
            try
            {
                fileStream = new FileStream(directory + '/' + fileName, FileMode.Create);
                gzipStream = new GZipStream(fileStream, CompressionMode.Compress);
                XmlSerializer serializer = new XmlSerializer(typeof(ShipSaveData));
                serializer.Serialize(gzipStream, this);

            }
            finally
            {
                if (gzipStream != null)
                    gzipStream.Close();
                else if (fileStream != null)
                    fileStream.Close();
            }
        }

        public static ShipSaveData Load(string directory, string fileName)
        {
            FileStream fileStream = null;
            GZipStream gzipStream = null;
            try
            {
                fileStream = new FileStream(directory + '/' + fileName, FileMode.Open);
                gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
                XmlSerializer serializer = new XmlSerializer(typeof(ShipSaveData));
                return (ShipSaveData)serializer.Deserialize(gzipStream);
            }
            finally
            {
                if (gzipStream != null)
                    gzipStream.Close();
                else if (fileStream != null)
                    fileStream.Close();
            }
        }
    }
}
