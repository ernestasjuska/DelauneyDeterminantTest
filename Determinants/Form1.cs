using System.Diagnostics;
using System.Numerics;

namespace Determinants
{
    public partial class Form1 : Form
    {
        private readonly Bitmap _bitmap;
        private readonly Graphics _graphics;

        private const float _sz = 10;
        private const float _wd = 1;

        private int _idx = 3;
        private Vector2[] _vs = [new(100, 100), new(200, 100), new(100, 200), new(150, 150)];
        private int _n = 3;
        private bool _drawd;

        public Form1()
        {
            InitializeComponent();

            _bitmap = new(512, 512);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            _graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            _graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DoRefresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _idx = (_idx + 1) % _vs.Length;
                
                DoRefresh();

                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                _vs[_idx].X = e.X;
                _vs[_idx].Y = e.Y;

                DoRefresh();
                
                return;
            }

            if (e.Button == MouseButtons.Middle)
            {
                _drawd = !_drawd;

                DoRefresh();

                return;
            }
        }

        private void DoRefresh()
        {
            _graphics.Clear(Color.Black);

            string deta = ExpandDeterminant(
                "a.X", "a.Y", "a.X * a.X + a.Y + a.Y", "1",
                "b.X", "b.Y", "b.X * b.X + b.Y + b.Y", "1",
                "c.X", "c.Y", "c.X * c.X + c.Y + c.Y", "1",
                "p.X", "p.Y", "p.X * p.X + p.Y + p.Y", "1");
            string detb = ExpandDeterminant(
                "a.X - p.X", "a.Y - p.Y", "(a.X - p.X) * (a.X - p.X) + (a.Y - p.Y) * (a.Y - p.Y)",
                "b.X - p.X", "b.Y - p.Y", "(b.X - p.X) * (b.X - p.X) + (b.Y - p.Y) * (b.Y - p.Y)",
                "c.X - p.X", "c.Y - p.Y", "(c.X - p.X) * (c.X - p.X) + (c.Y - p.Y) * (c.Y - p.Y)");
            Debug.WriteLine(deta);
            Debug.WriteLine(detb);

            Vector2 a = _vs[0];
            Vector2 b = _vs[1];
            Vector2 c = _vs[2];

            if (_drawd)
            {
                for (int y = 0; y < _bitmap.Height; y++)
                {
                    for (int x = 0; x < _bitmap.Width; x++)
                    {
                        Vector2 d = new(x, y);

                        float detm4 = Determinant(
                            a.X, a.Y, a.X * a.X + a.Y + a.Y, 1,
                            b.X, b.Y, b.X * b.X + b.Y + b.Y, 1,
                            c.X, c.Y, c.X * c.X + c.Y + c.Y, 1,
                            d.X, d.Y, d.X * d.X + d.Y + d.Y, 1);

                        detm4 = Determinant(
                            a.X - d.X, a.Y - d.Y, (a.X - d.X) * (a.X - d.X) + (a.Y - d.Y) * (a.Y - d.Y),
                            b.X - d.X, b.Y - d.Y, (b.X - d.X) * (b.X - d.X) + (b.Y - d.Y) * (b.Y - d.Y),
                            c.X - d.X, c.Y - d.Y, (c.X - d.X) * (c.X - d.X) + (c.Y - d.Y) * (c.Y - d.Y));

                        detm4 =
                            +a.Y * a.Y * b.Y * d.X
                            + a.Y * a.Y * b.X * c.Y
                            + a.Y * a.Y * c.X * d.Y
                            + a.Y * b.Y * b.Y * c.X
                            + a.Y * b.X * b.X * c.X
                            + a.Y * b.X * d.Y * d.Y
                            + a.Y * b.X * d.X * d.X
                            + a.Y * c.Y * c.Y * d.X
                            + a.Y * c.X * c.X * d.X
                            + a.X * a.X * b.Y * d.X
                            + a.X * a.X * b.X * c.Y
                            + a.X * a.X * c.X * d.Y
                            + a.X * b.Y * b.Y * d.Y
                            + a.X * b.Y * c.Y * c.Y
                            + a.X * b.Y * c.X * c.X
                            + a.X * b.X * b.X * d.Y
                            + a.X * c.Y * d.Y * d.Y
                            + a.X * c.Y * d.X * d.X
                            + b.Y * b.Y * c.Y * d.X
                            + b.Y * c.X * d.Y * d.Y
                            + b.Y * c.X * d.X * d.X
                            + b.X * b.X * c.Y * d.X
                            + b.X * c.Y * c.Y * d.Y
                            + b.X * c.X * c.X * d.Y
                            - a.Y * a.Y * b.Y * c.X
                            - a.Y * a.Y * b.X * d.Y
                            - a.Y * a.Y * c.Y * d.X
                            - a.Y * b.Y * b.Y * d.X
                            - a.Y * b.X * b.X * d.X
                            - a.Y * b.X * c.Y * c.Y
                            - a.Y * b.X * c.X * c.X
                            - a.Y * c.X * d.Y * d.Y
                            - a.Y * c.X * d.X * d.X
                            - a.X * a.X * b.Y * c.X
                            - a.X * a.X * b.X * d.Y
                            - a.X * a.X * c.Y * d.X
                            - a.X * b.Y * b.Y * c.Y
                            - a.X * b.Y * d.Y * d.Y
                            - a.X * b.Y * d.X * d.X
                            - a.X * b.X * b.X * c.Y
                            - a.X * c.Y * c.Y * d.Y
                            - a.X * c.X * c.X * d.Y
                            - b.Y * b.Y * c.X * d.Y
                            - b.Y * c.Y * c.Y * d.X
                            - b.Y * c.X * c.X * d.X
                            - b.X * b.X * c.X * d.Y
                            - b.X * c.Y * d.Y * d.Y
                            - b.X * c.Y * d.X * d.X;

                        detm4 =
                            +a.Y * b.X * b.X * c.X
                            + a.Y * b.X * d.X * d.X
                            + a.Y * c.X * c.X * d.X
                            + a.X * a.X * b.Y * d.X
                            + a.X * a.X * b.X * c.Y
                            + a.X * a.X * c.X * d.Y
                            + a.X * b.Y * c.X * c.X
                            + a.X * b.X * b.X * d.Y
                            + a.X * c.Y * d.X * d.X
                            + b.Y * c.X * d.X * d.X
                            + b.X * b.X * c.Y * d.X
                            + b.X * c.X * c.X * d.Y
                            - a.Y * b.X * b.X * d.X
                            - a.Y * b.X * c.X * c.X
                            - a.Y * c.X * d.X * d.X
                            - a.X * a.X * b.Y * c.X
                            - a.X * a.X * b.X * d.Y
                            - a.X * a.X * c.Y * d.X
                            - a.X * b.Y * d.X * d.X
                            - a.X * b.X * b.X * c.Y
                            - a.X * c.X * c.X * d.Y
                            - b.Y * c.X * c.X * d.X
                            - b.X * b.X * c.X * d.Y
                            - b.X * c.Y * d.X * d.X;

                        Brush brush = detm4 switch
                        {
                            > 0 => Brushes.DarkRed,
                            < 0 => Brushes.DarkBlue,
                            _ => Brushes.DarkGreen,
                        };
                        _graphics.FillRectangle(brush, x, y, 1f, 1f);
                    }
                }
            }

            var ct = CircumcircleTriangle(a, b, c);

            _graphics.DrawEllipse(Pens.Cyan, ct.p.X - ct.r, ct.p.Y - ct.r, 2 * ct.r, 2 * ct.r);
            _graphics.DrawEllipse(Pens.Cyan, ct.p.X - _sz / 2, ct.p.Y - _sz / 2, _sz, _sz);

            for (int i = 0; i < _vs.Length; i++)
            {
                Vector2 v = _vs[i];
                _graphics.FillEllipse(Brushes.Orange, v.X - _sz / 2, v.Y - _sz / 2, _sz, _sz);

                if (i == _idx)
                {
                    _graphics.DrawEllipse(Pens.Yellow, v.X - _sz, v.Y - _sz, 2 * _sz, 2 * _sz);
                }
            }

            for (int i = 0; i < _n; i++)
            {
                Vector2 v = _vs[i];
                _graphics.DrawString(i.ToString() + ":" + v.ToString("000"), SystemFonts.DefaultFont, Brushes.Gray, v.X, v.Y);
            }

            _graphics.DrawString("CURSOR:" + _vs[3].ToString("000"), SystemFonts.DefaultFont, Brushes.White, _vs[3].X, _vs[3].Y);

            float area = 0.5f * Enumerable.Range(0, _n).Select(i =>
            {
                int j = (i + 1) % _n;
                return _vs[i].X * _vs[j].Y - _vs[j].X * _vs[i].Y;
            }).Sum();

            float detm =
                _vs[0].X * _vs[1].Y - _vs[1].X * _vs[0].Y +
                _vs[1].X * _vs[2].Y - _vs[2].X * _vs[1].Y +
                _vs[2].X * _vs[0].Y - _vs[0].X * _vs[2].Y;

            float detm2 = Determinant(
                _vs[0].X, _vs[0].Y, 1,
                _vs[1].X, _vs[1].Y, 1,
                _vs[2].X, _vs[2].Y, 1);

            float detm3 = Determinant(
                _vs[0].X, _vs[0].Y, _vs[0].X * _vs[0].X + _vs[0].Y + _vs[0].Y, 1,
                _vs[1].X, _vs[1].Y, _vs[1].X * _vs[1].X + _vs[1].Y + _vs[1].Y, 1,
                _vs[2].X, _vs[2].Y, _vs[2].X * _vs[2].X + _vs[2].Y + _vs[2].Y, 1,
                _vs[3].X, _vs[3].Y, _vs[3].X * _vs[3].X + _vs[3].Y + _vs[3].Y, 1);

            Text = $"Area = {area}; Clockwise order = {area > 0}; Determinant = {detm}; Determinant2 = {detm2}; Determinant3 = {detm3}";

            pictureBox1.Image = _bitmap;
        }

