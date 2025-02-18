﻿using Dapper;
using ProjectPratice.Repository.Dtos.Condition;
using ProjectPratice.Repository.Dtos.DataModel;
using ProjectPratice.Repository.Interface;
using System.Data.SqlClient;

namespace ProjectPratice.Repository.Implement
{
    public class CardRepository : ICardRepository
    {

        private readonly string _connectString;
        public CardRepository(string connectString) { 
            this._connectString = connectString;
        }


        /// <summary>
        /// 查詢卡片列表
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IEnumerable<CardDataModel> GetList(CardSearchCondition condition)
        {
            var sql = "SELECT * FROM Card";

            var sqlQuery = new List<string>();
            var parameter = new DynamicParameters();

            if (condition.MinCost.HasValue)
            {
                sqlQuery.Add($" Cost >= @MinCost ");
                parameter.Add("MinCost", condition.MinCost);
            }

            if (condition.MaxCost.HasValue)
            {
                sqlQuery.Add($" Cost <= @MaxCost ");
                parameter.Add("MaxCost", condition.MaxCost);
            }

            if (condition.MinAttack.HasValue)
            {
                sqlQuery.Add($" Attack >= @MinAttack ");
                parameter.Add("MinAttack", condition.MinAttack);
            }

            if (condition.MaxAttack.HasValue)
            {
                sqlQuery.Add($" Attack <= @MaxAttack ");
                parameter.Add("MaxAttack", condition.MaxAttack);
            }

            if (condition.MinHealth.HasValue)
            {
                sqlQuery.Add($" Health >= @MinHealth ");
                parameter.Add("MinHealth", condition.MinHealth);
            }

            if (condition.MaxHealth.HasValue)
            {
                sqlQuery.Add($" Health <= @MaxHealth ");
                parameter.Add("MaxHealth", condition.MaxHealth);
            }

            if (string.IsNullOrWhiteSpace(condition.Name) is false)
            {
                sqlQuery.Add($" Name LIKE @Name ");
                parameter.Add("Name", $"%{condition.MaxHealth}%");
            }

            if (sqlQuery.Any())
            {
                sql += $" WHERE {string.Join(" AND ", sqlQuery)} ";
            }

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.Query<CardDataModel>(sql, parameter);
                return result;
            }
        }

        /// <summary>
        /// 查詢卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <returns></returns>
        public CardDataModel Get(int id)
        {
            var sql =
            @"		
                SELECT * 
                FROM Card 
                Where Id = @id
            ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, System.Data.DbType.Int32);

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.QueryFirstOrDefault<CardDataModel>(sql, parameters);
                return result;
            }
        }


        public bool Insert(CardCondition condition)
        {
            var sql =
                @"
                INSERT INTO Card 
                (
                   [Name]
                  ,[Description]
                  ,[Attack]
                  ,[Health]
                  ,[Cost]
                ) 
                VALUES 
                (
                    @Name
                   ,@Description
                   ,@Attack
                   ,@Health
                   ,@Cost
                );

                SELECT @@IDENTITY;
            ";

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.Execute(sql, condition);
                return result > 0;
            }
        }

        /// <summary>
        /// 更新卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool Update(int id, CardCondition condition)
        {
            var sql =
                @"
                UPDATE Card
                SET 
                    [Name] = @Name
                   ,[Description] = @Description
                   ,[Attack] = @Attack
                   ,[Health] = @Health
                   ,[Cost] = @Cost
                WHERE
                    Id = @id
            ";

            var parameters = new DynamicParameters();

            parameters.AddDynamicParams(condition);
            parameters.Add("Id", id, System.Data.DbType.Int32);

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.Execute(sql, parameters);
                return result > 0;
            }
        }

        /// <summary>
        /// 刪除卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var sql =
                @"
                DELETE FROM Card
                WHERE Id = @Id
            ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, System.Data.DbType.Int32);

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.Execute(sql, parameters);
                return result > 0;
            }
        }
    }
}
