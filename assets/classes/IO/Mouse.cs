using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Point = System.Drawing.Point;

namespace GameOfLife.assets.classes.IO
{
    public static class Mouse
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(UInt16 virtualKeyCode);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(UInt16 virtualKeyCode);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        public enum Button { Left, Right };

        private static Dictionary<Button, UInt16> buttons = new Dictionary<Button, ushort>()
        {
            { Button.Left, 0x01 },
            { Button.Right, 0x02 },
        };

        public static bool IsButtonPressed(Button button)
        {
            return (GetKeyState(buttons[button]) >> 15 != 0);
        }

        public static bool IsAnyButtonPressed()
        {
            foreach (var code in buttons.Values)
                if (GetKeyState(code) >> 15 != 0)
                    return true;

            return false;
        }

        // MSDN System.Drawing.Point, not GameOfLife.assets.classes.Mathematics.Point
        public static Point GetPosition()
        {
            GetCursorPos(out Point lpPoint);
            return lpPoint;
        }
    }
}
