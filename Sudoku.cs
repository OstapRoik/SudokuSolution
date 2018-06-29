using System;
using System.Linq;
using System.Collections.Generic;

namespace MyProjects.Game
{
    static class Sudoku
    {
        static public int[,] GetSolution(int[,] data)
        {
            Field field = new Field(data);
            if(field.Length == 0)
                return null;

            int n = field.Length;
            int[] pattern = new int[n];
            for (int i = 0; i < n; i++)
            {
                pattern[i] = i + 1;
            }
            Stack<FieldForStorage> DataBank = new Stack<FieldForStorage>();

            //Заміна відсутніх елементів(0) на масив усіх можливих(по рядку)
            for (int i = 0; i < n*n; i++)
            {
                int[] present = field.GetRow(i/n);
                int[] missing = pattern.Except(present).ToArray();
                if ((int)field[i] == 0)
                {
                    field[i] = missing;
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
                        #region ::Оновлення таблиці можливих значень::
                        for (int i = 0; i < n; i++)
                        {
                            oldArray[i] = field.GetRow(i);
                        }
                        for (int i = 0; i < n*n; i++)
                        {
                            if (field[i] is System.Int32[])
                            {
                                int[] present = field.GetRow(i/n);
                                field[i] = ((int[])field[i]).Except(present).ToArray();//виключення збігів з рядком
                                field.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                            }
                        }
                        for (int i = 0; i < n * n; i++)
                        {
                            if (field[i] is System.Int32[])
                            {
                                int[] present = field.GetCol(i % n);
                                field[i] = ((int[])field[i]).Except(present).ToArray();//виключення збігів з стовпцем
                                field.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                            }
                        }
                        for (int i = 0; i < n * n; i++)
                        {
                            if (field[i] is System.Int32[])
                            {
                                int SqrtN = (int)Math.Sqrt(n);
                                int[] present = field.GetCube((int)((i / n) / SqrtN) * SqrtN + (int)((i % n) / SqrtN));
                                field[i] = ((int[])field[i]).Except(present).ToArray();//виключення збігів з квадратом
                                field.CheckType();//заміна масивів з довжиною 1 на змінну типу int
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = field.GetRow(i);
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
                            oldArray[i] = field.GetRow(i);
                        }
                        for (int i = 0; i < n; i++)
                        {
                            int[] mas = new int[0];
                            List<int> count = new List<int>();
                            for (int j = 0; j < n; j++)
                            {
                                if (field[i*n+j] is System.Int32[])
                                {
                                    mas = mas.Union((int[])field[i*n+j]).ToArray();
                                    count.Add(j);
                                }
                            }
                            for (int k = 0; k < count.Count; k++)
                            {
                                int[] res = mas;
                                for (int l = 0; l < n; l++)
                                {
                                    if (field[i*n+l] is System.Int32[] && l != count[k])
                                        res = res.Except((int[])field[i*n+l]).ToArray();
                                }
                                if (res.Length != 0)
                                {
                                    int num = res[0];
                                    field.ChangeNum(i * n + count[k], num);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = field.GetRow(i);
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
                            oldArray[i] = field.GetRow(i);
                        }
                        for (int i = 0; i < n; i++)
                        {
                            int[] mas = new int[0];
                            List<int> count = new List<int>();
                            for (int j = 0; j < n; j++)
                            {
                                if (field[j * n + i] is System.Int32[])
                                {
                                    mas = mas.Union((int[])field[j * n + i]).ToArray();
                                    count.Add(j);
                                }
                            }
                            for (int k = 0; k < count.Count; k++)
                            {
                                int[] res = mas;
                                for (int l = 0; l < n; l++)
                                {
                                    if (field[l * n + i] is System.Int32[] && l != count[k])
                                        res = res.Except((int[])field[l * n + i]).ToArray();
                                }
                                if (res.Length != 0)
                                {
                                    int num = res[0];
                                    field.ChangeNum(count[k] * n + i, num);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = field.GetRow(i);
                        }
                        if (Compare(oldArray, newArray))
                            goto case 4;
                        #endregion
                        break;
                    case 4:
                        #region ::Стек::
                        for (int i = 0; i < n; i++)
                        {
                            oldArray[i] = field.GetRow(i);
                        }
                        //перевірка на пусту комірку
                        bool emptyCell = false;
                        for (int i = 0; i < n * n; i++)
                        {
                            if (field[i] is System.Int32[])
                            {
                                if (((int[])field[i]).Length == 0)
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
                                if (field[i] is System.Int32[])
                                {
                                    FieldForStorage temporaryMap = new FieldForStorage(new Field(field), i, 0);
                                    DataBank.Push(temporaryMap);
                                    int num = ((int[])field[i])[0];
                                    field.ChangeNum(i, num);
                                    break;
                                }
                            }
                        }
                        else//якщо є пуста комірка
                        {
                            while(true)
                            {
                                FieldForStorage temporaryMap = new FieldForStorage(DataBank.Pop());
                                temporaryMap.Digit++;
                                field = new Field(temporaryMap.field);
                                if(temporaryMap.Digit != ((int[])field[temporaryMap.cell]).Length)
                                {
                                    int num = ((int[])field[temporaryMap.cell])[temporaryMap.Digit];
                                    field.ChangeNum(temporaryMap.cell, num);
                                    DataBank.Push(temporaryMap);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            newArray[i] = field.GetRow(i);
                        }
                        if (Compare(oldArray, newArray))
                            goto case 5;
                        #endregion
                        break;
                    case 5:
                        #region ::Вихід в разі помилки::
                        new ErrorInFieldException("Incorrect the data in field!");
                        return null;
                        #endregion
                }

                //умова виходу з вічного циклу
                check = false;
                for (int i = 0; i < n; i++)
                {
                    int[] tempArray = field.GetRow(i);
                    if(tempArray.Contains(0) || tempArray.Length != n)
                    {
                        check = true;
                        break;
                    }
                }
            }

            int[,] result = new int[n, n];
            for (int i = 0; i < n*n; i++)
            {
                result[i/n, i%n] = (int)field[i];
            }
            return result;
        }
        static private bool Compare(int[][] array1, int[][] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            bool check = true;
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                if(!array1[i].SequenceEqual(array2[i]))
                {
                    check = false;
                    break;
                }
            }
            return check;
        }
    }
}