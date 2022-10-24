using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ExportPages
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringEx
    {

        #region FormatWith

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks>
        /// Original: http://haacked.com/archive/2009/01/04/fun-with-named-formats-string-parsing-and-edge-cases.aspx
        /// </remarks>
        public static string FormatWith(this string format, object source)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            //var formattedStrings = (from expression in SplitFormat(format)
            //                        select expression.Eval(source)).ToArray();
            //return String.Join("", formattedStrings);
            StringBuilder formattedString = new StringBuilder();
            foreach (ITextExpression item in SplitFormat(format))
            {
                formattedString.Append(item.Eval(source));
            }
            return formattedString.ToString();
        }

        /// <summary>
        /// Get member value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks>
        /// Original from: http://stackoverflow.com/questions/1196991/get-property-value-from-string-using-reflection-in-c-sharp
        /// </remarks>
        private static object GetMemberValue(object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            foreach (string part in propertyName.Split('.'))
            {
                if (obj == null)
                    return null;

                Type type = obj.GetType();
                PropertyInfo pInfo = type.GetProperty(part);

                if (pInfo != null)
                    obj = pInfo.GetValue(obj, null);
                else
                {
                    FieldInfo fInfo = type.GetField(part);
                    if (fInfo == null)
                        throw new ArgumentException(String.Format(Properties.Resources.PropertyOrFieldDoesNotExist, propertyName, obj.GetType().Name));
                    obj = fInfo.GetValue(obj);
                }
            }
            return obj;
        }


        private static IEnumerable<ITextExpression> SplitFormat(string format)
        {
            int exprEndIndex = -1;
            int expStartIndex;

            do
            {
                expStartIndex = IndexOfExpressionStart(format, exprEndIndex + 1);
                if (expStartIndex < 0)
                {
                    //everything after last end brace index.
                    if (exprEndIndex + 1 < format.Length)
                    {
                        yield return new LiteralFormat(
                            format.Substring(exprEndIndex + 1));
                    }
                    break;
                }

                if (expStartIndex - exprEndIndex - 1 > 0)
                {
                    //everything up to next start brace index
                    yield return new LiteralFormat(format.Substring(exprEndIndex + 1
                      , expStartIndex - exprEndIndex - 1));
                }

                int endBraceIndex = IndexOfExpressionEnd(format, expStartIndex + 1);
                if (endBraceIndex < 0)
                {
                    //rest of string, no end brace (could be invalid expression)
                    yield return new FormatExpression(format.Substring(expStartIndex));
                }
                else
                {
                    exprEndIndex = endBraceIndex;
                    //everything from start to end brace.
                    yield return new FormatExpression(format.Substring(expStartIndex
                      , endBraceIndex - expStartIndex + 1));

                }
            } while (expStartIndex > -1);
        }

        private static int IndexOfExpressionStart(string format, int startIndex)
        {
            int index = format.IndexOf('{', startIndex);
            if (index == -1)
            {
                return index;
            }

            //peek ahead.
            if (index + 1 < format.Length)
            {
                char nextChar = format[index + 1];
                if (nextChar == '{')
                {
                    return IndexOfExpressionStart(format, index + 2);
                }
            }

            return index;
        }

        private static int IndexOfExpressionEnd(string format, int startIndex)
        {
            int endBraceIndex = format.IndexOf('}', startIndex);
            if (endBraceIndex == -1)
            {
                return endBraceIndex;
            }
            //start peeking ahead until there are no more braces...
            // }}}}
            int braceCount = 0;
            for (int i = endBraceIndex + 1; i < format.Length; i++)
            {
                if (format[i] == '}')
                {
                    braceCount++;
                }
                else
                {
                    break;
                }
            }
            if (braceCount % 2 == 1)
            {
                return IndexOfExpressionEnd(format, endBraceIndex + braceCount + 1);
            }

            return endBraceIndex;
        }

        private class FormatExpression : ITextExpression
        {
            readonly bool _invalidExpression = false;

            public FormatExpression(string expression)
            {
                if (!expression.StartsWith("{") || !expression.EndsWith("}"))
                {
                    _invalidExpression = true;
                    Expression = expression;
                    return;
                }

                string expressionWithoutBraces = expression.Substring(1
                    , expression.Length - 2);
                int colonIndex = expressionWithoutBraces.IndexOf(':');
                int commaIndex = expressionWithoutBraces.IndexOf(',');
                if ((commaIndex < 0) && (colonIndex < 0))
                {
                    Expression = expressionWithoutBraces;
                }
                else
                {
                    int separatorIndex = commaIndex < 0 ? colonIndex : commaIndex;
                    Expression = expressionWithoutBraces.Substring(0, separatorIndex);
                    Format = expressionWithoutBraces.Substring(separatorIndex);
                }
            }

            public string Expression
            {
                get;
                private set;
            }

            public string Format
            {
                get;
                private set;
            }

            public string Eval(object o)
            {
                if (_invalidExpression)
                {
                    throw new FormatException("Invalid expression");
                }
                try
                {
                    if (String.IsNullOrEmpty(Format))
                    {
                        return (GetMemberValue(o, Expression) ?? String.Empty).ToString();
                    }
                    string format = "{0" + Format + "}";
                    return String.Format(format, GetMemberValue(o, Expression));
                }
                catch (ArgumentException ex)
                {
                    throw new FormatException(ex.Message, ex);
                }
            }
        }

        private class LiteralFormat : ITextExpression
        {
            public LiteralFormat(string literalText)
            {
                LiteralText = literalText;
            }

            public string LiteralText
            {
                get;
                private set;
            }

            public string Eval(object o)
            {
                string literalText = LiteralText
                    .Replace("{{", "{")
                    .Replace("}}", "}");
                return literalText;
            }
        }

        private interface ITextExpression
        {
            string Eval(object o);
        }

        #endregion

        /// <summary>
        /// Reverse character order (abcd -> dcba)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Reverse(this string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;
            StringBuilder sb = new StringBuilder();
            for (int i = text.Length - 1; i >= 0; i--)
            {
                sb.Append(text[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get characters from left
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string text, int length)
        {
            if (text == null)
                return null;
            if (length == 0) // Optimalizace
                return String.Empty;
            if (text.Length <= length)
                return text;
            return text.Substring(0, length);
        }

        /// <summary>
        /// Get characters from right
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string text, int length)
        {
            if (text == null)
                return null;
            if (length == 0) // Optimalizace
                return String.Empty;
            if (text.Length <= length)
                return text;
            return text.Substring(text.Length - length);
        }

        /// <summary>
        /// Remove diacricics from string
        /// </summary>
        /// <param name="text">String without diacritics</param>
        /// <returns></returns>
        public static string ToAscii(this string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;
            text = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(text[i]) != System.Globalization.UnicodeCategory.NonSpacingMark) sb.Append(text[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in characters, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        public static IEnumerable<string> WordWrap(this string text, int width)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            int length = text.Length;

            if (length <= width)
                yield return text;

            int pos = 0;
            do
            {
                int wrap = (pos + width >= length) ? length - pos : BreakLine(text, pos, width);
                if (wrap > 0)
                {
                    if (pos + wrap > length)
                        yield return text.Substring(pos);
                    else
                        yield return text.Substring(pos, wrap);
                }
                pos += wrap;

                // Skip whitespace after separator
                if (pos < length && Char.IsWhiteSpace(text[pos]))
                    pos++;

            } while (pos < length);
        }

        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace, separator or punctuation in line
            int i = max;
            while (i >= 0 && !(Char.IsWhiteSpace(text[pos + i]) || Char.IsSeparator(text[pos + i]) || Char.IsPunctuation(text[pos + i])))
                i--;

            // If no break candidate found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before break
            return i + 1;
        }
    }
}
