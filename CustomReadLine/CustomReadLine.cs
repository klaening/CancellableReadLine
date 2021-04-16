using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomReadLine
{
    public static class CustomReadLine
    {
        //It ain't pretty but it seems to do the trick

        public static string CancelableReadLine(out bool isEsc)
        {
            var builder = new StringBuilder();
            var cki = Console.ReadKey(true);
            int index = 0;
            Tuple<int, int> startPosition;

            while (cki.Key != ConsoleKey.Enter && cki.Key != ConsoleKey.Escape)
            {
                if (cki.Key == ConsoleKey.LeftArrow)
                {
                    if (index < 1)
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    LeftArrow(ref index, cki);
                }
                else if (cki.Key == ConsoleKey.RightArrow)
                {
                    if (index >= builder.Length)
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    RightArrow(ref index, cki, builder);
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    if (index < 1)
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    BackSpace(ref index, cki, builder);
                }
                else
                {
                    if (cki.KeyChar == '\0')
                    {
                        cki = Console.ReadKey(true);
                        continue;
                    }

                    Default(ref index, cki, builder);
                }

                cki = Console.ReadKey(true);
            }

            if (cki.Key == ConsoleKey.Escape)
            {
                startPosition = GetStartPosition(index);
                ErasePrint(builder, startPosition);

                isEsc = true;
                return string.Empty;
            }

            isEsc = false;

            startPosition = GetStartPosition(index);
            var endPosition = GetEndPosition(startPosition.Item2, builder.Length);
            var left = 0;
            var top = startPosition.Item1 + endPosition.Item1 + 1;

            Console.SetCursorPosition(left, top);

            var value = builder.ToString();
            return value;
        }

        private static void LeftArrow(ref int index, ConsoleKeyInfo cki)
        {
            var previousIndex = index;
            index--;

            if (cki.Modifiers == ConsoleModifiers.Control)
            {
                index = 0;

                var startPosition = GetStartPosition(previousIndex);
                Console.SetCursorPosition(startPosition.Item2, startPosition.Item1);

                return;
            }

            if (Console.CursorLeft > 0)
                Console.CursorLeft--;
            else
            {
                Console.CursorTop--;
                Console.CursorLeft = Console.BufferWidth - 1;
            }
        }

        private static void RightArrow(ref int index, ConsoleKeyInfo cki, StringBuilder builder)
        {
            var previousIndex = index;
            index++;

            if (cki.Modifiers == ConsoleModifiers.Control)
            {
                index = builder.Length;

                var startPosition = GetStartPosition(previousIndex);
                var endPosition = GetEndPosition(startPosition.Item2, builder.Length);
                var top = startPosition.Item1 + endPosition.Item1;
                var left = endPosition.Item2;

                Console.SetCursorPosition(left, top);

                return;
            }

            if (Console.CursorLeft < Console.BufferWidth - 1)
                Console.CursorLeft++;
            else
            {
                Console.CursorTop++;
                Console.CursorLeft = 0;
            }
        }

        private static void BackSpace(ref int index, ConsoleKeyInfo cki, StringBuilder builder)
        {
            var previousIndex = index;
            index--;

            var startPosition = GetStartPosition(previousIndex);
            ErasePrint(builder, startPosition);

            if (cki.Modifiers == ConsoleModifiers.Control)
            {
                builder.Remove(0, previousIndex);
                index = 0;
                Console.Write(builder.ToString());

                Console.SetCursorPosition(startPosition.Item2, startPosition.Item1);

                return;
            }

            builder.Remove(index, 1);
            Console.Write(builder.ToString());

            GoBackToCurrentPosition(index, startPosition);
        }

        private static void Default(ref int index, ConsoleKeyInfo cki, StringBuilder builder)
        {
            var previousIndex = index;
            index++;

            builder.Insert(previousIndex, cki.KeyChar);

            var startPosition = GetStartPosition(previousIndex);
            Console.SetCursorPosition(startPosition.Item2, startPosition.Item1);

            Console.Write(builder.ToString());

            GoBackToCurrentPosition(index, startPosition);
        }

        private static Tuple<int, int> GetStartPosition(int previousIndex)
        {
            //Rename to top and left
            int top;
            int left;

            if (previousIndex <= Console.CursorLeft)
            {
                top = Console.CursorTop;
                left = Console.CursorLeft - previousIndex;
            }
            else
            {
                var decrementValue = previousIndex - Console.CursorLeft;
                var rowsFromStart = decrementValue / Console.BufferWidth;
                top = Console.CursorTop - rowsFromStart;
                left = decrementValue - rowsFromStart * Console.BufferWidth;

                if (left != 0)
                {
                    top--;
                    left = Console.BufferWidth - left;
                }
            }

            return new Tuple<int, int>(top, left);
        }

        private static void GoBackToCurrentPosition(int index, Tuple<int, int> startPosition)
        {
            var rowsToGo = (index + startPosition.Item2) / Console.BufferWidth;
            var rowIndex = index - rowsToGo * Console.BufferWidth;

            var left = startPosition.Item2 + rowIndex;
            var top = startPosition.Item1 + rowsToGo;

            Console.SetCursorPosition(left, top);
        }

        private static Tuple<int, int> GetEndPosition(int startColumn, int builderLength)
        {
            var cursorTop = (builderLength + startColumn) / Console.BufferWidth;
            var cursorLeft = startColumn + (builderLength - cursorTop * Console.BufferWidth);

            return new Tuple<int, int>(cursorTop, cursorLeft);
        }

        private static void ErasePrint(StringBuilder builder, Tuple<int, int> startPosition)
        {
            Console.SetCursorPosition(startPosition.Item2, startPosition.Item1);
            Console.Write(new string(Enumerable.Range(0, builder.Length).Select(o => ' ').ToArray()));

            Console.SetCursorPosition(startPosition.Item2, startPosition.Item1);
        }
    }
}
