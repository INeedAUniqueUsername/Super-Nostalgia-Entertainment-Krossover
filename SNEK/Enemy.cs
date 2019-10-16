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
        public int exhaustTime;
        public int fireTime;
        Moving target;
        public Enemy(Moving p, Point pos) {
            this.target = p;
            hp = 100;
            this.pos = pos;
            vel = new Point(0, 0);
            this.exhaustTime = 10;
            fireTime = 60;
        }
        public void Collide(World g, Entity other) {
            if (other is Enemy e) {
                hp = 0;
                e.hp = 0;
                Console.WriteLine("Enemy-Enemy");
            } else if (other is Plasma p) {
                //Uncomment this to make enemies self-destruct automatically
                if(p.source != this) {
                    hp -= 10;
                }
                Console.WriteLine("Enemy-Plasma");
            } else if (other is Fragment f) {
                hp -= 10;
                vel += f.vel * 0.5;
                Console.WriteLine("Enemy-Fragment");
            } else if (other is Laser l) {
                if(l.source != this) {
                    hp = 0;
                    l.active = false;
                    Console.WriteLine("Enemy-Laser");
                }
                
            }
        }
        public void Update(World g) {
            //Make sure we have a target, even if it's another enemy
            if(!g.entities.Contains(target)) {
                var targets = g.entities.Where(t => t is Moving && t != this).Select(t => (Moving) t);
                if(targets.Count() > 0) {
                    target = targets.ElementAt(g.r.Next(targets.Count()));
                } else {
                    target = null;
                }
            }
            //Don't do anything if we don't have a target
            if(target != null) {
                Point aimAt = pos.Closest(target.pos, target.pos + g.N, target.pos + g.NE, target.pos + g.E, target.pos + g.SE, target.pos + g.S, target.pos + g.SW, target.pos + g.W, target.pos + g.NW);
                Point direction = (aimAt - pos).normal;
                //If we're turning around, then we decelerate faster
                if (direction.dot(vel) < 0) {
                    vel += direction * (1 / 30f);
                } else {
                    vel += direction * (1 / 60f);
                }
            }
            if(vel.magnitude > 2) {
                vel = vel.normal * 2;
            }

            Point lastPos = pos;
            pos += vel;
            pos = pos.Constrain(g);
            if (lastPos.X != pos.X || lastPos.Y != pos.Y) {
                g.Place(new Plasma(this, lastPos, exhaustTime));
            }

            if(fireTime > 0) {
                fireTime--;
            } else if(target != null) {
                double laserSpeed = 1.5;
                double distance = (pos - target.pos).magnitude;
                if (distance < 64) {
                    fireTime = g.r.Next(30, 60);
                    double timeToHit = distance / laserSpeed;
                    Point hitPos = target.pos + target.vel * timeToHit;
                    g.Place(new Laser(this, pos, (hitPos - pos).normal * laserSpeed));
                }
                
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
