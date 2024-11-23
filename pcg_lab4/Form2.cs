using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pcg_lab4
{
    public partial class Form2 : Form
    {
        private Bitmap bitmap;
        private int pixelSize = 1; // Масштаб от 1 до 5
        private int offsetX = 0;   // Смещение по X
        private int offsetY = 0;   // Смещение по Y
        private bool showGrid = false; // Флаг для отображения координатной сетки
        private bool showCoordGrid = false;
        public Form2()
        {
            this.ClientSize = new Size(800, 800); // Размер окна
            this.Paint += new PaintEventHandler(DrawPicture);
            this.KeyDown += new KeyEventHandler(OnKeyDown); // Обработка клавиш
        }

        private void DrawPicture(object sender, PaintEventArgs e)
        {
            bitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
            if (showGrid && pixelSize == 5) DrawCoordinateGrid(); // Координатная сетка при масштабе 5x
            if (showCoordGrid) DrawGrid();
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void DrawGrid()
        {
            int centerX = ClientSize.Width / 2 + offsetX * pixelSize;
            int centerY = ClientSize.Height / 2 + offsetY * pixelSize;

            // Оси с учетом смещения и масштаба
            for (int x = 0; x < ClientSize.Width; x += pixelSize)
            {
                SetPixelBlock(centerX + x, centerY, Color.LightGray, pixelSize);
                SetPixelBlock(centerX - x, centerY, Color.LightGray, pixelSize);
            }

            for (int y = 0; y < ClientSize.Height; y += pixelSize)
            {
                SetPixelBlock(centerX, centerY + y, Color.LightGray, pixelSize);
                SetPixelBlock(centerX, centerY - y, Color.LightGray, pixelSize);
            }
        }

        private void DrawCoordinateGrid()
        {
            if (pixelSize != 5) return; // Сетка отображается только при максимальном увеличении

            int gridSpacing = pixelSize; // Интервал между линиями сетки, масштабированный
            int startX = offsetX % gridSpacing; // Начальная позиция с учётом смещения
            int startY = offsetY % gridSpacing;

            // Рисуем вертикальные линии сетки
            for (int x = startX; x < ClientSize.Width; x += gridSpacing)
            {
                for (int y = 0; y < ClientSize.Height; y++)
                {
                    SetPixelBlock(x, y, Color.Gray, 1); // 1 пиксель ширины линии
                }
            }

            // Рисуем горизонтальные линии сетки
            for (int y = startY; y < ClientSize.Height; y += gridSpacing)
            {
                for (int x = 0; x < ClientSize.Width; x++)
                {
                    SetPixelBlock(x, y, Color.Gray, 1); // 1 пиксель высоты линии
                }
            }
        }

        private void SetPixelBlock(int x, int y, Color color, int size)
        {
            for (int dx = 0; dx < size; dx++)
            {
                for (int dy = 0; dy < size; dy++)
                {
                    int px = x + dx;
                    int py = y + dy;
                    if (px >= 0 && px < bitmap.Width && py >= 0 && py < bitmap.Height)
                    {
                        bitmap.SetPixel(px, py, color);
                    }
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    pixelSize = Math.Min(pixelSize + 1, 5);
                    break;
                case Keys.Down:
                    pixelSize = Math.Max(pixelSize - 1, 1);
                    break;
                case Keys.W:
                    offsetY += 10; // Двигаемся вверх
                    break;
                case Keys.S:
                    offsetY -= 10; // Двигаемся вниз
                    break;
                case Keys.A:
                    offsetX += 10; // Двигаемся влево
                    break;
                case Keys.D:
                    offsetX -= 10; // Двигаемся вправо
                    break;
                case Keys.E:
                    if (pixelSize == 5) showGrid = !showGrid; // Включение/выключение сетки
                    break;
                case Keys.R:
                    if (!showCoordGrid)
                        showCoordGrid = true;
                    else
                        showCoordGrid = false;
                    break;
            }
            Invalidate();
        }
    }
}