        private static (Vector2 p, float r) CircumcircleTriangle(Vector2 a, Vector2 b, Vector2 c)
        {
            float x1 = a.X;
            float y1 = a.Y;
            float x2 = b.X;
            float y2 = b.Y;
            float x3 = c.X;
            float y3 = c.Y;

            float da = Determinant(
                x1, y1, 1,
                x2, y2, 1,
                x3, y3, 1);
            float dbx = -Determinant(
                x1 * x1 + y1 * y1, y1, 1,
                x2 * x2 + y2 * y2, y2, 1,
                x3 * x3 + y3 * y3, y3, 1);
            float dby = Determinant(
                x1 * x1 + y1 * y1, x1, 1,
                x2 * x2 + y2 * y2, x2, 1,
                x3 * x3 + y3 * y3, x3, 1);

            float cx = -dbx / 2.0f / da;
            float cy = -dby / 2.0f / da;

            Vector2 p = new(cx, cy);
            float r = (p - a).Length();

            return (p, r);
        }

        private static float Determinant(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            return
                m11 * Determinant(
                    m22, m23, m24,
                    m32, m33, m34,
                    m42, m43, m44) -
                m12 * Determinant(
                    m21, m23, m24,
                    m31, m33, m34,
                    m41, m43, m44) +
                m13 * Determinant(
                    m21, m22, m24,
                    m31, m32, m34,
                    m41, m42, m44) -
                m14 * Determinant(
                    m21, m22, m23,
                    m31, m32, m33,
                    m41, m42, m43);
        }

