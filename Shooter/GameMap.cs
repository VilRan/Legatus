using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    public class GameMap
    {
        public Dictionary<long, GameObject> Objects = new Dictionary<long, GameObject>();

        private List<long> Removing = new List<long>();
        private int IDCounter = 0;

        public GameMap()
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject obj in Objects.Values)
            {
                obj.Update(gameTime);
            }
            foreach (long id in Removing)
            {
                Objects.Remove(id);
            }
            Removing.Clear();
        }

        public int AddObject(GameObject obj)
        {
            int id = IDCounter++;
            obj.ID = id;
            obj.Map = this;
            Objects.Add(id, obj);
            return id;
        }
        
        public void AddObject(GameObject obj, int id)
        {
            obj.ID = id;
            obj.Map = this;
            Objects.Add(id, obj);
        }

        public void RemoveObject(GameObject obj)
        {
            Removing.Add(obj.ID);
        }
    }
}
