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
        public static Point ANGLE_ZERO = new Point(1, 0);
        public double x, y;
        public int X => (int) Math.Round(x);
        public int Y => (int) Math.Round(y);
        public Point(double x, double y) {
            this.x = x;
            this.y = y;
        }
        public Point(double angle) {
            this.x = Math.Cos(angle);
            this.y = Math.Sin(angle);
        }
        public Point(Vector2 v) {
            this.x = v.X;
            this.y = v.Y;
        }
        public double angle => Math.Atan2(y, x);
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
        public Point rotate(double angle) => new Point(magnitude * Math.Cos(this.angle + angle), magnitude * Math.Sin(this.angle + angle));
        public double dot(Point p) => x * p.x + y * p.y;
    }
    static class Points {
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
        public static Point Closest(this Point pos, params Point[] points) {
            Point result = points[0];
            double resultDistance = (pos - result).magnitude;
            foreach(Point p in points.Skip(1)) {
                double distance = (pos - p).magnitude;
                if(distance < resultDistance) {
                    result = p;
                }
            }
            return result;
        }

    }
}
