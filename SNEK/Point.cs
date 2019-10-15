using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Point2 = Microsoft.Xna.Framework.Point;
namespace SNEK {
    class Point {
        public static Point ZERO = new Point(0, 0);
        public double x, y;
        public int X => (int) Math.Round(x);
        public int Y => (int) Math.Round(y);
        public Point(double x, double y) {
            this.x = x;
            this.y = y;
        }
        public double magnitude => Math.Sqrt(x * x + y * y);
        public Point normal => new Point(x / magnitude, y / magnitude);
        public Point round() => new Point(X, Y);
        public static implicit operator Vector2(Point p) => new Vector2((float) p.x, (float) p.y);
        public static implicit operator Point2(Point p) => new Point2(p.X, p.Y);
        public static Point operator+(Point p1, Point p2) {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }
        public static Point operator -(Point p1, Point p2) => new Point(p1.x - p2.x, p1.y - p2.y);
        public static Point operator *(Point p, double scale) => new Point(p.x * scale, p.y * scale);
    }
    static class PointHelper {
        public static Point Constrain(this Point pos, World g) {
            if (pos.X >= g.width) {
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
            return pos;
        }
    }
}
