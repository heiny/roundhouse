using System;
using MySql.Data.MySqlClient;

namespace roundhouse.databases.mysql
{
    public static class MySqlExtensions
    {
        public static string apply_delimiter_and_remove_if_specified(this MySqlScript script)
        {
            string query = script.Query;
            Tuple<int, int> start_delimiter = find_delimiter(query, 0);
            if (start_delimiter == null) return null;

            Tuple<int, int> end_delimiter = find_delimiter(query, start_delimiter.Item2);
            if (end_delimiter == null) return null;

            string delimiter_statement = query.Substring(start_delimiter.Item1, (start_delimiter.Item2 - start_delimiter.Item1) + 1);
            string end_delimiter_statement = query.Substring(end_delimiter.Item1, (end_delimiter.Item2 - end_delimiter.Item1) + 1);

            script.Query = query.Replace(delimiter_statement, string.Empty)
                                .Replace(end_delimiter_statement, string.Empty);

            script.Delimiter = delimiter_statement.Substring(10, delimiter_statement.Length - 11);
            return end_delimiter_statement.Substring(10, end_delimiter_statement.Length - 11);
        }

        // TODO: Regex is prob a better choice because there are so many special cases here - BGH
        public static Tuple<int, int> find_delimiter(string query, int start_index)
        {
            if (string.IsNullOrEmpty(query)) return null;
            if (start_index < 0) start_index = 0;

            query = query.ToLower();
            int start_delimiter_index = query.IndexOf("delimiter", start_index);
            if (start_delimiter_index >= start_index && start_delimiter_index < query.Length)
            {
                int end_delimiter_index = query.IndexOf(Environment.NewLine, start_delimiter_index);
                if (end_delimiter_index < 0)
                {
                    // note: either not found OR special case where normal delimiter is last char
                    end_delimiter_index = query.IndexOf(";", start_delimiter_index);
                }

                if (end_delimiter_index >= 0 && end_delimiter_index < query.Length)
                {
                    return new Tuple<int, int>(start_delimiter_index, end_delimiter_index);
                }
            }
            return null;
        }
    }
}
