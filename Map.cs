using System;
using System.Linq;
using System.Collections.Generic;

namespace MyProjects
{
    class Map
    {
        //Variables
        private Object[][] array;
        private int ArrayLength;
        private int SqrtArrayLength;

        //Constructors
        public Map()
        {
            ArrayLength = 0;
            SqrtArrayLength = 0;

            array = null;
        }
        public Map(int[,] mas)
        {
            if (mas.GetLength(0) != mas.GetLength(1))
            {
                ArrayLength = 0;
                SqrtArrayLength = 0;

                array = null;
                return;
            }
            ArrayLength = mas.GetLength(0);
            SqrtArrayLength = (int)Math.Sqrt(ArrayLength);

            array = new Object[ArrayLength][];
            for (int i = 0; i < ArrayLength; i++)
            {
                array[i] = new Object[ArrayLength];
                for (int j = 0; j < ArrayLength; j++)
                {
                    array[i][j] = mas[i, j];
                }
            }
        }
        public Map(Map oldMap)
        {
            ArrayLength = oldMap.ArrayLength;
            SqrtArrayLength = oldMap.ArrayLength;

            array = new Object[ArrayLength][];
            for (int i = 0; i < ArrayLength; i++)
            {
                array[i] = new Object[ArrayLength];
                for (int j = 0; j < ArrayLength; j++)
                {
                    if(oldMap.array[i][j] is System.Int32[])
                        array[i][j] = ((int[])oldMap.array[i][j]).Clone();
                    else
                        array[i][j] = oldMap.array[i][j];
                }
            }
        }

