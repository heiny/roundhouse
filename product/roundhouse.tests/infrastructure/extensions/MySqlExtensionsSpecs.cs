namespace roundhouse.tests.infrastructure.extensions
{
    using System;
    using bdddoc.core;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using roundhouse.databases.mysql;

    public class MySqlExtensionsSpecs
    {
        [Concern(typeof(MySqlExtensions))]
        public class when_the_mysql_extensions_finds_the_start_delimiter : observations_for_a_static_sut
        {
            static Tuple<int, int> result;

            private because b = () => result = MySqlExtensions.find_delimiter(@"DELIMITER $$
                SOME SQL STATEMENT$$
                DELIMITER ;", 0);

            [Observation]
            public void should_return_the_start_delimiter_coordinates()
            {
                result.should_not_be_null();
                result.Item1.should_be_equal_to(0);
                result.Item2.should_be_equal_to(12);
            }
        }

        [Concern(typeof(MySqlExtensions))]
        public class when_the_mysql_extensions_finds_the_end_delimiter : observations_for_a_static_sut
        {
            static Tuple<int, int> result;

            private because b = () => result = MySqlExtensions.find_delimiter(@"DELIMITER $$
                SOME SQL STATEMENT$$
                DELIMITER ;", 13);

            [Observation]
            public void should_return_the_end_delimiter_coordinates()
            {
                result.should_not_be_null();
                result.Item1.should_be_equal_to(68);
                result.Item2.should_be_equal_to(78);
            }
        }
    }
}
