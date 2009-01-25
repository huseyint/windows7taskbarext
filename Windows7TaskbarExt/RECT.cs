namespace Huseyint.Windows7.Native
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public int Height
        {
            get
            {
                return this.Bottom - this.Top;
            }
        }

        public int Width
        {
            get
            {
                return this.Right - this.Left;
            }
        }

        public Size Size
        {
            get
            {
                return new Size(this.Width, this.Height);
            }
        }

        public Point Location
        {
            get
            {
                return new Point(this.Left, this.Top);
            }
        }

        public static implicit operator Rectangle(RECT rect)
        {
            return rect.ToRectangle();
        }

        public static implicit operator RECT(Rectangle rect)
        {
            return FromRectangle(rect);
        }

        public static RECT FromRectangle(Rectangle rectangle)
        {
            return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        // Handy method for converting to a System.Drawing.Rectangle
        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(this.Left, this.Top, this.Right, this.Bottom);
        }

        public override int GetHashCode()
        {
            return this.Left ^ ((this.Top << 13) | (this.Top >> 0x13))
              ^ ((this.Width << 0x1a) | (this.Width >> 6))
              ^ ((this.Height << 7) | (this.Height >> 0x19));
        }
    }
}