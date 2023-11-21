using System;

namespace TrelloClone.Models.Term
{
    public static class Term
    {
        public static int GetQuarter(DateTime dateTime)
        {
            if (dateTime.Month == 1 || dateTime.Month == 2 || dateTime.Month == 3)
            {
                return 1;
            }

            else if (dateTime.Month == 4 || dateTime.Month == 5 || dateTime.Month == 6)
            {
                return 2;
            }

            else if (dateTime.Month == 7 || dateTime.Month == 8 || dateTime.Month == 9)
            {
                return 3;
            }

            else if (dateTime.Month == 10 || dateTime.Month == 11 || dateTime.Month == 12)
            {
                return 4;
            }

            return 0;
        }

        public static int GetPreviousQuarter(DateTime dateTime)
        {
            if (dateTime.Month == 1 || dateTime.Month == 2 || dateTime.Month == 3)
            {
                return 4;
            }

            else if (dateTime.Month == 4 || dateTime.Month == 5 || dateTime.Month == 6)
            {
                return 1;
            }

            else if (dateTime.Month == 7 || dateTime.Month == 8 || dateTime.Month == 9)
            {
                return 2;
            }

            else if (dateTime.Month == 10 || dateTime.Month == 11 || dateTime.Month == 12)
            {
                return 3;
            }

            return 0;
        }

        public static int GetNextQuarter(DateTime dateTime)
        {
            if (dateTime.Month == 1 || dateTime.Month == 2 || dateTime.Month == 3)
            {
                return 2;
            }

            else if (dateTime.Month == 4 || dateTime.Month == 5 || dateTime.Month == 6)
            {
                return 3;
            }

            else if (dateTime.Month == 7 || dateTime.Month == 8 || dateTime.Month == 9)
            {
                return 4;
            }

            else if (dateTime.Month == 10 || dateTime.Month == 11 || dateTime.Month == 12)
            {
                return 1;
            }

            return 0;
        }

        //год учитывается
        public static DateTime GetMin(int quarter, DateTime dateTime)
        {
            if (quarter == 1)
            {
                return new DateTime(dateTime.Year, 1, 1);
            }

            else if (quarter == 2)
            {
                return new DateTime(dateTime.Year, 4, 1);
            }

            else if (quarter == 3)
            {
                return new DateTime(dateTime.Year, 7, 1);
            }

            else if (quarter == 4)
            {
                return new DateTime(dateTime.Year, 10, 1);
            }

            return new DateTime();
        }
        public static DateTime GetMax(int quarter, DateTime dateTime)
        {
            if (quarter == 1)
            {
                if (dateTime.Month == 3 && dateTime.Day >= 20)
                {
                    return new DateTime(dateTime.Year, 6, DateTime.DaysInMonth(dateTime.Year, 6));
                }

                return new DateTime(dateTime.Year, 3, DateTime.DaysInMonth(dateTime.Year, 3));
            }

            else if (quarter == 2)
            {
                if (dateTime.Month == 6 && dateTime.Day >= 20)
                {
                    return new DateTime(dateTime.Year, 9, DateTime.DaysInMonth(dateTime.Year, 9));
                }

                return new DateTime(dateTime.Year, 6, DateTime.DaysInMonth(dateTime.Year, 6));
            }

            else if (quarter == 3)
            {
                if (dateTime.Month == 9 && dateTime.Day >= 20)
                {
                    return new DateTime(dateTime.Year, 12, DateTime.DaysInMonth(dateTime.Year, 12));
                }

                return new DateTime(dateTime.Year, 9, DateTime.DaysInMonth(dateTime.Year, 9));
            }

            else if (quarter == 4)
            {
                if(dateTime.Month == 12 && dateTime.Day >= 20)
                {
                    return new DateTime(dateTime.Year + 1, 3, DateTime.DaysInMonth(dateTime.Year + 1, 3));
                }

                else
                {
                    return new DateTime(dateTime.Year, 12, DateTime.DaysInMonth(dateTime.Year, 12));
                }             
            }

            return new DateTime();
        }
    }
}
