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
    public class ReservaEstacaoRepository : EfRepository<ReservaEstacao>, IReservaEstacaoRepository
    {
        public ReservaEstacaoRepository(NeuroSyncDbContext context) : base(context)
        {
        }

        public async Task<bool> ReservaCompativelAsync(int reservaId, CancellationToken cancellationToken = default)
        {
            using var command = Context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "BEGIN :result := FNC_RESERVA_COMPATIVEL(:p_id_reserva); END;";
            command.CommandType = CommandType.Text;

            var resultParam = command.CreateParameter();
            resultParam.ParameterName = "result";
            resultParam.Direction = ParameterDirection.Output;
            resultParam.DbType = DbType.String;
            resultParam.Size = 1;
            command.Parameters.Add(resultParam);

            var pId = command.CreateParameter();
            pId.ParameterName = "p_id_reserva";
            pId.Value = reservaId;
            pId.DbType = DbType.Int32;
            command.Parameters.Add(pId);

            if (command.Connection!.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync(cancellationToken);
            }

            await command.ExecuteNonQueryAsync(cancellationToken);

            var valor = resultParam.Value?.ToString();
            return string.Equals(valor, "S", StringComparison.OrdinalIgnoreCase) || valor == "Y" || valor == "1";
        }
    }
}