        //Methods
        public void CheckType()
        {
            for (int i = 0; i < ArrayLength; i++)
            {
                for (int j = 0; j < ArrayLength; j++)
                {
                    if(array[i][j] is System.Int32[])
                    {
                        if (((int[])array[i][j]).Length == 1)
                        {
                            //array[i][j] = ((int[])array[i][j])[0];
                            int num = ((int[])array[i][j])[0];
                            this.ChangeNum(i * ArrayLength + j, num);
                        }
                    }
                }
            }
        }//int[] -> int
        public void ChangeNum(int CellNumber, int num)
        {
            int x = CellNumber % ArrayLength;
            int y = CellNumber / ArrayLength;
            array[y][x] = (int)num;
            for (int i = CellNumber - x; i < CellNumber - x + ArrayLength; i++)
            {
                if (array[i / ArrayLength][i % ArrayLength] is System.Int32[])
                    array[i / ArrayLength][i % ArrayLength] = ((int[])array[i / ArrayLength][i % ArrayLength]).Where(elem => (elem != num)).ToArray<int>();
            }
            for (int i = CellNumber - (y*ArrayLength), j = 0; j < ArrayLength; i+=ArrayLength, j++)
            {
                if (array[i / ArrayLength][i % ArrayLength] is System.Int32[])
                    array[i / ArrayLength][i % ArrayLength] = ((int[])array[i / ArrayLength][i % ArrayLength]).Where(elem => (elem != num)).ToArray<int>();
            }

            int[] a = new int[] { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
            int[] b = new int[] { 3, 4, 5, 12, 13, 14, 21, 22, 23 };
            int[] c = new int[] { 6, 7, 8, 15, 16, 17, 24, 25, 26 };
            int[] d = new int[] { 27, 28, 29, 36, 37, 38, 45, 46, 47 };
            int[] e = new int[] { 30, 31, 32, 39, 40, 41, 48, 49, 50 };
            int[] f = new int[] { 33, 34, 35, 42, 43, 44, 51, 52, 53 };
            int[] g = new int[] { 54, 55, 56, 63, 64, 65, 72, 73, 74 };
            int[] h = new int[] { 57, 58, 59, 66, 67, 68, 75, 76, 77 };
            int[] k = new int[] { 60, 61, 62, 69, 70, 71, 78, 79, 80 };

            int[] workmas = new int[0];
            List<int[]> aaa = new List<int[]>();
            aaa.Add(a);
            aaa.Add(b);
            aaa.Add(c);
            aaa.Add(d);
            aaa.Add(e);
            aaa.Add(f);
            aaa.Add(g);
            aaa.Add(h);
            aaa.Add(k);

            for (int i = 0; i < ArrayLength; i++)
            {
                if (aaa[i].Contains(CellNumber))
                {
                    workmas = aaa[i];
                    break;
                }
            }
            for (int i = 0; i < ArrayLength; i++)
            {
                if (array[workmas[i] / ArrayLength][workmas[i] % ArrayLength] is System.Int32[])
                    array[workmas[i] / ArrayLength][workmas[i] % ArrayLength] = ((int[])array[workmas[i] / ArrayLength][workmas[i] % ArrayLength]).Where(elem => (elem != num)).ToArray<int>();
            }
        }
        public int[] GetRow(int num)
        {
            if (num >= ArrayLength || num < 0)
                return null;
            int[] res = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {
                if (array[num][i].GetType().ToString() == "System.Int32")
                    res[i] = (int)array[num][i];
                else
                    res[i] = 0;
            }
            return res;
        }//num => [0 ... (ArrayLength - 1)]
        public int[] GetCol(int num)
        {
            if (num >= ArrayLength || num < 0)
                return null;
            int[] res = new int[ArrayLength];
            for (int i = 0; i < ArrayLength; i++)
            {
                if (array[i][num].GetType().ToString() == "System.Int32")
                    res[i] = (int)array[i][num];
                else
                    res[i] = 0;
            }
            return res;
        }//num => [0 ... (ArrayLength - 1)]
        public int[] GetCube(int num)
        {
            if (num >= ArrayLength || num < 0)
                return null;
            int[] result = new int[ArrayLength];
            int SqrtLevelSize = (int)Math.Sqrt(ArrayLength);
            int SquareNumber = num + 1;
            int TempNumber = (int)(num / SqrtLevelSize);
            for (int i = TempNumber * SqrtLevelSize, k = 0; i < (TempNumber + 1) * SqrtLevelSize; i++)
            {
                for (int j = SqrtLevelSize * (SquareNumber - TempNumber * SqrtLevelSize) - SqrtLevelSize; j < SqrtLevelSize * (SquareNumber - TempNumber * SqrtLevelSize); j++, k++)
                {
                    if (array[i][j].GetType().ToString() == "System.Int32")
                        result[k] = (int)array[i][j];
                    else
                        result[k] = 0;
                }
            }
            return result;
        }//num => [0 ... (ArrayLength - 1)]

        //Properties
        public int Length
        {
            get
            {
                return ArrayLength;
            }
        }

        //Overloaded methods
        public Object this[int index]
        {
            set
            {
                array[index / ArrayLength][index % ArrayLength] = value;
            }
            get
            {
                return array[index / ArrayLength][index % ArrayLength];
            }
        }
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < ArrayLength; i++)
            {
                for (int j = 0; j < ArrayLength; j++)
                {
                    if (array[i][j] is System.Int32)
                        result += (int)array[i][j] + " ";
                    else
                        result += 0 + " ";
                }
                result += "\r\n";
            }
            return result;
        }
    }
    class MapForStorage
    {
        //Variables
        private Map array;
        private int cellNum;
        private int currDigit;

        //Constructors
        public MapForStorage(Map _map, int _cellNum, int _currDigit)
        {
            array = _map;
            cellNum = _cellNum;
            currDigit = _currDigit;
        }
        public MapForStorage(MapForStorage map)
        {
            array = map.map;
            cellNum = map.cell;
            currDigit = map.Digit;
        }

        //Properties
        public Map map
        {
            get
            {
                return array;
            }
        }
        public int cell
        {
            get
            {
                return cellNum;
            }
        }
        public int Digit
        {
            get
            {
                return currDigit;
            }
            set
            {
                currDigit = value;
            }
        }
    }
}