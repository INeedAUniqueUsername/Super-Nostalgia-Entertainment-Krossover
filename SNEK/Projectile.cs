using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEK {
    class LaserTrail : Entity {
        public Point pos { get; private set; }
        int lifetime;
        float opacity;
        public LaserTrail(Point pos, float opacity = 1) {
            this.pos = pos;
            lifetime = 5;
            this.opacity = opacity;
        }
        public void Update(World g) {
            lifetime--;
            if(lifetime > 0) {
                g.Add(this);
            }
        }
        public void Draw(SpriteBatch g) {
            g.Draw(Sprites.laser, pos.round() * 16, null, Color.White * opacity * (lifetime > 5 ? 1 : lifetime / 5f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //new Rectangle((pos - new Point(8, 8)).point, new Point(16, 16).point);
        }
    }
    class Laser : Moving {
        public bool active;
        public Entity source { get; private set; }
        public Point pos { get; private set; }
        public Point vel { get; private set; }
        int lifetime;
        public Laser(Entity source, Point pos, Point vel) {
            this.source = source;
            this.pos = pos;
            this.vel = vel;
            lifetime = 20;
            active = true;
        }
        public void Collide(World g, Entity other) {
            if (other is Fragment f) {
                //Pass through
            } else if (other is Plasma p) {
                //Pass through
            } else if (other is Laser l) {
                //Pass through
            } else if (other is Player player) {
                player.Collide(g, this);
                active = false;
            } else if (other is Enemy enemy) {
                Console.WriteLine("Enemy hit");
                enemy.Collide(g, this);
                active = false;
            }
        }
        public void Update(World g) {
            pos += vel;
            pos = pos.Constrain(g);
            Point dest = pos + vel;
            Point lastPos = pos;
            //Move stepwise so that we have a continuous plasma trail
            //Subtract one step so that we don't get plasma in front of us when moving slowly
            for (int i = 0; i < vel.magnitude - 1 && active; i++) {
                pos += vel.normal;
                check();
                lastPos = pos;
            }

            pos = dest;
            check();

            void check() {
                pos = pos.Constrain(g);

                if (lastPos.X != pos.X || lastPos.Y != pos.Y) {
                    g.Place(new LaserTrail(lastPos, lifetime / 15f));
                }

                if (g.Collide(pos, out Entity other)) {
                    Console.WriteLine("Laser Collision " + other.GetType().Name);
                    Collide(g, other);
                }
            }
            lifetime--;
            if(active && lifetime > 0) {
                g.Place(this);
            }
        }
        public void Draw(SpriteBatch g) {
            g.Draw(Sprites.laser, pos.round() * 16, null, Color.White * (lifetime > 10 ? 1 : lifetime / 10f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //new Rectangle((pos - new Point(8, 8)).point, new Point(16, 16).point);
        }
    }
    class Fragment : Moving {
        public Point pos { get; private set; }
        public Point vel { get; private set; }
        int lifetime;
        public Fragment(Point pos, Point vel) {
            this.pos = pos;
            this.vel = vel;
            lifetime = 125;
        }
        public void Update(World g) {
            pos += vel;
            pos = pos.Constrain(g);
            bool active = true;
            if (g.Collide(pos, out var other)) {
                if (other is Fragment f) {
                    //Pass through
                } else if (other is Plasma p) {
                    //Pass through
                } else if(other is Laser l) {

                } else if(other is Player player) {
                    player.Collide(g, this);
                    active = false;
                } else if(other is Enemy enemy) {
                    enemy.Collide(g, this);
                    active = false;
                }
            }
            lifetime--;
            if(active && lifetime > 0) {
                g.Place(this);
            }
        }
        public void Draw(SpriteBatch g) {
            g.Draw(Sprites.fragment, pos.round() * 16, null, Color.White * (lifetime > 25 ? 1 : lifetime / 25f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //new Rectangle((pos - new Point(8, 8)).point, new Point(16, 16).point);
        }
    }
}
