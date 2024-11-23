using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class CaptchaGeneratorWPF
{
    private static readonly Random Random = new Random();

    private static string GenerateRandomText()
    {
        int length = Random.Next(4, 8);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] captchaChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            captchaChars[i] = chars[Random.Next(chars.Length)];
        }
        return new string(captchaChars);
    }
    public static RenderTargetBitmap GenerateCaptchaImage(out string captchaText)
    {
        captchaText = GenerateRandomText();
        int width = 200;
        int height = 70;
        DrawingVisual visual = new DrawingVisual();
        using (DrawingContext dc = visual.RenderOpen())
        {
            DrawBackground(dc, width, height);
            DrawCaptchaText(dc, captchaText, width, height);
        }
        RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(visual);
        return bitmap;
    }

    private static void DrawBackground(DrawingContext dc, int width, int height)
    {
        dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));
        for (int i = 0; i < 100; i++)
        {
            double x = Random.NextDouble() * width;
            double y = Random.NextDouble() * height;
            dc.DrawEllipse(Brushes.LightGray, null, new Point(x, y), 1, 1);
        }
        for (int i = 0; i < 5; i++)
        {
            Point start = new Point(Random.Next(width), Random.Next(height));
            Point end = new Point(Random.Next(width), Random.Next(height));
            dc.DrawLine(new Pen(Brushes.Black, 1), start, end);
        }
    }
    private static void DrawCaptchaText(DrawingContext dc, string text, int width, int height)
    {
        double x = 10;
        foreach (char c in text)
        {
            double fontSize = Random.Next(20, 30);
            double angle = Random.Next(-20, 20);
            Color color = Color.FromRgb((byte)Random.Next(50, 200), (byte)Random.Next(50, 200), (byte)Random.Next(50, 200));
            Brush brush = new SolidColorBrush(color);
            FormattedText formattedText = new FormattedText(c.ToString(), System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), fontSize, brush, 1.0);
            dc.PushTransform(new TranslateTransform(x, height / 2));
            dc.PushTransform(new RotateTransform(angle));
            dc.DrawText(formattedText, new Point(0, -formattedText.Height / 2));
            dc.Pop();
            dc.Pop();
            x += fontSize - 5;
        }
    }
}
