using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEK {
    interface Moving : Entity {
        Point vel { get; }
    }
}
