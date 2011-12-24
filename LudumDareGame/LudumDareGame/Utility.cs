using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LudumDareGame {
    abstract class Utility {

        //Method graciously borrowed from http://www.krissteele.net/blogdetails.aspx?id=222
        public static bool CheckPerPixel(Texture2D texture1, Texture2D texture2, Vector2 position1, Vector2 position2) {
            bool perPixelCollission = false;

            uint[] bitsA = new uint[texture1.Width * texture1.Height];
            uint[] bitsB = new uint[texture2.Width * texture2.Height];

            Rectangle texture1Rectangle = new Rectangle(Convert.ToInt32(position1.X), Convert.ToInt32(position1.Y), texture1.Width, texture1.Height);
            Rectangle texture2Rectangle = new Rectangle(Convert.ToInt32(position2.X), Convert.ToInt32(position2.Y), texture2.Width, texture2.Height);

            texture1.GetData<uint>(bitsA);
            texture2.GetData<uint>(bitsB);

            int x1 = Math.Max(texture1Rectangle.X, texture2Rectangle.X);
            int x2 = Math.Min(texture1Rectangle.X + texture1Rectangle.Width, texture2Rectangle.X + texture2Rectangle.Width);

            int y1 = Math.Max(texture1Rectangle.Y, texture2Rectangle.Y);
            int y2 = Math.Min(texture1Rectangle.Y + texture1Rectangle.Height, texture2Rectangle.Y + texture2Rectangle.Height);

            for (int y = y1; y < y2; ++y) {
                for (int x = x1; x < x2; ++x) {
                    if (((bitsA[(x - texture1Rectangle.X) + (y - texture1Rectangle.Y) * texture1Rectangle.Width] & 0xFF000000) >> 24) > 20 &&
                        ((bitsB[(x - texture2Rectangle.X) + (y - texture2Rectangle.Y) * texture2Rectangle.Width] & 0xFF000000) >> 24) > 20) {
                        perPixelCollission = true;
                        break;
                    }
                }

                // Reduce amount of looping by breaking out of this.
                if (perPixelCollission) {
                    break;
                }
            }

            return perPixelCollission;
        }

        //Method lovingly borrowed from http://stuckinprogramming.blogspot.com/2011/01/how-to-flip-texture2d-vertically-or.html
        public static Texture2D Flip(Texture2D source, bool vertical, bool horizontal) {
            Texture2D flipped = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] flippedData = new Color[data.Length];

            source.GetData(data);

            for (int x = 0; x < source.Width; x++)
                for (int y = 0; y < source.Height; y++) {
                    int idx = (horizontal ? source.Width - 1 - x : x) + ((vertical ? source.Height - 1 - y : y) * source.Width);
                    flippedData[x + y * source.Width] = data[idx];
                }

            flipped.SetData(flippedData);

            return flipped;
        }  

    }
}
