using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp122_2023_09_28
{
    class Drow
    {
        /// <start>
        /// <Бірінші> DrawChessboard ең басында шақырасын. Ол досканы сызып береді</Бірінші>
        /// 
        /// <Екінші>  Сосын DrawDomalak шақырасын. Ол Листтегі барлық шашкиді салып береді.</Екінші>
        /// 
        /// <Үшінші>  DrawYaceika ол координатада берген ячейканы сол түске бояп береді. </Үшінші>
        ///          Ячейканың таңдауды осымен істейсін
        /// </start>






        /// <summary>
        /// Доскадағы орындарды сақтайды. Басында бір табылып алып потом өзгермейді
        /// </summary>
        public static Dictionary<(int, int), (int, int)> system = new Dictionary<(int, int), (int, int)>();


        /// <summary>
        /// Досканы сызады. Және координаталарын system сөздігіне ыңғайлы қылып жазып беретін әдісті шақырады
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public static void DrawChessboard(int rows = 8, int cols = 8)
        {
            // Размер ячейки
            int cellWidth = 6;
            int cellHeight = 3;

            int x = 0, y = 0;

            // Определение общей ширины и высоты доски
            int boardWidth = cols * cellWidth;
            int boardHeight = rows * cellHeight;

            for (int i = 0; i <= boardHeight; i++)
            {
                for (int j = 0; j <= boardWidth; j++)
                {
                    if (i % cellHeight == 0 || i == boardHeight)
                    {
                        // Выводим символ для границы
                        Console.Write("-");
                    }
                    else if (j % 6 == 0 || j == boardWidth)
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        // Қара ақ ячейка сызу
                        if ((x + y) % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("■");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("■");
                        }
                        Console.ResetColor();
                        if (j % cellWidth == 5)
                        {
                            y++;
                        }
                    }
                    if (i % cellHeight == 0)
                        x++;
                }
                Console.WriteLine(); // Переход на новую строку после каждой строки

            }
            Koordinatas();

        }

        /// <summary>
        /// Ең басты осыны қолданасын. Ячейканы бояп береді
        /// </summary>
        /// <param name="x"> 0-7 арасындағы х координатасы </param>
        /// <param name="y"> 0-7 арасындағы у координатасы </param>
        /// <param name="color"> Ячейка түсі. Белгіленіп тұрған ячейканы басқа түс беріп шақыра саласын </param>
        public static void DrawYaceika(int x, int y, ConsoleColor color)
        {
            var value = system[(x, y)];
            Console.ForegroundColor = color;
            for (int i = value.Item1; i < value.Item1 + 3; i++)
            {
                for (int j = value.Item2; j < value.Item2 + 2; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("■");
                }
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Шашкиларды салып береді
        /// </summary>
        /// <param name="positions"> Шашкилардың листі келеді.</param>
        /// <param name="color"> Листті салатын түс беріледі. Союзник - қызыл. Противник - көк </param>
        /// <TODO> Листтің типін ауыстыру керек болу мүмкін  </TODO> 
        public static void DrawDomalak(List<(int, int)> positions, ConsoleColor color)
        {
            foreach (var position in positions)
            {
                DrawYaceika(position.Item1, position.Item2, color);
            }
        }



        /// <summary>
        /// Координаталарды ыңғайлы сөздікке жазып береді. Тек бір рет шақырылады. Сондықтан прайвит
        /// </summary>
        public static void Koordinatas()
        {
            int nacaloX;
            int nacaloY = 1;
            for (int i = 0; i < 8; i++)
            {
                nacaloX = 2;
                for (int j = 0; j < 8; j++)
                {
                    system.Add((i, j), (nacaloX, nacaloY));
                    nacaloX += 6;
                }
                nacaloY += 3;
            }
        }
    }
}