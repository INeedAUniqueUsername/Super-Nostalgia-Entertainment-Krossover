using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point2D = Microsoft.Xna.Framework.Point;
namespace SNEK {
    class Player : Moving {
        bool fireCooldown = false;
        public int fireTimer;
        public static Texture2D sprite;
        public Point pos { get; private set; }
        public Point vel { get; private set; }
        public int hp;
        public Player(Point pos) {
            hp = 100;
            this.pos = pos;
            vel = new Point(0, 0);
        }
        public void Collide(World g, Entity other) {
            if (other is Enemy e) {
                hp = 0;
                e.hp = 0;

                Console.WriteLine("Enemy damage");
            } else if(other is Fragment f) {
                hp -= 10;
                vel += f.vel * 0.5;
            } else if (other is Plasma p && p.source != this) {
                Console.WriteLine("Plasma damage");
                hp -= 10;
            } else if(other is Laser l && l.source != this) {
                Console.WriteLine("Laser damage");
                hp -= 5;
            }
        }
        public void Update(World g) {
            var state = Keyboard.GetState();
            var left = state.IsKeyDown(Keys.Left);
            var right = state.IsKeyDown(Keys.Right);
            var up = state.IsKeyDown(Keys.Up);
            var down = state.IsKeyDown(Keys.Down);
            var space = state.IsKeyDown(Keys.Space);
            /*
            if (!(left && right)) {
                if(left) {
                    vel.x -= 1/30f;
                } else if(right) {
                    vel.x += 1/30f;
                }
            }
            if(!(up && down)) {
                if(up) {
                    vel.y -= 1/30f;
                } else if(down) {
                    vel.y += 1/30f;
                }
            }
            */
            Point direction = Point.ZERO;
            if(!(up && down)) {
                if(up) {
                    direction += new Point(0, -1);
                } else if(down) {
                    direction += new Point(0, 1);
                }
            }
            if(!(left && right)) {
                if (left) {
                    direction += new Point(-1, 0);
                } else if (right) {
                    direction += new Point(1, 0);
                }
            }

            Point center = new Point(GraphicsDeviceManager.DefaultBackBufferWidth / 2, GraphicsDeviceManager.DefaultBackBufferHeight / 2);
            Point offset = new Point(Mouse.GetState().Position.ToVector2()) - center;
            if(offset.magnitude > 0) {
                direction = offset;
                if(direction.magnitude > 1/30f) {
                    direction = direction.normal * (1 / 30f);
                }
            }
            //Point place = offset.magnitude > 1 ? center + offset.normal * (offset.magnitude - 1) : center;
            Point place = center;
            Mouse.SetPosition(place.X, place.Y);

            if (direction != Point.ZERO) {
                vel += direction.normal * (1 / 30f);
            }
            if(vel.magnitude > 3) {
                vel = vel.normal * 3;
            }

            Point dest = pos + vel;
            Point lastPos = pos;
            //Move stepwise so that we have a continuous plasma trail
            //Subtract one step so that we don't get plasma in front of us when moving slowly
            for (int i = 0; i < vel.magnitude - 1; i++) {
                pos += vel.normal;
                check();
                lastPos = pos;
            }
            pos = dest;
            check();
            if (g.Collide(pos, out Entity other)) {
                Console.WriteLine("Collision");
                Collide(g, other);
            }

            void check() {
                pos = pos.Constrain(g);
                if (lastPos.X != pos.X || lastPos.Y != pos.Y) {
                    g.Place(new Plasma(this, lastPos, 15));
                }
            }
            if(fireTimer < 100) {
                fireTimer++;
            } else {
                fireCooldown = false;
            }
            
            if(space && vel.magnitude > 0.05 && !fireCooldown) {
                fireTimer -= 5;
                if(fireTimer < 1) {
                    fireTimer = 0;
                    fireCooldown = true;
                }
                g.Add(new Laser(this, (pos + vel.normal).Constrain(g), vel.normal * 2));
                //g.Add(new Laser(this, (pos + vel.normal.rotate(-30 * Math.PI / 180)).Constrain(g), vel.normal * 2));
                //g.Add(new Laser(this, (pos + vel.normal.rotate(-90 * Math.PI / 180)).Constrain(g), vel.normal * 2));
                //g.Add(new Laser(this, (pos + vel.normal.rotate(30 * Math.PI / 180)).Constrain(g), vel.normal * 2));
                //g.Add(new Laser(this, (pos + vel.normal.rotate(90 * Math.PI/180)).Constrain(g), vel.normal * 2));
            }
            /*
            Point lastPos = pos;
            pos += vel;
            if(pos.X >= g.width) {
                pos.x -= g.width;
            }
            if (pos.X < 0) {
                pos.x += g.width;
            }
            if (pos.Y >= g.height) {
                pos.y -= g.height;
            }
            if (pos.Y < 0) {
                pos.y += g.height;
            }
            if (lastPos.X != pos.X || lastPos.Y != pos.Y) {
                g.Place(new Plasma(lastPos, 10));
            }
            if(g.Collide(pos, out Entity other)) {
                Collide(g, other);
            }
            */
            if(hp > 0) {
                g.Place(this);
            }
        }
        public void Draw(SpriteBatch g) {
            g.Draw(Sprites.player, pos.round() * 16, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); //new Rectangle((pos - new Point(8, 8)).point, new Point(16, 16).point);
        }
    }
}
