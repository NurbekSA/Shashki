using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Channels;
using System.IO;
using System.ComponentModel.Design;

namespace ConsoleApp122_2023_09_28
{
    public class Program
    {
        public static int isX = 0;
        public static int isY = 7;
        public static List<List<int>> pole = new List<List<int>>(8);
        public static Network network = new Network();
        public enum wdo { cant_go, can_go, not_over };
        public static void Main(string[] args)
        {
            toltyru();
            Console.OutputEncoding = Encoding.UTF8;

            network.GetOtpravkaAsync();


            Console.Write("IP: ");
            string ip = Console.ReadLine();
            network.GetPriniat(ip); // Басқа комптың айпиін енгізу

            Console.WriteLine("Red or Blue r/b");
            string str = Console.ReadLine();
            Console.Clear();
            Drow.DrawChessboard();
            if (str == "r")
            {
                network.zhdatOtvetNaparni();
            }
            else if (str == "b")
            {
                tandau();
            }


            Console.ReadLine();

        }
        static public void toltyru()
        {
            for (int i = 0; i < 8; i++)
            {
                List<int> row = new List<int>(8);
                pole.Add(row);
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pole[i].Add(0);
                }
            }
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i < 3)
                    {
                        if (i % 2 == 0)
                        {
                            if (j % 2 == 1)
                            {
                                pole[i][j] = 1;
                            }
                            else
                            {
                                pole[i][j] = 0;
                            }

                        }
                        else
                        {
                            if (j % 2 == 1)
                            {
                                pole[i][j] = 0;
                            }
                            else
                            {
                                pole[i][j] = 1;
                            }
                        }
                    }
                    if (i > 4)
                    {
                        if (i % 2 == 0)
                        {
                            if (j % 2 == 1)
                            {
                                pole[i][j] = 2;
                            }
                            else
                            {
                                pole[i][j] = 0;
                            }

                        }
                        else
                        {
                            if (j % 2 == 1)
                            {
                                pole[i][j] = 0;
                            }
                            else
                            {
                                pole[i][j] = 2;
                            }
                        }
                    }


                }
            }
        }
        static public void show()
        {
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 8; j++)
                {
                    if (isX == j && isY == i)
                    {
                        Drow.DrawYaceika(i, j, ConsoleColor.Yellow);
                    }
                    else if (pole[i][j] == 1)
                    {
                        Drow.DrawYaceika(i, j, ConsoleColor.DarkCyan);

                    }
                    else if (pole[i][j] == 2)
                    {
                        Drow.DrawYaceika(i, j, ConsoleColor.Gray);
                    }
                    else
                    {
                        Drow.DrawYaceika(i, j, ConsoleColor.Black);
                    }
                }


            }

        }
        static public void tandau()
        {
            int whooseX = -1;
            int whooseY = -1;
            bool workeWhenHones = false;

            while (true)
            {
                

                show();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                      
                        if (workeWhenHones)
                        {
                            wdo tho = wdo.not_over;
                            while (tho == wdo.not_over)
                            {
                                tho = chekWhooseSvoi(isX, isY, whooseX, whooseY);
                                if (tho == wdo.can_go)
                                {

                                    Console.SetCursorPosition(0, Console.CursorTop - 8);
                                    Console.Write(new string(' ', Console.WindowWidth));
                                    show();
                                    network.otpravitOtvetNaparniku<List<List<int>>>(pole);
                                    network.zhdatOtvetNaparni();
                                }
                            }

                            workeWhenHones = !workeWhenHones;
                        }
                        else
                        {
                            whooseX = isX;
                            whooseY = isY;
                            workeWhenHones = !workeWhenHones;
                        }

                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new string(' ', Console.WindowWidth));
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        if (isX > 1)
                        {
                            isX -= 2;
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        if (isX < 7)
                        {
                            isX += 2;
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        if (isX > 0 && isY < 7)
                        {
                            isY++;
                            isX--;
                        }
                        else if (isX == 0 && isY > 0)
                        {

                            isY++;
                            isX++;
                        }


                    }
                    else if (keyInfo.Key == ConsoleKey.UpArrow)
                    {

                        if (isY > 0 && isX < 7)
                        {
                            isY--;
                            isX++;
                        }
                        else if (isY > 0)
                        {
                            isY--;
                            isX--;
                        }
                    }

                    Console.SetCursorPosition(0, Console.CursorTop - 8);
                    Console.Write(new string(' ', Console.WindowWidth));
                
                }
            
        }
        static wdo chekWhooseSvoi(int x, int y, int oldX, int oldY)
        {

            if (pole[y][x] == 0)
            {
                if (y - oldY == 2 || y - oldY == -2)
                {
                    if (pole[(y + oldY) / 2][(x + oldX) / 2] == 1)
                    {
                        pole[(y + oldY) / 2][(x + oldX) / 2] = 0;
                        if (y == 0) pole[y][x] = 4;
                        else pole[y][x] = 2;

                        pole[oldY][oldX] = 0;
                        if (eat(x, y))
                        {
                            return wdo.not_over;
                        }
                        return wdo.can_go;
                    }
                    else return wdo.cant_go;
                }


                if (y - oldY == -1)
                {
                    if (pole[y][x] == 0)
                    {
                        if (y == 0) pole[y][x] = 4;
                        else pole[y][x] = 2;
                        pole[oldY][oldX] = 0;
                        return wdo.can_go;
                    }

                }
                return wdo.cant_go;
            }
            return wdo.cant_go;
        }

        static bool eat(int x, int y)
        {
            if (y < 2)
            {
                if (x < 2)
                {
                    if (pole[y + 1][x + 1] == 1) { if (pole[y + 2][x + 2] == 0) { return true; } }
                }
                if (x > 5)
                {
                    if (pole[y + 1][x - 1] == 1) { if (pole[y + 2][x - 2] == 0) { return true; } }

                }
                else if (pole[y + 1][x + 1] == 1) { if (pole[y + 2][x + 2] == 0) { return true; } }
                else if(pole[y + 1][x - 1] == 1) { if (pole[y + 2][x - 2] == 0) { return true; } }




            }
            if (y > 5)
            {
                if (x < 2)
                {
                    if (pole[y - 1][x + 1] == 1) { if (pole[y - 2][x + 2] == 0) { return true; } }
                }
                if (x > 5)
                {
                    if (pole[y - 1][x - 1] == 1) { if (pole[y - 2][x - 2] == 0) { return true; } }

                }
                else if(pole[y - 1][x + 1] == 1) { if (pole[y - 2][x + 2] == 0) { return true; } }
                else if(pole[y - 1][x - 1] == 1) { if (pole[y - 2][x - 2] == 0) { return true; } } 
                
                
            }    
            return false;

        }


    }

}