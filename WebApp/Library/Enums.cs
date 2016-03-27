using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApp.Library.Enums
{
    public class FormModes
    {
        public const byte ReadOnly = 1;
        public const byte Edit = 2;
        public const byte Add = 3;
        public const byte Confirm = 3;
    }

    public static class TaskStatusColorCodes
    {
        public static string Active = Color.Green.ToString();
        public static string Waiting = Color.Blue.ToString();
        public static string Completed = Color.Gray.ToString();
    }

    public static class TaskStatuses
    {
        public const byte Active = 1;
        public const byte Completed = 2;
        public const byte Waiting = 3;
        public const byte Obsolete = 4;
    }
}