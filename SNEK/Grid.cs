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
        private HashSet<Entity> _entities;
        public Entity[,] grid;

        public Point N => new Point(0, -height);
        public Point NE => new Point(width, -height);
        public Point NW => new Point(-width, -height);
        public Point E => new Point(width, 0);
        public Point W => new Point(-width, 0);
        public Point S => new Point(0, height);
        public Point SE => new Point(width, -height);
        public Point SW => new Point(-width, -height);

        public Entity this[Point p] {
            get => grid[p.X, p.Y];
            set => grid[p.X, p.Y] = value;
        }

        public World(int width, int height) {
            this.width = width;
            this.height = height;
            grid = new Entity[width, height];
            _entities = new HashSet<Entity>();
            entities = _entities;
        }
        public bool Collide(Point p, out Entity e) {
            e = this[p];
            return e != null;
        }
        public void Place(Entity e) {
            this[e.pos] = e;
            _entities.Add(e);
        }
        public void Add(Entity e) {
            _entities.Add(e);
        }
        public void Remove(Entity e) {
            _entities.Remove(e);
        }
        public void Update() {
            grid = new Entity[width, height];
            _entities = new HashSet<Entity>();
            foreach(Entity e in entities) {
                e.Update(this);
            }
            entities = _entities;
        }
        public void Draw(SpriteBatch g) {
            foreach(Entity e in _entities) {
                e.Draw(g);
            }
        }
    }
}
