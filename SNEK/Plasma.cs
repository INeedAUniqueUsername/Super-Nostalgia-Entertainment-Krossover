using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SNEK {
    class Plasma : Entity {
        public Entity source;
        int lifetime;
        public Point pos { get; private set; }
        public Plasma(Entity source, Point pos, int lifetime) {
            this.source = source;
            this.pos = pos;
            this.lifetime = lifetime;
        }
        public void Update(World g) {
            lifetime--;
            if(lifetime > 0) {
                g.Place(this);
            }
        }
        public void Draw(SpriteBatch g) {
            g.Draw(Sprites.plasma, pos.round() * 16, null, Color.White * ((float) lifetime / Math.Max(lifetime, 8)), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //new Rectangle((pos - new Point(8, 8)).point, new Point(16, 16).point);
        }
    }
}
