using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NeuroSync.Domain.Entities;
using NeuroSync.Domain.Repositories;
using NeuroSync.Infrastructure.Persistence;

namespace NeuroSync.Infrastructure.Repositories
{
    public class EstacaoTrabalhoRepository : EfRepository<EstacaoTrabalho>, IEstacaoTrabalhoRepository
    {
        public EstacaoTrabalhoRepository(NeuroSyncDbContext context) : base(context)
        {
        }

        public async Task<decimal> CalcularIndiceConfortoAsync(int estacaoId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
        {
            using var command = Context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "BEGIN :result := FNC_CALC_INDICE_CONFORTO(:p_id_estacao, :p_data_inicio, :p_data_fim); END;";
            command.CommandType = CommandType.Text;

            var resultParam = command.CreateParameter();
            resultParam.ParameterName = "result";
            resultParam.Direction = ParameterDirection.Output;
            resultParam.DbType = DbType.Decimal;
            command.Parameters.Add(resultParam);

            var pId = command.CreateParameter();
            pId.ParameterName = "p_id_estacao";
            pId.Value = estacaoId;
            pId.DbType = DbType.Int32;
            command.Parameters.Add(pId);

            var pInicio = command.CreateParameter();
            pInicio.ParameterName = "p_data_inicio";
            pInicio.Value = inicio;
            pInicio.DbType = DbType.DateTime;
            command.Parameters.Add(pInicio);

            var pFim = command.CreateParameter();
            pFim.ParameterName = "p_data_fim";
            pFim.Value = fim;
            pFim.DbType = DbType.DateTime;
            command.Parameters.Add(pFim);

            if (command.Connection!.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync(cancellationToken);
            }

            await command.ExecuteNonQueryAsync(cancellationToken);

            var valor = resultParam.Value == null || resultParam.Value == DBNull.Value ? 0 : Convert.ToDecimal(resultParam.Value);
            return valor;
        }

        public async Task<decimal> CalcularTaxaOcupacaoAsync(int estacaoId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
        {
            using var command = Context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "BEGIN :result := FNC_TAXA_OCUPACAO_ESTACAO(:p_id_estacao, :p_data_inicio, :p_data_fim); END;";
            command.CommandType = CommandType.Text;

            var resultParam = command.CreateParameter();
            resultParam.ParameterName = "result";
            resultParam.Direction = ParameterDirection.Output;
            resultParam.DbType = DbType.Decimal;
            command.Parameters.Add(resultParam);

            var pId = command.CreateParameter();
            pId.ParameterName = "p_id_estacao";
            pId.Value = estacaoId;
            pId.DbType = DbType.Int32;
            command.Parameters.Add(pId);

            var pInicio = command.CreateParameter();
            pInicio.ParameterName = "p_data_inicio";
            pInicio.Value = inicio;
            pInicio.DbType = DbType.DateTime;
            command.Parameters.Add(pInicio);

            var pFim = command.CreateParameter();
            pFim.ParameterName = "p_data_fim";
            pFim.Value = fim;
            pFim.DbType = DbType.DateTime;
            command.Parameters.Add(pFim);

            if (command.Connection!.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync(cancellationToken);
            }

            await command.ExecuteNonQueryAsync(cancellationToken);

            var valor = resultParam.Value == null || resultParam.Value == DBNull.Value ? 0 : Convert.ToDecimal(resultParam.Value);
            return valor;
        }
    }
}
