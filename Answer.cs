using System;
using System.Linq;
using System.Collections.Generic;

namespace MyProjects
{
    static class Answer
    {
        static public int[,] Sudoku(int[,] data)
        {
            Map map = new Map(data);

            int n = map.Length;
            int[] pattern = new int[n];
            for (int i = 0; i < n; i++)
            {
                pattern[i] = i + 1;
            }
            Stack<MapForStorage> DataBank = new Stack<MapForStorage>();

            //Заміна відсутніх елементів(0) на масив усіх можливих(по рядку)
            for (int i = 0; i < n*n; i++)
            {
                int[] present = map.GetRow(i/n);
                int[] missing = pattern.Except(present).ToArray();
                if ((int)map[i] == 0)
                {
                    map[i] = missing;
                }
            }

            bool check = true;
            while (check)
            {
                int[][] oldArray = new int[n][];
                int[][] newArray = new int[n][];
                switch(1)
                {
                    case 1:
                        #region ::Створення таблиці можливих значень::
                        for (int i = 0; i < n; i++)
                        {
                            oldArray[i] = map.GetRow(i);
                        }
                        for (int i = 0; i < n*n; i++)
                        {
                            if (map[i] is System.Int32[])
                            {
                                int[] present = map.GetRow(i/n);
                                map[i] = ((int[])map[i]).Except(present).ToArray();//виключення збігів з рядком
                                map.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                            }
                        }
                        for (int i = 0; i < n * n; i++)
                        {
                            if (map[i] is System.Int32[])
                            {
                                int[] present = map.GetCol(i % n);
                                map[i] = ((int[])map[i]).Except(present).ToArray();//виключення збігів з стовпцем
                                map.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                            }
                        }
                        for (int i = 0; i < n * n; i++)
                        {
                            if (map[i] is System.Int32[])
                            {
                                int SqrtN = (int)Math.Sqrt(n);
                                int[] present = map.GetCube((int)((i / n) / SqrtN) * SqrtN + (int)((i % n) / SqrtN));
                                map[i] = ((int[])map[i]).Except(present).ToArray();//виключення збігів з квадратом
                                map.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = map.GetRow(i);
                        }
                        if (Compare(oldArray, newArray))
                            goto case 2;
                        #endregion
                        break;
                    case 2:
                        #region ::Голі елементи в рядках::
                        //Пошук унікального елемента в рядку
                        for (int i = 0; i < n; i++)
                        {
                            oldArray[i] = map.GetRow(i);
                        }
                        for (int i = 0; i < n; i++)
                        {
                            int[] mas = new int[0];
                            List<int> count = new List<int>();
                            for (int j = 0; j < n; j++)
                            {
                                if (map[i*n+j] is System.Int32[])
                                {
                                    mas = mas.Union((int[])map[i*n+j]).ToArray();
                                    count.Add(j);
                                }
                            }
                            for (int k = 0; k < count.Count; k++)
                            {
                                int[] res = mas;
                                for (int l = 0; l < n; l++)
                                {
                                    if (map[i*n+l] is System.Int32[] && l != count[k])
                                        res = res.Except((int[])map[i*n+l]).ToArray();
                                }
                                if (res.Length != 0)
                                {
                                    int num = res[0];
                                    map.ChangeNum(i * n + count[k], num);
                                    map.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = map.GetRow(i);
                        }
                        if (Compare(oldArray, newArray))
                            goto case 3;
                        #endregion
                        break;
                    case 3:
                        #region ::Голі елементи в стовпцях::
                        //Пошук унікального елемента в стовпці
                        for (int i = 0; i < n; i++)
                        {
                            oldArray[i] = map.GetRow(i);
                        }
                        for (int i = 0; i < n; i++)
                        {
                            int[] mas = new int[0];
                            List<int> count = new List<int>();
                            for (int j = 0; j < n; j++)
                            {
                                if (map[j * n + i] is System.Int32[])
                                {
                                    mas = mas.Union((int[])map[j * n + i]).ToArray();
                                    count.Add(j);
                                }
                            }
                            for (int k = 0; k < count.Count; k++)
                            {
                                int[] res = mas;
                                for (int l = 0; l < n; l++)
                                {
                                    if (map[l * n + i] is System.Int32[] && l != count[k])
                                        res = res.Except((int[])map[l * n + i]).ToArray();
                                }
                                if (res.Length != 0)
                                {
                                    int num = res[0];
                                    map.ChangeNum(count[k] * n + i, num);
                                    map.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = map.GetRow(i);
                        }
                        if (Compare(oldArray, newArray))
                            goto case 4;
                        #endregion
                        break;
                    case 4:
                        #region ::Стек::
                        for (int i = 0; i < n; i++)
                        {
                            oldArray[i] = map.GetRow(i);
                        }
                        //перевірка на пусту комірку
                        bool emptyCell = false;
                        for (int i = 0; i < n * n; i++)
                        {
                            if (map[i] is System.Int32[])
                            {
                                if (((int[])map[i]).Length == 0)
                                {
                                    emptyCell = true;
                                    break;
                                }
                            }
                        }

                        if (emptyCell == false)//якщо пустих немає
                        {
                            for (int i = 0; i < n * n; i++)
                            {
                                if (map[i] is System.Int32[])
                                {
                                    MapForStorage temporaryMap = new MapForStorage(new Map(map), i, 0);
                                    DataBank.Push(temporaryMap);
                                    int num = ((int[])map[i])[0];
                                    map.ChangeNum(i, num);
                                    map.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                                    break;
                                }
                            }
                        }
                        else//якщо є пуста комірка
                        {
                            while(true)
                            {
                                MapForStorage temporaryMap = new MapForStorage(DataBank.Pop());
                                temporaryMap.Digit++;
                                map = new Map(temporaryMap.map);
                                if(temporaryMap.Digit != ((int[])map[temporaryMap.cell]).Length)
                                {
                                    int num = ((int[])map[temporaryMap.cell])[temporaryMap.Digit];
                                    map.ChangeNum(temporaryMap.cell, num);
                                    map.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                                    DataBank.Push(temporaryMap);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = map.GetRow(i);
                        }
                        if (Compare(oldArray, newArray))
                            goto case 5;
                        #endregion
                        break;
                    case 5:
                        #region ::Вихід в разі помилки::
                        //Console.Write("\r\n Error! \r\n");
                        //Console.ReadKey();
                        System.Windows.MessageBox.Show("Error!");
                        Environment.Exit(0);
                        #endregion
                        break;
                }

                //умова виходу з вічного циклу
                check = false;
                for (int i = 0; i < n; i++)
                {
                    HashSet<int> plural = new HashSet<int>(map.GetRow(i).Where(elem => (elem != 0)));
                    if (plural.Count != n)
                    {
                        check = true;
                        break;
                    }
                }


                //for (int i = 0; i < n; i++)
                //{
                //    Console.WriteLine(string.Join(" ", map.GetRow(i)));
                //}
                //Console.Write("\r\n");

                //Print(map);

                //Console.ReadKey();
            }

            int[,] result = new int[n, n];
            for (int i = 0; i < n*n; i++)
            {
                result[i/n, i%n] = (int)map[i];
            }
            return result;
        }
        static private bool Compare(int[][] array1, int[][] array2)
        {
            bool check = true;
            if (array1.Length != array2.Length)
                return false;
            int n = array1.GetLength(0);
            for (int i = 0; i < n * n; i++)
            {
                if (array1[(int)(i / n)][i % n] != array2[(int)(i / n)][i % n])
                {
                    check = false;
                    break;
                }
            }
            return check;
        }
        //static private void Print(Map mas)
        //{
        //    Console.Write("|\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|\r\n");
        //    for (int i = 0; i < mas.Length; i++)
        //    {
        //        for (int j = 0; j < mas.Length; j++)
        //        {
        //            if (mas[i*mas.Length+j].GetType().ToString() == "System.Int32")
        //                Console.WriteLine(mas[i * mas.Length + j].ToString() + " | " + "System.Int32");
        //            else
        //                Console.WriteLine(string.Join(" ", (int[])mas[i * mas.Length + j]) + " | " + "System.Int32[]");
        //        }
        //        Console.Write("\r\n");
        //    }
        //}
    }
}