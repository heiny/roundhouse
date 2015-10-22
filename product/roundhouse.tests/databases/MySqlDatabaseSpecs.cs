using roundhouse.infrastructure.logging.custom;

namespace roundhouse.tests.databases
{
    using bdddoc.core;
    using consoles;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using log4net;
    using roundhouse.databases;
    using roundhouse.databases.mysql;
    using roundhouse.infrastructure.app;

    public class MySqlDatabaseSpecs
    {
        public abstract class concern_for_MySqlDatabase : observations_for_a_sut_with_a_contract<Database, MySqlDatabase>
        {
            protected static ConfigurationPropertyHolder configuration_property_holder;

            private context c = () =>
            {
                configuration_property_holder = new DefaultConfiguration
                {
                    Logger = new Log4NetLogFactory().create_logger_bound_to(typeof(MySqlDatabaseSpecs))
                };
            };
        }

        [Concern(typeof (MySqlDatabase))]
        public class when_initializing_a_connection_to_a_mysql_database_without_a_connection_string_provided : concern_for_MySqlDatabase
        {
            private because b = () =>
                                    {
                                        sut.connection_string = "";
                                        sut.database_name = "bob";
                                        sut.server_name = "localhost";
                                        sut.initialize_connections(configuration_property_holder);
                                    };

            [Observation]
            public void should_have_the_original_database_as_the_database_to_connect_to()
            {
                sut.connection_string.should_contain("bob");
            }

            [Observation]
            public void should_have_localhost_as_the_server_to_connect_to()
            {
                sut.connection_string.should_contain("localhost");
            }

            [Observation]
            public void should_have_information_schema_as_the_admin_database_to_connect_to()
            {
                sut.connection_string.should_contain("information_schema");
            }

            [Observation]
            public void should_have_3306_as_the_port_to_connect_to()
            {
                sut.connection_string.should_contain("Port=3306");
            }

            [Observation]
            public void should_allow_user_variables_when_a_connection_string_is_not_provided()
            {
                sut.connection_string.should_contain("AllowUserVariables=true");
            }
        }

        [Concern(typeof(MySqlDatabase))]
        public class when_initializing_a_connection_to_a_mysql_database_with_a_connection_string_provided : concern_for_MySqlDatabase
        {
            private because b = () =>
            {
                sut.connection_string = "Server=127.0.0.1;Port=3307;initial catalog=marley;uid=dude;pwd=123";
                sut.database_name = "bob";
                sut.server_name = "(local)";
                sut.initialize_connections(configuration_property_holder);
            };

            [Observation]
            public void should_have_the_connection_string_database_as_the_database_to_connect_to()
            {
                sut.connection_string.should_contain("marley");
            }

            [Observation]
            public void should_have_information_schema_as_the_admin_database_to_connect_to()
            {
                sut.admin_connection_string.should_contain("information_schema");
            }

            [Observation]
            public void should_have_localhost_address_as_the_server_to_connect_to()
            {
                sut.connection_string.should_contain("127.0.0.1");
            }

            [Observation]
            public void should_use_connection_string_user_name_when_provided_in_connection_string()
            {
                sut.connection_string.should_contain("uid=dude");
            }

            [Observation]
            public void should_use_connection_string_password_when_provided_in_connection_string()
            {
                sut.connection_string.should_contain("pwd=123");
            }

            [Observation]
            public void should_use_connection_string_port_when_provided_in_connection_string()
            {
                sut.connection_string.should_contain("Port=3307");
            }
        }
    }
}
