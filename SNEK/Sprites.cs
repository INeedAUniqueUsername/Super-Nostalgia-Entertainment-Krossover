using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEK {
    class Sprites {
        public static Texture2D logo, player, enemy, plasma, laser, fragment;
        public static void Initialize(ContentManager c) {
            //logo = c.Load<Texture2D>("Logo");
            player = c.Load<Texture2D>("Player");
            enemy = c.Load<Texture2D>("Drone");
            plasma = c.Load<Texture2D>("Plasma");
            laser = c.Load<Texture2D>("Laser");
            fragment = c.Load<Texture2D>("Fragment");
        }
    }
}