        private static float Determinant(
            float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33)
        {
            return
                m11 * Determinant(
                    m22, m23,
                    m32, m33) -
                m12 * Determinant(
                    m21, m23,
                    m31, m33) +
                m13 * Determinant(
                    m21, m22,
                    m31, m32);
        }

        private static float Determinant(
            float m11, float m12,
            float m21, float m22)
        {
            return
                m11 * m22 -
                m12 * m21;
        }

        private static string ExpandDeterminant(
            string m11, string m12, string m13, string m14,
            string m21, string m22, string m23, string m24,
            string m31, string m32, string m33, string m34,
            string m41, string m42, string m43, string m44)
        {
            return
                $"(({m11})*({ExpandDeterminant(
                    m22, m23, m24,
                    m32, m33, m34,
                    m42, m43, m44)})-" +
                $"({m12})*({ExpandDeterminant(
                    m21, m23, m24,
                    m31, m33, m34,
                    m41, m43, m44)})+" +
                $"({m13})*({ExpandDeterminant(
                    m21, m22, m24,
                    m31, m32, m34,
                    m41, m42, m44)})-" +
                $"({m14})*({ExpandDeterminant(
                    m21, m22, m23,
                    m31, m32, m33,
                    m41, m42, m43)}))";
        }

        private static string ExpandDeterminant(
            string m11, string m12, string m13,
            string m21, string m22, string m23,
            string m31, string m32, string m33)
        {
            return
                $"(({m11})*({ExpandDeterminant(
                    m22, m23,
                    m32, m33)})-" +
                $"({m12})*({ExpandDeterminant(
                    m21, m23,
                    m31, m33)})+" +
                $"({m13})*({ExpandDeterminant(
                    m21, m22,
                    m31, m32)}))";
        }

        private static string ExpandDeterminant(
            string m11, string m12,
            string m21, string m22)
        {
            return
                $"(({m11})*({m22})-" +
                $"({m12})*({m21}))";
        }
    }
}
