using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SNEK {
    class Enemy : Moving {
        public int hp;
        public Point pos { get; private set; }
        public Point vel { get; private set; }
        public Enemy(Point pos) {
            hp = 100;
            this.pos = pos;
            vel = new Point(0, 0);
        }
        public void Collide(World g, Entity other) {
            if (other is Enemy e) {
                hp = 0;
                e.hp = 0;
                Console.WriteLine("Enemy-Enemy");
            } else if (other is Plasma p) {
                hp -= 10;
                Console.WriteLine("Enemy-Plasma");
            } else if (other is Fragment f) {
                hp -= 10;
                Console.WriteLine("Enemy-Fragment");
            } else if (other is Laser l) {
                hp = 0;
                l.active = false;
                Console.WriteLine("Enemy-Laser");
            }
        }
        public void Update(World g) {
            Point lastPos = pos;
            pos += vel;
            if (lastPos.X != pos.X || lastPos.Y != pos.Y) {
                g.Place(new Plasma(this, lastPos, 10));
            }
            if (g.Collide(pos, out Entity other)) {
                if(other is Player p) {
                    p.Collide(g, this);
                } else {
                    Collide(g, other);
                }
            }
            if (hp > 0) {
                g.Place(this);
            }
        }

        public void Draw(SpriteBatch g) {
            g.Draw(Sprites.enemy, pos.round() * 16, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //new Rectangle((pos - new Point(8, 8)).point, new Point(16, 16).point);
        }
    }
}
