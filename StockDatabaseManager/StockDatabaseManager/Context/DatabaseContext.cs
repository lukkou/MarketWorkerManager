using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using StockDatabaseManager.Models;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;

namespace StockDatabaseManager.Context
{

	class DatabaseContext : DbContext
	{
		public DatabaseContext() : base("StockDB") { }

		#region 共通
		public DbSet<ExecutionJob> ExecutionJobs { get;set;}
		#endregion

		#region 株
		public DbSet<StockMaster> StockMasters { get; set; }
		public DbSet<ClassMaster> ClassMaster { get; set; }
		public DbSet<IndustryCode17Master> IndustryCode17Master { get; set; }
		public DbSet<IndustryCode33Master> IndustryCode33Master { get; set; }
		public DbSet<CandleStick> CandleStick { get; set; }
		public DbSet<OldStockMaster> OldStockMaster { get; set; }
		public DbSet<OldCandleStick> OldCandleStick { get; set; }
		#endregion

		#region FX
		public DbSet<IndexCalendar> IndexCalendar { get; set; }
		#endregion
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
