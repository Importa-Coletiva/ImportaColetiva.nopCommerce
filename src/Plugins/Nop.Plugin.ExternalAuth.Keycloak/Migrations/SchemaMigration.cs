using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.ExternalAuth.Keycloak.Domains;

namespace Nop.Plugin.ExternalAuth.Keycloak.Migrations
{
    [NopMigration("7/6/2025 10:58:41 AM", "Nop.Plugin.ExternalAuth.Keycloak schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            Create.TableFor<CustomTable>();
        }
    }
}