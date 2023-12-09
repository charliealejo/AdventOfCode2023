namespace AdventOfCode2023
{
    internal class FileHelper
    {
        public static IEnumerable<string> ReadLines(string filename)
        {
            return File.ReadAllLines(filename);
        }

        public static IEnumerable<long> ReadLinesAsInt(string filename)
        {
            return ReadLines(filename).Select(long.Parse);
        }

        public static IEnumerable<double> ReadLinesAsDouble(string filename)
        {
            return ReadLines(filename).Select(double.Parse);
        }

        public static IEnumerable<IEnumerable<long>> ReadLinesAsIntLists(string filename, string separator)
        {
            return ReadLines(filename).Select(l => l.Split(separator).Select(long.Parse));
        }

        public static IEnumerable<Tuple<Types>> ReadLinesAsTuple<Types>(string filename)
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                var values = line.Split(' ');
                yield return CreateTuple<Types>(values);
            }
        }

        private static Tuple<Types> CreateTuple<Types>(string[] values)
        {
            var typesArray = typeof(Types).GetGenericArguments();
            var tupleType = typeof(Tuple<>).MakeGenericType(typesArray);

            var tupleValues = new object[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                tupleValues[i] = ConvertToType(values[i], typesArray[i]);
            }

            var tuple = Activator.CreateInstance(tupleType, tupleValues);
            return (Tuple<Types>)tuple;
        }

        public static object ConvertToType(string value, Type targetType)
        {
            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine($"Could not convert '{value}' to {targetType.Name}.");
                throw;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Wrong format for the conversion to {targetType.Name}.");
                throw;
            }
            catch (OverflowException)
            {
                Console.WriteLine($"The value '{value}' is out of range for the type {targetType.Name}.");
                throw;
            }
        }
    }
}
