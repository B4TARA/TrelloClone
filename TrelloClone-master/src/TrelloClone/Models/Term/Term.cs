using System;
using System.Collections.Generic;

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

        public static bool IsEqualQuarter(DateTime dateTime1, DateTime dateTime2)
        {
            if (GetQuarter(dateTime1) == GetQuarter(dateTime2)
                 && dateTime1.Year == dateTime2.Year)
            {
                return true;
            }

            return false;
        }     

        public static List<int> GetQuarterMonths(int quarter)
        {

            switch (quarter)
            {
                case 1: return new List<int>() { 1, 2, 3 };
                case 2: return new List<int>() { 4, 5, 6 };
                case 3: return new List<int>() { 7, 8, 9 };
                case 4: return new List<int>() { 10, 11, 12 };
            }

            return new List<int>() { 1, 2, 3 };
        }

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
                if (dateTime.Month == 12 && dateTime.Day >= 20)
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
        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1: return "Январь";
                case 2: return "Ферваль";
                case 3: return "Март";
                case 4: return "Апрель";
                case 5: return "Май";
                case 6: return "Июнь";
                case 7: return "Июль";
                case 8: return "Август";
                case 9: return "Сентябрь";
                case 10: return "Октябрь";
                case 11: return "Ноябрь";
                case 12: return "Декабрь";
            }

            return "Январь";
        }
        public static DateTime GetFakeDate()
        {
            return new DateTime(2024, 10, 1);
        }
    }
}
