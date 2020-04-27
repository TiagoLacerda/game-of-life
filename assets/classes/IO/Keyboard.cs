using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameOfLife.assets.classes.IO
{
    public static class Keyboard
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(UInt16 virtualKeyCode);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(UInt16 virtualKeyCode);

        public enum Key { ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, SPACE, LEFT, RIGHT, UP, DOWN};

        private static Dictionary<Key, KeyInfo> keys = new Dictionary<Key, KeyInfo>() {
            { Key.C, new KeyInfo(0x43) },
            { Key.D, new KeyInfo(0x44) },
            { Key.L, new KeyInfo(0x4C) },
            { Key.R, new KeyInfo(0x52) },
            { Key.W, new KeyInfo(0x57) },
            { Key.SPACE, new KeyInfo(0x20) },
            { Key.LEFT,  new KeyInfo(0x25) },
            { Key.UP,    new KeyInfo(0x26) },
            { Key.RIGHT, new KeyInfo(0x27) },
            { Key.DOWN,  new KeyInfo(0x28) }
        };

        // This implementation only works in an environment where IsKeyDown is constantly called in a loop. TLDR: it's bad
        public static bool IsKeyDown(Key key)
        {
            if (!keys.ContainsKey(key))
                return false;

            if (GetAsyncKeyState(keys[key].code) >> 15 != 0)
            {
                if (keys[key].held)
                {
                    return false;
                }
                else
                {
                    keys[key].held = true;
                    return true;
                }
            }
            else
            {
                keys[key].held = false;
                return false;
            }
        }
    }

    class KeyInfo
    {
        public ushort code;
        public bool held;

        public KeyInfo(ushort code)
        {
            this.code = code;
            held = false;
        }
    }
}
