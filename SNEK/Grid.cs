using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEK {
    class World {
        public Random r = new Random();

        public int width, height;
        public HashSet<Entity> entities;
        public Entity[,] grid;
        public Entity this[Point p] {
            get => grid[p.X, p.Y];
            set => grid[p.X, p.Y] = value;
        }

        public World(int width, int height) {
            this.width = width;
            this.height = height;
            grid = new Entity[width, height];
            entities = new HashSet<Entity>();
        }
        public bool Collide(Point p, out Entity e) {
            e = this[p];
            return e != null;
        }
        public void Place(Entity e) {
            this[e.pos] = e;
            entities.Add(e);
        }
        public void Add(Entity e) {
            entities.Add(e);
        }
        public void Remove(Entity e) {
            entities.Remove(e);
        }
        public void Update() {
            grid = new Entity[width, height];
            HashSet<Entity> entities2 = entities;
            entities = new HashSet<Entity>();
            foreach(Entity e in entities2) {
                e.Update(this);
            }
        }
        public void Draw(SpriteBatch g) {
            foreach(Entity e in entities) {
                e.Draw(g);
            }
        }
    }
}
