﻿// COPYRIGHT 2016 by the Open Rails project.
// 
// This file is part of Open Rails.
// 
// Open Rails is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Open Rails is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Open Rails.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Orts.DataConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var converters = new List<IDataConverter>()
            {
                new TerrainConverter()
            };

            try
            {
                var conversions = GetConversions(args);
                if (conversions.Count == 0)
                {
                    ShowHelp(converters);
                    return;
                }

                foreach (var conversion in conversions)
                {
                    var valid = false;
                    foreach (var converter in converters)
                    {
                        if (converter.DoConversion(conversion))
                        {
                            valid = true;
                            Console.WriteLine(conversion.Input);
                            foreach (var output in conversion.Output)
                            {
                                Console.WriteLine("--> {0}", output);
                            }
                            break;
                        }
                    }

                    if (!valid)
                    {
                        throw new InvalidCommandLineException("No conversion available for " + conversion.Input);
                    }
                }
            }
            catch (FileNotFoundException error)
            {
                Console.WriteLine("Error: File not found: " + error.FileName);
            }
            catch (InvalidCommandLineException error)
            {
                Console.WriteLine("Error: " + error.Message);
                Console.WriteLine();
                ShowHelp(converters);
            }
        }

        static void ShowHelp(List<IDataConverter> converters)
        {
            Console.WriteLine("Open Rails Data Converter utility");
            Console.WriteLine();
            Console.WriteLine("{0} /input INPUT [/output] [OUTPUT [...]]", Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName));
            Console.WriteLine();
            //                "1234567890123456789012345678901234567890123456789012345678901234567890123456789"
            Console.WriteLine("    INPUT         Specifies the file to read.");
            Console.WriteLine("    OUTPUT        Specifies the file to generate.");
            Console.WriteLine();
            Console.WriteLine("  Multiple outputs may be specified for each input.");
            Console.WriteLine();
            Console.WriteLine("  Available file format conversions");
            Console.WriteLine("    Input   Output  Description");
            foreach (var converter in converters)
            {
                converter.ShowConversions();
            }
        }

        static List<DataConversion> GetConversions(string[] args)
        {
            var conversions = new List<DataConversion>();
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLowerInvariant())
                {
                    case "/input":
                        i++;
                        if (i >= args.Length)
                        {
                            throw new InvalidCommandLineException("Expected input filename; got end of input");
                        }
                        if (args[i].StartsWith("/"))
                        {
                            throw new InvalidCommandLineException("Expected input filename; got option " + args[i]);
                        }
                        conversions.Add(new DataConversion(args[i]));
                        break;
                    case "/output":
                        if (conversions.Count == 0)
                        {
                            throw new InvalidCommandLineException("Expected /input; got /output");
                        }
                        if (conversions.Last().Output.Count > 0)
                        {
                            throw new InvalidCommandLineException("Expected output filename or /input; got /output");
                        }
                        i++;
                        if (i >= args.Length)
                        {
                            throw new InvalidCommandLineException("Expected output filename; got end of input");
                        }
                        goto default;
                    default:
                        if (conversions.Count == 0)
                        {
                            throw new InvalidCommandLineException("Expected /input; got " + args[i]);
                        }
                        if (args[i].StartsWith("/"))
                        {
                            throw new InvalidCommandLineException("Expected output filename; got option " + args[i]);
                        }
                        conversions.Last().Output.Add(args[i]);
                        break;
                }
            }
            return conversions;
        }
    }

    [Serializable]
    internal class InvalidCommandLineException : Exception
    {
        public InvalidCommandLineException()
        {
        }

        public InvalidCommandLineException(string message) : base(message)
        {
        }

        public InvalidCommandLineException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCommandLineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class DataConversion
    {
        public string Input { get; private set; }
        public List<string> Output { get; private set; }
        public DataConversion(string input)
        {
            Input = input;
            Output = new List<string>();
        }
        public void SetInput(string input)
        {
            Input = input;
        }
    }

    public interface IDataConverter
    {
        void ShowConversions();
        bool DoConversion(DataConversion conversion);
    }
}
