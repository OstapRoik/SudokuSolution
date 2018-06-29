using System;
using System.Linq;
using System.Collections.Generic;

namespace MyProjects.Game
{
    public class Field
    {
        //Variables
        private Object[][] array;
        private int ArrayLength;
        private int SqrtArrayLength;

        //Constructors
        public Field(int[,] data)
        {
            if (data.GetLength(0) != data.GetLength(1) || 
                data.GetLength(0) < 9 ||
                !IsInt(Math.Sqrt(data.GetLength(0))))
            {
                new FormatFieldException("Wrong field size! Allowable size 9x9, 16x16, ...");

                ArrayLength = 0;
                SqrtArrayLength = 0;
                array = null;

                return;
            }


            ArrayLength = data.GetLength(0);
            SqrtArrayLength = (int)Math.Sqrt(ArrayLength);

            array = new Object[ArrayLength][];
            for (int i = 0; i < ArrayLength; i++)
            {
                array[i] = new Object[ArrayLength];
                for (int j = 0; j < ArrayLength; j++)
                {
                    if (data[i, j] >= 0 && data[i, j] <= ArrayLength)
                        array[i][j] = data[i, j];
                    else
                    {
                        new ErrorInFieldException("Incorrect the digit in field!");
                        ArrayLength = 0;
                        SqrtArrayLength = 0;
                        array = null;
                        return;
                    }
                }
            }
        }
        public Field(Field field)
        {
            ArrayLength = field.ArrayLength;
            SqrtArrayLength = field.SqrtArrayLength;

            array = new Object[ArrayLength][];
            for (int i = 0; i < ArrayLength; i++)
            {
                array[i] = new Object[ArrayLength];
                for (int j = 0; j < ArrayLength; j++)
                {
                    if(field.array[i][j] is System.Int32[])
                        array[i][j] = ((int[])field.array[i][j]).Clone();
                    else
                        array[i][j] = field.array[i][j];
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

            int[][] patterns = new int[ArrayLength][];
            for (int i = 0; i < ArrayLength; i++)
            {
                patterns[i] = new int[ArrayLength];
            }
            int digit = 0;
            for (int i = 0; i < SqrtArrayLength; i++)
            {
                for (int j = 0; j < SqrtArrayLength; j++)
                {
                    for (int ii = 0 + i * SqrtArrayLength; ii < (i + 1) * SqrtArrayLength; ii++)
                    {
                        for (int jj = 0 + j * SqrtArrayLength; jj < (j + 1) * SqrtArrayLength; jj++)
                        {
                            patterns[ii][jj] = digit;
                            digit++;
                        }
                    }
                }
            }

            int[] workmas = null;

            for (int i = 0; i < ArrayLength; i++)
            {
                if (patterns[i].Contains(CellNumber))
                {
                    workmas = patterns[i];
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
        private bool IsInt(object obj)
        {
            return Convert.ToInt32(obj) == Convert.ToDouble(obj);
        }

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
    public class FieldForStorage
    {
        //Variables
        private Field array;
        private int cellNum;
        private int currDigit;

        //Constructors
        public FieldForStorage(Field field, int cellNum, int currDigit)
        {
            this.array = field;
            this.cellNum = cellNum;
            this.currDigit = currDigit;
        }
        public FieldForStorage(FieldForStorage field)
        {
            array = field.field;
            cellNum = field.cell;
            currDigit = field.Digit;
        }

        //Properties
        public Field field
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