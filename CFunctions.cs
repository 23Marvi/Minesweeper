using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper {
    class CFunctions {
        public static bool AroundClick(Point A, Point B) {
            for (int y = A.Y - 1; y < A.Y + 2; y++) {
                for (int x = A.X - 1; x < A.X + 2; x++) {
                    if (new Point(x, y) == B) return true;
                }
            } return false;
        }

        public static bool WithinBounds(Rectangle A, Rectangle B) {
            if (A.Location.X + A.Size.Width >= B.X && A.Location.X <= B.Location.X + B.Width) {
                if (A.Location.Y + A.Size.Height >= B.Y && A.Location.Y <= B.Location.Y + B.Height) {
                    return true;
                }
            } return false;
        }
    }
}
