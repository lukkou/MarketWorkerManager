using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using RealtimeIndexImporter.Models;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;

namespace RealtimeIndexImporter.Context
{
	class DatabaseContext : DbContext
	{
		public DatabaseContext() : base("Database") { }

		public DbSet<IndexCalendar> IndexCalendars { get; set; }
	}

	public class MysqlConfiguration : DbConfiguration
	{
		public MysqlConfiguration()
		{
			AddDependencyResolver(new MySqlDependencyResolver());
			SetProviderFactory(MySqlProviderInvariantName.ProviderName, new MySqlClientFactory());
			SetDefaultConnectionFactory(new MySqlConnectionFactory());
			SetMigrationSqlGenerator(MySqlProviderInvariantName.ProviderName, () => new MySqlMigrationSqlGenerator());
			SetProviderServices(MySqlProviderInvariantName.ProviderName, new MySqlProviderServices());
			SetProviderFactoryResolver(new MySqlProviderFactoryResolver());
			SetManifestTokenResolver(new MySqlManifestTokenResolver());
			// __migrationHistory テーブルのデフォルト設定の変更
			SetHistoryContext("MySql.Data.MySqlClient", (connection, defaultSchema) => new MyHistoryContext(connection, defaultSchema));

		}

		public class MyHistoryContext : HistoryContext
		{
			public MyHistoryContext(DbConnection dbConnection, string defaultSchema) : base(dbConnection, defaultSchema)
			{
			}

			protected override void OnModelCreating(DbModelBuilder modelBuilder)
			{
				base.OnModelCreating(modelBuilder);

				// 複合キー(MigrationId, ContextKey)の長さがデフォルトでは大きすぎるので設定を変更する
				// 基底クラスの OnModelCreating(modelBuilder) でデフォルト設定を行っているので、base.OnModelCreating() の後に行うこと
				modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
				modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
			}
		}
	}
}
