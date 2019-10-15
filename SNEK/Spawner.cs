using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SNEK {
    class Spawner : Entity {
        int wait = 30;
        int count = 1;
        public Point pos => null;

        public void Draw(SpriteBatch g) {
        }

        public void Update(World w) {
            w.Add(this);
            wait--;
            if(wait > 0) {
                return;
            }
            wait = 30;
            Console.WriteLine("Spawner");
            if (w.entities.Count(e => e is Enemy) == 0) {
                Console.WriteLine("Wave");
                count++;
                for(int i = 0; i < count; i++) {
                    w.Place(new Enemy(new Point(w.r.NextDouble() * w.width, w.r.NextDouble() * w.height).Constrain(w)));
                }
            } 
        }
    }
}
