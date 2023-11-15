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
    }
}
