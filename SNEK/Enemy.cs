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
        Player p;
        public Enemy(Player p, Point pos) {
            this.p = p;
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
            Point target = pos.Closest(p.pos, p.pos + g.N, p.pos + g.NE, p.pos + g.E, p.pos + g.SE, p.pos + g.S, p.pos + g.SW, p.pos + g.W, p.pos + g.NW);
            Point direction = (target - pos).normal;
            //If we're turning around, then we decelerate faster
            if(direction.dot(vel) < 0) {
                vel += direction * (1 / 30f);
            } else {
                vel += direction * (1 / 60f);
            }
            if(vel.magnitude > 2) {
                vel = vel.normal * 2;
            }

            Point lastPos = pos;
            pos += vel;
            pos = pos.Constrain(g);
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
            } else {
                for (double a = 0; a < 2 * Math.PI; a += Math.PI / 3) {
                    g.Place(new Fragment(pos, new Point(vel.angle + a + g.r.NextDouble() * Math.PI/6) * 2));
                }
            }
        }

        public void Draw(SpriteBatch g) {
            g.Draw(Sprites.enemy, pos.round() * 16, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //new Rectangle((pos - new Point(8, 8)).point, new Point(16, 16).point);
        }
    }
}
