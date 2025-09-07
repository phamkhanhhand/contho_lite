using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CT.Utils
{

    /// <summary>
    /// Capcha
    /// </summary>
    public class CapchaHelper
    {

        #region constant
        private const string CharBank = "ABCDEFGHJKLNPQRSTUVXYZW12346789";
        private const FontSizeRatioLevel FontSizeRatio = FontSizeRatioLevel.Low;
        private const BackgroundNoiseLevel BackgroundNoise = BackgroundNoiseLevel.Low;
        private const LineNoiseLevel LineNoise = LineNoiseLevel.None;
        private const int Height = 50;
        private const int Width = 180;
        private const int TextLength = 5;
        private static readonly List<string> ListFont = new List<string>()
        {
            "arial",
            "arial black",
            "comic sans ms",
            "courier new",
            "estrangelo edessa",
            "franklin gothic medium",
            "georgia",
            "lucida console",
            "lucida sans unicode",
            "mangal",
            "microsoft sans serif",
            "palatino linotype",
            "sylfaen",
            "tahoma",
            "times new roman",
            "trebuchet ms",
            "verdana"
        };
        #endregion

        private readonly Random _rand;
        private readonly DateTime _generatedAt;
        private readonly string _guid;

        public CapchaHelper()
        {
            _rand = new Random();

            _generatedAt = DateTime.Now;
            _guid = Guid.NewGuid().ToString();
        }

        #region public method
        public string RenderImageBase64(out string text)
        {
            Bitmap image = RenderImage(out text);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();

                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        public Bitmap RenderImage(out string text)
        {
            return GenerateImage(out text);
        }
        #endregion

        /// <summary>
        /// Renders the CAPTCHA image
        /// </summary>
        private Bitmap GenerateImage(out string text)
        {
            Font fnt = null;
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            //-- fill an empty white rectangle
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            Brush br = new SolidBrush(Color.White);
            gr.FillRectangle(br, rect);

            text = GenerateRandomText();
            int charOffset = 0;
            double charWidth = Width / TextLength;
            foreach (char c in text)
            {
                // gen font
                fnt = GenerateRandomFont();
                Rectangle rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), Height);

                // gen text graphic
                GraphicsPath gp = GenerateTextGraphic(c.ToString(), fnt, rectChar);

                // draw the character
                br = new SolidBrush(Color.Black);
                gr.FillPath(br, gp);

                charOffset += 1;
            }

            AddNoise(gr, rect);
            AddLine(gr, rect);

            //-- clean up unmanaged resources
            if (fnt != null) fnt.Dispose();
            br.Dispose();
            gr.Dispose();

            return bmp;
        }

        /// <summary>
        /// generate random text for the CAPTCHA
        /// </summary>
        private string GenerateRandomText()
        {
            string text = string.Empty;

            while (text.Length < TextLength)
            {
                int index = _rand.Next(0, CharBank.Length);
                int sensitiveCase = _rand.Next(0, 2);
                char ch = CharBank[index];
                if (sensitiveCase == 1)
                    text = text + ch.ToString().ToUpper();
                else
                    text = text + ch.ToString().ToLower();
            }
            return text;
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size
        /// </summary>
        private Font GenerateRandomFont()
        {
            float fsize;

            int fontNameIndex = _rand.Next(0, ListFont.Count);
            string fname = ListFont[fontNameIndex];
            //switch (FontSizeRatio)
            //{
            //    case FontSizeRatioLevel.Low:
            //        fsize = Convert.ToInt32(Height * 0.8);
            //        break;
            //    case FontSizeRatioLevel.Medium:
            //        fsize = Convert.ToInt32(Height * 0.85);
            //        break;
            //    case FontSizeRatioLevel.High:
            //        fsize = Convert.ToInt32(Height * 0.9);
            //        break;
            //    case FontSizeRatioLevel.Extreme:
            //        fsize = Convert.ToInt32(Height * 0.95);
            //        break;
            //    default:
            //        fsize = Convert.ToInt32(Height * 0.7);
            //        break;
            //}
            fsize = Convert.ToInt32(Height * 0.8);

            return new Font(fname, fsize, FontStyle.Bold);
        }

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font
        /// </summary>
        private GraphicsPath GenerateTextGraphic(string text, Font font, Rectangle rectangle)
        {
            StringFormat sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(text, font.FontFamily, (int)font.Style, font.Size, rectangle, sf);

            double warpDivisor = 0.0;
            double rangeModifier = 0.0;

            //switch (FontSizeRatio)
            //{
            //    case FontSizeRatioLevel.None:
            //        return gp;
            //    case FontSizeRatioLevel.Low:
            //        warpDivisor = 6;
            //        rangeModifier = 1;
            //        break;
            //    case FontSizeRatioLevel.Medium:
            //        warpDivisor = 5;
            //        rangeModifier = 1.3;
            //        break;
            //    case FontSizeRatioLevel.High:
            //        warpDivisor = 4.5;
            //        rangeModifier = 1.4;
            //        break;
            //    case FontSizeRatioLevel.Extreme:
            //        warpDivisor = 4;
            //        rangeModifier = 1.5;
            //        break;
            //}
            warpDivisor = 6;
            rangeModifier = 1;

            RectangleF rectF = new RectangleF(Convert.ToSingle(rectangle.Left), 0, Convert.ToSingle(rectangle.Width), rectangle.Height);

            int hrange = Convert.ToInt32(rectangle.Height / warpDivisor);
            int wrange = Convert.ToInt32(rectangle.Width / warpDivisor);
            int left = rectangle.Left - Convert.ToInt32(wrange * rangeModifier);
            int top = rectangle.Top - Convert.ToInt32(hrange * rangeModifier);
            int width = rectangle.Left + rectangle.Width + Convert.ToInt32(wrange * rangeModifier);
            int height = rectangle.Top + rectangle.Height + Convert.ToInt32(hrange * rangeModifier);

            if (left < 0)
                left = 0;
            if (top < 0)
                top = 0;
            if (width > Width)
                width = Width;
            if (height > Height)
                height = Height;

            PointF leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            PointF rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            PointF leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            PointF rightBottom = RandomPoint(width - wrange, width, height - hrange, height);

            PointF[] points = new[] { leftTop, rightTop, leftBottom, rightBottom };
            Matrix m = new Matrix();
            m.Translate(0, 0);
            gp.Warp(points, rectF, m, WarpMode.Perspective, 0);

            return gp;
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges
        /// </summary>
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(_rand.Next(xmin, xmax), _rand.Next(ymin, ymax));
        }

        /// <summary>
        /// Returns a random point within the specified rectangle
        /// </summary>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// Add a variable level of graphic noise to the image
        /// </summary>
        private void AddNoise(Graphics graphics1, Rectangle rect)
        {
            int density = 0;
            int size = 0;

            //switch (BackgroundNoise)
            //{
            //    case BackgroundNoiseLevel.None:
            //        return;
            //    case BackgroundNoiseLevel.Low:
            //        density = 30;
            //        size = 40;
            //        break;
            //    case BackgroundNoiseLevel.Medium:
            //        density = 18;
            //        size = 40;
            //        break;
            //    case BackgroundNoiseLevel.High:
            //        density = 16;
            //        size = 39;
            //        break;
            //    case BackgroundNoiseLevel.Extreme:
            //        density = 12;
            //        size = 38;
            //        break;
            //}
            density = 30;
            size = 40;

            SolidBrush br = new SolidBrush(Color.Black);
            int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

            for (int i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
            {
                graphics1.FillEllipse(br, _rand.Next(rect.Width), _rand.Next(rect.Height), _rand.Next(max),
                                      _rand.Next(max));
            }
            br.Dispose();
        }

        /// <summary>
        /// Add variable level of curved lines to the image
        /// </summary>
        private void AddLine(Graphics graphics1, Rectangle rect)
        {
            int length = 0;
            float width = 0;
            int linecount = 0;

            //switch (LineNoise)
            //{
            //    case LineNoiseLevel.None:
            //        return;
            //    case LineNoiseLevel.Low:
            //        length = 4;
            //        width = Convert.ToSingle(Height / 31.25);
            //        // 1.6
            //        linecount = 1;
            //        break;
            //    case LineNoiseLevel.Medium:
            //        length = 5;
            //        width = Convert.ToSingle(Height / 27.7777);
            //        // 1.8
            //        linecount = 1;
            //        break;
            //    case LineNoiseLevel.High:
            //        length = 3;
            //        width = Convert.ToSingle(Height / 25);
            //        // 2.0
            //        linecount = 2;
            //        break;
            //    case LineNoiseLevel.Extreme:
            //        length = 3;
            //        width = Convert.ToSingle(Height / 22.7272);
            //        // 2.2
            //        linecount = 3;
            //        break;
            //}

            PointF[] pf = new PointF[length + 1];
            Pen p = new Pen(Color.Black, width);

            for (int l = 1; l <= linecount; l++)
            {
                for (int i = 0; i <= length; i++)
                {
                    pf[i] = RandomPoint(rect);
                }
                graphics1.DrawCurve(p, pf, 1.75f);
            }

            p.Dispose();
        }

        #region class helper
        /// <summary>
        /// Amount of background noise to add to rendered image
        /// </summary>
        private enum BackgroundNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        /// <summary>
        /// Amount of random font warping to apply to rendered text
        /// </summary>
        private enum FontSizeRatioLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        /// <summary>
        /// Amount of curved line noise to add to rendered image
        /// </summary>
        private enum LineNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }
        #endregion


    }
}
