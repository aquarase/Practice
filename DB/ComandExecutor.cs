using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgApp.Models;

namespace RpgApp.Database
{
    internal class CommandExecutor
    {
        private readonly ConnectionManager _connectionManager;
        public CommandExecutor(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public async Task<object> ExecuteScalarAsync(string sql, params SqlParameter[] parameters)
        {
            object result = null;

            await _connectionManager.ExecuteWithConnectionAsync(async (connection) =>
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    result = await command.ExecuteScalarAsync();
                }
            });
            return result;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
        {
            int rowsAffected = 0;

            await _connectionManager.ExecuteWithConnectionAsync(async (connection) =>
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            });

            return rowsAffected;
        }

        public async Task CallStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {
            await _connectionManager.ExecuteWithConnectionAsync(async (connection) =>
            {
                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    await command.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task<Character> GetCharacterByNameAsync(string characterName)
        {
            const string sql = "SELECT CharacterID, CharacterName, CharacterLevel, IsAlive, GuildID FROM Characters WHERE CharacterName = @CharacterName";
            Character character = null;

            await _connectionManager.ExecuteWithConnectionAsync(async (connection) =>
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@CharacterName", SqlDbType.VarChar, 30).Value = characterName;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            character = new Character
                            {
                                CharacterId = reader.GetInt32(reader.GetOrdinal("CharacterID")),
                                CharacterName = reader.GetString(reader.GetOrdinal("CharacterName")),
                                CharacterLevel = reader.GetInt32(reader.GetOrdinal("CharacterLevel")),
                                IsAlive = reader.GetBoolean(reader.GetOrdinal("IsAlive")),

                                GuildId = reader.IsDBNull(reader.GetOrdinal("GuildID"))
                                    ? null
                                    : reader.GetInt32(reader.GetOrdinal("GuildID"))
                            };
                        }
                    }
                }
            });
            return character;
        }

        public async Task<bool> CreateCharacterAsync(int id, string name, int lvl, bool isAlive)
        {
            const string sql = "INSERT INTO Characters (CharacterID, CharacterName, CharacterLevel, IsAlive, GuildID) VALUES (@Id, @Name, @Lvl, @isAlive, NULL)";
            var pID = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            var pName = new SqlParameter("@Name", SqlDbType.VarChar, 30) { Value = name };
            var pLvl = new SqlParameter("@Lvl", SqlDbType.Int) { Value = lvl };
            var pIsAlive = new SqlParameter("@isAlive", SqlDbType.Bit) { Value = isAlive };

            int rows = await ExecuteNonQueryAsync(sql, pID, pName, pLvl, pIsAlive);
            return rows > 0;
        }

        public async Task<bool> AddCharacterLevelsAsync(string name, int levels)
        {
            const string sql = "UPDATE Characters SET CharacterLevel = CharacterLevel + @Levels WHERE CharacterName = @Name";
            var pName = new SqlParameter("@Name", SqlDbType.NVarChar, 50) { Value = name };
            var pLevels = new SqlParameter("@Levels", SqlDbType.Int) { Value = levels };

            int rows = await ExecuteNonQueryAsync(sql, pName, pLevels);
            return rows > 0;
        }

        public async Task<bool> DeleteCharacterByNameAsync(string Name)
        {
            const string sql = "DELETE FROM Characters WHERE CharacterName = @Name";
            int rowsAffected = 0;

            await _connectionManager.ExecuteWithConnectionAsync(async (connection) =>
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            });

            return rowsAffected > 0;
        }
    }
}
