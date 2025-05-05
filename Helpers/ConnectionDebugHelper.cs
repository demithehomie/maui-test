using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using BTGClientManager.Data;
using BTGClientManager.Models;

namespace BTGClientManager.Helpers
{
    public static class ConnectionDebugHelper
    {
        /// <summary>
        /// Testa a conexão com o banco de dados e verifica a estrutura das tabelas
        /// </summary>
        public static async Task<string> DiagnoseConnection(string connectionString)
        {
            var result = new StringBuilder();
            
            try
            {
                result.AppendLine("🔍 INICIANDO DIAGNÓSTICO DE CONEXÃO");
                result.AppendLine($"Timestamp: {DateTime.Now}");
                result.AppendLine($"Connection string: {MaskPassword(connectionString)}");
                
                // Teste 1: Conexão básica com Npgsql
                result.AppendLine("\n1️⃣ Testando conexão básica com PostgreSQL...");
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    result.AppendLine("✅ Conexão básica bem sucedida!");
                    
                    // Exibir versão do PostgreSQL
                    using (var cmd = new NpgsqlCommand("SELECT version();", conn))
                    {
                        var version = await cmd.ExecuteScalarAsync();
                        result.AppendLine($"📊 Versão do PostgreSQL: {version}");
                    }
                    
                    // Teste 2: Verificar se a tabela clients existe
                    result.AppendLine("\n2️⃣ Verificando tabela 'clients'...");
                    using (var cmd = new NpgsqlCommand(
                        "SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'clients');", conn))
                    {
                        var tableExists = (bool)(await cmd.ExecuteScalarAsync() ?? false);
                        if (tableExists)
                        {
                            result.AppendLine("✅ Tabela 'clients' encontrada!");
                            
                            // Verificar estrutura da tabela
                            using (var colCmd = new NpgsqlCommand(
                                "SELECT column_name, data_type FROM information_schema.columns WHERE table_name = 'clients';", conn))
                            {
                                using (var reader = await colCmd.ExecuteReaderAsync())
                                {
                                    result.AppendLine("📋 Estrutura da tabela:");
                                    while (await reader.ReadAsync())
                                    {
                                        result.AppendLine($"   - {reader["column_name"]} ({reader["data_type"]})");
                                    }
                                }
                            }
                            
                            // Contar registros
                            using (var countCmd = new NpgsqlCommand("SELECT COUNT(*) FROM clients;", conn))
                            {
                                var count = await countCmd.ExecuteScalarAsync();
                                result.AppendLine($"📈 Total de registros: {count}");
                            }
                            
                            // Visualizar primeiros registros
                            using (var dataCmd = new NpgsqlCommand("SELECT * FROM clients LIMIT 3;", conn))
                            {
                                using (var reader = await dataCmd.ExecuteReaderAsync())
                                {
                                    if (await reader.ReadAsync())
                                    {
                                        result.AppendLine("👁️ Primeiros registros:");
                                        do
                                        {
                                            var sb = new StringBuilder();
                                            sb.Append("   - ");
                                            for (int i = 0; i < reader.FieldCount; i++)
                                            {
                                                sb.Append($"{reader.GetName(i)}: {reader[i]}, ");
                                            }
                                            result.AppendLine(sb.ToString().TrimEnd(new[] { ',', ' ' }));
                                        } while (await reader.ReadAsync() && reader.HasRows);
                                    }
                                    else
                                    {
                                        result.AppendLine("⚠️ Nenhum registro encontrado na tabela!");
                                    }
                                }
                            }
                        }
                        else
                        {
                            result.AppendLine("❌ Tabela 'clients' NÃO encontrada!");
                            
                            // Listar todas tabelas no schema public
                            result.AppendLine("\n📑 Tabelas existentes no schema public:");
                            using (var tablesCmd = new NpgsqlCommand(
                                "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';", conn))
                            {
                                using (var reader = await tablesCmd.ExecuteReaderAsync())
                                {
                                    if (!await reader.ReadAsync())
                                    {
                                        result.AppendLine("   Nenhuma tabela encontrada!");
                                    }
                                    else
                                    {
                                        do
                                        {
                                            result.AppendLine($"   - {reader["table_name"]}");
                                        } while (await reader.ReadAsync());
                                    }
                                }
                            }
                        }
                    }
                }
                
                // Teste 3: Conexão via Entity Framework
                result.AppendLine("\n3️⃣ Testando conexão via Entity Framework Core...");
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                    optionsBuilder.UseNpgsql(connectionString);
                    
                    using (var context = new AppDbContext(optionsBuilder.Options))
                    {
                        // Verificar se o contexto consegue se conectar
                        var canConnect = await context.Database.CanConnectAsync();
                        result.AppendLine(canConnect 
                            ? "✅ EF Core conectou com sucesso!" 
                            : "❌ EF Core NÃO conseguiu conectar!");
                        
                        // Tentar listar clients
                        try
                        {
                            var clientCount = await context.Clients.CountAsync();
                            result.AppendLine($"📊 EF Core encontrou {clientCount} clientes");
                            
                            if (clientCount > 0)
                            {
                                var firstClients = await context.Clients.Take(2).ToListAsync();
                                result.AppendLine("👁️ Primeiros clientes via EF Core:");
                                foreach (var client in firstClients)
                                {
                                    result.AppendLine($"   - Id: {client.Id}, Nome: {client.Name} {client.Lastname}, Endereço: {client.Address}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.AppendLine($"❌ Erro ao consultar clientes via EF Core: {ex.Message}");
                            if (ex.InnerException != null)
                            {
                                result.AppendLine($"   Detalhes: {ex.InnerException.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AppendLine($"❌ Erro ao criar DbContext: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        result.AppendLine($"   Detalhes: {ex.InnerException.Message}");
                    }
                }
                
                result.AppendLine("\n✅ DIAGNÓSTICO CONCLUÍDO");
            }
            catch (Exception ex)
            {
                result.AppendLine($"\n❌ ERRO DURANTE DIAGNÓSTICO: {ex.Message}");
                if (ex.InnerException != null)
                {
                    result.AppendLine($"Detalhes: {ex.InnerException.Message}");
                }
                result.AppendLine($"Stack trace: {ex.StackTrace}");
            }
            
            return result.ToString();
        }
        
        /// <summary>
        /// Cria a tabela clients se não existir e corrige problemas comuns
        /// </summary>
        public static async Task EnsureTableCreated(string connectionString)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    
                    // Verificar se a tabela existe
                    using (var cmd = new NpgsqlCommand(
                        "SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'clients');", conn))
                    {
                        var scalarResult = await cmd.ExecuteScalarAsync();
                        var tableExists = scalarResult != null && (bool)scalarResult;
                        
                        if (!tableExists)
                        {
                            // Criar a tabela
                            string createTableSql = @"
                                CREATE TABLE clients (
                                    ""Id"" SERIAL PRIMARY KEY,
                                    ""Name"" TEXT NOT NULL,
                                    ""Lastname"" TEXT NOT NULL,
                                    ""Address"" TEXT NOT NULL,
                                    ""Age"" INTEGER NULL
                                );";
                            
                            using (var createCmd = new NpgsqlCommand(createTableSql, conn))
                            {
                                await createCmd.ExecuteNonQueryAsync();
                                Console.WriteLine("Tabela 'clients' criada com sucesso!");
                            }
                        }
                        else
                        {
                            // Verificar se a tabela está com a estrutura correta
                            // Verificar case-sensitivity e estrutura
                            string checkColumnsSql = @"
                                SELECT column_name FROM information_schema.columns 
                                WHERE table_name = 'clients' 
                                AND column_name IN ('Id', 'Name', 'Lastname', 'Address', 'Age');";
                            
                            using (var checkCmd = new NpgsqlCommand(checkColumnsSql, conn))
                            {
                                using (var reader = await checkCmd.ExecuteReaderAsync())
                                {
                                    var columns = new List<string>();
                                    while (await reader.ReadAsync())
                                    {
                                        columns.Add(reader.GetString(0));
                                    }
                                    
                                    // Se estiver faltando alguma coluna com o case correto
                                    if (!columns.Contains("Id") || !columns.Contains("Name") || 
                                        !columns.Contains("Lastname") || !columns.Contains("Address"))
                                    {
                                        Console.WriteLine("Problema de case-sensitivity detectado na tabela!");
                                    }
                                }
                            }
                        }
                    }
                    
                    // Verificar e criar a tabela de migrações do EF
                    using (var cmd = new NpgsqlCommand(
                        "SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = '__EFMigrationsHistory');", conn))
                    {
                        var tableExists = (bool)(await cmd.ExecuteScalarAsync() ?? false);
                        
                        if (!tableExists)
                        {
                            string createMigrationTableSql = @"
                                CREATE TABLE ""__EFMigrationsHistory"" (
                                    ""MigrationId"" character varying(150) NOT NULL,
                                    ""ProductVersion"" character varying(32) NOT NULL,
                                    CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
                                );";
                                
                            using (var createCmd = new NpgsqlCommand(createMigrationTableSql, conn))
                            {
                                await createCmd.ExecuteNonQueryAsync();
                                Console.WriteLine("Tabela '__EFMigrationsHistory' criada com sucesso!");
                            }
                            
                            // Inserir migração inicial
                            string insertMigrationSql = @"
                                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                                VALUES ('20250505000000_InitialCreate', '9.0.0-preview.4.24176.1')
                                ON CONFLICT DO NOTHING;";
                                
                            using (var insertCmd = new NpgsqlCommand(insertMigrationSql, conn))
                            {
                                await insertCmd.ExecuteNonQueryAsync();
                                Console.WriteLine("Migração inicial registrada!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao garantir criação da tabela: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detalhes: {ex.InnerException.Message}");
                }
            }
        }
        
        private static string MaskPassword(string connectionString)
        {
            // Máscara a senha na connection string para exibição segura nos logs
            if (string.IsNullOrEmpty(connectionString))
                return string.Empty;
                
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            if (!string.IsNullOrEmpty(builder.Password))
            {
                builder.Password = "********";
            }
            return builder.ConnectionString;
        }
    }
}