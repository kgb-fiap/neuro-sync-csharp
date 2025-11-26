using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeuroSync.Domain.Entities;

namespace NeuroSync.Infrastructure.Persistence
{
    public class NeuroSyncDbContext : DbContext
    {
        public NeuroSyncDbContext(DbContextOptions<NeuroSyncDbContext> options) : base(options)
        {
        }

        public DbSet<Empresa> Empresas => Set<Empresa>();
        public DbSet<Filial> Filiais => Set<Filial>();
        public DbSet<Setor> Setores => Set<Setor>();
        public DbSet<Perfil> Perfis => Set<Perfil>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<UsuarioPerfil> UsuariosPerfis => Set<UsuarioPerfil>();
        public DbSet<PreferenciaSensorial> PreferenciasSensoriais => Set<PreferenciaSensorial>();
        public DbSet<ZonaSensorial> ZonasSensoriais => Set<ZonaSensorial>();
        public DbSet<EstacaoTrabalho> EstacoesTrabalho => Set<EstacaoTrabalho>();
        public DbSet<TipoSensor> TiposSensor => Set<TipoSensor>();
        public DbSet<Sensor> Sensores => Set<Sensor>();
        public DbSet<LeituraSensor> LeiturasSensor => Set<LeituraSensor>();
        public DbSet<StatusReserva> StatusReservas => Set<StatusReserva>();
        public DbSet<ReservaEstacao> ReservasEstacao => Set<ReservaEstacao>();
        public DbSet<AvaliacaoEstacao> AvaliacoesEstacao => Set<AvaliacaoEstacao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var boolToCharConverter = new ValueConverter<bool, string>(
                v => v ? "S" : "N",
                v => v == "S");

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.ToTable("EMPRESA");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID_EMPRESA");
                entity.Property(e => e.RazaoSocial).HasColumnName("RAZAO_SOCIAL").HasMaxLength(150).IsRequired();
                entity.Property(e => e.NomeFantasia).HasColumnName("NOME_FANTASIA").HasMaxLength(150);
                entity.Property(e => e.Cnpj).HasColumnName("CNPJ").HasMaxLength(20).IsRequired();
                entity.Property(e => e.EmailCorporativo).HasColumnName("EMAIL_CORPORATIVO").HasMaxLength(150);
                entity.Property(e => e.TelefoneCorporativo).HasColumnName("TELEFONE_CORPORATIVO").HasMaxLength(20);
                entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasIndex(e => e.Cnpj).IsUnique();
                entity.HasMany(e => e.Filiais).WithOne(f => f.Empresa).HasForeignKey(f => f.EmpresaId);
            });

            modelBuilder.Entity<Filial>(entity =>
            {
                entity.ToTable("FILIAL");
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).HasColumnName("ID_FILIAL");
                entity.Property(f => f.EmpresaId).HasColumnName("ID_EMPRESA");
                entity.Property(f => f.NomeFilial).HasColumnName("NOME_FILIAL").HasMaxLength(150).IsRequired();
                entity.Property(f => f.CodigoFilial).HasColumnName("CODIGO_FILIAL").HasMaxLength(30);
                entity.Property(f => f.Cidade).HasColumnName("CIDADE").HasMaxLength(100);
                entity.Property(f => f.Uf).HasColumnName("UF").HasMaxLength(2);
                entity.Property(f => f.Pais).HasColumnName("PAIS").HasMaxLength(50);
                entity.Property(f => f.Endereco).HasColumnName("ENDERECO").HasMaxLength(255);
                entity.Property(f => f.DataCadastro).HasColumnName("DATA_CADASTRO").HasDefaultValueSql("SYSDATE");
                entity.Property(f => f.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasOne(f => f.Empresa).WithMany(e => e.Filiais).HasForeignKey(f => f.EmpresaId);
                entity.HasMany(f => f.Setores).WithOne(s => s.Filial).HasForeignKey(s => s.FilialId);
                entity.HasMany(f => f.Zonas).WithOne(z => z.Filial).HasForeignKey(z => z.FilialId);
            });

            modelBuilder.Entity<Setor>(entity =>
            {
                entity.ToTable("SETOR");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("ID_SETOR");
                entity.Property(s => s.FilialId).HasColumnName("ID_FILIAL");
                entity.Property(s => s.NomeSetor).HasColumnName("NOME_SETOR").HasMaxLength(150).IsRequired();
                entity.Property(s => s.CodigoSetor).HasColumnName("CODIGO_SETOR").HasMaxLength(30);
                entity.Property(s => s.Andar).HasColumnName("ANDAR").HasMaxLength(20);
                entity.Property(s => s.Descricao).HasColumnName("DESCRICAO").HasMaxLength(255);
                entity.Property(s => s.DataCadastro).HasColumnName("DATA_CADASTRO").HasDefaultValueSql("SYSDATE");
                entity.Property(s => s.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasOne(s => s.Filial).WithMany(f => f.Setores).HasForeignKey(s => s.FilialId);
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.ToTable("PERFIL");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("ID_PERFIL");
                entity.Property(p => p.NomePerfil).HasColumnName("NOME_PERFIL").HasMaxLength(50).IsRequired();
                entity.Property(p => p.Descricao).HasColumnName("DESCRICAO").HasMaxLength(255);
                entity.Property(p => p.NivelAcesso).HasColumnName("NIVEL_ACESSO");
                entity.Property(p => p.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasIndex(p => p.NomePerfil).IsUnique();
                entity.Ignore(p => p.DataCadastro);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("ID_USUARIO");
                entity.Property(u => u.SetorId).HasColumnName("ID_SETOR");
                entity.Property(u => u.NomeCompleto).HasColumnName("NOME_COMPLETO").HasMaxLength(150).IsRequired();
                entity.Property(u => u.EmailCorporativo).HasColumnName("EMAIL_CORPORATIVO").HasMaxLength(150).IsRequired();
                entity.Property(u => u.MatriculaInterna).HasColumnName("MATRICULA_INTERNA").HasMaxLength(50);
                entity.Property(u => u.Telefone).HasColumnName("TELEFONE").HasMaxLength(20);
                entity.Property(u => u.DataAdmissao).HasColumnName("DATA_ADMISSAO");
                entity.Property(u => u.DataCadastro).HasColumnName("DATA_CADASTRO").HasDefaultValueSql("SYSDATE");
                entity.Property(u => u.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.Property(u => u.FlagNeurodivergente).HasColumnName("FLAG_NEURODIVERGENTE").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(false);
                entity.Property(u => u.ObservacoesSuporte).HasColumnName("OBSERVACOES_SUPORTE").HasMaxLength(500);
                entity.Property(u => u.SenhaHash).HasColumnName("SENHA_HASH").HasMaxLength(255).IsRequired();
                entity.Property(u => u.DataUltimoLogin).HasColumnName("DATA_ULTIMO_LOGIN");
                entity.Property(u => u.QuantidadeTentativasLogin).HasColumnName("QTDE_TENTATIVAS_LOGIN");
                entity.Property(u => u.MudarSenhaProxLogin).HasColumnName("MUDAR_SENHA_PROX_LOGIN").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(false);
                entity.HasIndex(u => u.EmailCorporativo).IsUnique();
                entity.HasOne(u => u.Setor).WithMany(s => s.Usuarios).HasForeignKey(u => u.SetorId);
            });

            modelBuilder.Entity<UsuarioPerfil>(entity =>
            {
                entity.ToTable("USUARIO_PERFIL");
                entity.HasKey(up => up.Id);
                entity.Property(up => up.Id).HasColumnName("ID_USUARIO_PERFIL");
                entity.Property(up => up.UsuarioId).HasColumnName("ID_USUARIO");
                entity.Property(up => up.PerfilId).HasColumnName("ID_PERFIL");
                entity.Property(up => up.DataAtribuicao).HasColumnName("DATA_ATRIBUICAO").HasDefaultValueSql("SYSDATE");
                entity.Property(up => up.UsuarioResponsavel).HasColumnName("USUARIO_RESPONSAVEL").HasMaxLength(150);
                entity.Property(up => up.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasOne(up => up.Usuario).WithMany(u => u.Perfis).HasForeignKey(up => up.UsuarioId);
                entity.HasOne(up => up.Perfil).WithMany(p => p.Usuarios).HasForeignKey(up => up.PerfilId);
                entity.Ignore(up => up.DataCadastro);
            });

            modelBuilder.Entity<PreferenciaSensorial>(entity =>
            {
                entity.ToTable("PREFERENCIA_SENSORIAL");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("ID_PREFERENCIA");
                entity.Property(p => p.UsuarioId).HasColumnName("ID_USUARIO");
                entity.Property(p => p.RuidoMaxDb).HasColumnName("RUIDO_MAX_DB");
                entity.Property(p => p.LuzMinLux).HasColumnName("LUZ_MIN_LUX");
                entity.Property(p => p.LuzMaxLux).HasColumnName("LUZ_MAX_LUX");
                entity.Property(p => p.ToleranciaVisual).HasColumnName("TOLERANCIA_VISUAL");
                entity.Property(p => p.PrefereZona).HasColumnName("PREFERE_ZONA").HasMaxLength(30);
                entity.Property(p => p.ObservacoesPreferencia).HasColumnName("OBSERVACOES_PREFERENCIA").HasMaxLength(500);
                entity.Property(p => p.DataInicioVigencia).HasColumnName("DATA_INICIO_VIGENCIA");
                entity.Property(p => p.DataFimVigencia).HasColumnName("DATA_FIM_VIGENCIA");
                entity.Property(p => p.Ativo).HasColumnName("ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasOne(p => p.Usuario).WithMany(u => u.Preferencias).HasForeignKey(p => p.UsuarioId);
                entity.Ignore(p => p.DataCadastro);
            });

            modelBuilder.Entity<ZonaSensorial>(entity =>
            {
                entity.ToTable("ZONA_SENSORIAL");
                entity.HasKey(z => z.Id);
                entity.Property(z => z.Id).HasColumnName("ID_ZONA");
                entity.Property(z => z.FilialId).HasColumnName("ID_FILIAL");
                entity.Property(z => z.NomeZona).HasColumnName("NOME_ZONA").HasMaxLength(150).IsRequired();
                entity.Property(z => z.TipoZona).HasColumnName("TIPO_ZONA").HasMaxLength(50);
                entity.Property(z => z.Descricao).HasColumnName("DESCRICAO").HasMaxLength(255);
                entity.Property(z => z.RuidoMedioEstimadoDb).HasColumnName("RUIDO_MEDIO_ESTIMADO_DB");
                entity.Property(z => z.LuzMediaEstimadoLux).HasColumnName("LUZ_MEDIA_ESTIMADO_LUX");
                entity.Property(z => z.CaracteristicaVisual).HasColumnName("CARACTERISTICA_VISUAL").HasMaxLength(50);
                entity.Property(z => z.CapacidadeEstimada).HasColumnName("CAPACIDADE_ESTIMADA");
                entity.Property(z => z.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasOne(z => z.Filial).WithMany(f => f.Zonas).HasForeignKey(z => z.FilialId);
                entity.Ignore(z => z.DataCadastro);
            });

            modelBuilder.Entity<EstacaoTrabalho>(entity =>
            {
                entity.ToTable("ESTACAO_TRABALHO");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID_ESTACAO");
                entity.Property(e => e.ZonaSensorialId).HasColumnName("ID_ZONA");
                entity.Property(e => e.CodigoEstacao).HasColumnName("CODIGO_ESTACAO").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(255);
                entity.Property(e => e.PermiteReserva).HasColumnName("PERMITE_RESERVA").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.Property(e => e.PermiteUsoEspontaneo).HasColumnName("PERMITE_USO_ESPONTANEO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.Property(e => e.StatusEstacao).HasColumnName("STATUS_ESTACAO").HasMaxLength(20).HasDefaultValue("ATIVA");
                entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES").HasMaxLength(500);
                entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").HasDefaultValueSql("SYSDATE");
                entity.HasIndex(e => e.CodigoEstacao).IsUnique();
                entity.HasOne(e => e.ZonaSensorial).WithMany(z => z.Estacoes).HasForeignKey(e => e.ZonaSensorialId);
                entity.HasMany(e => e.Sensores).WithOne(s => s.EstacaoTrabalho).HasForeignKey(s => s.EstacaoTrabalhoId);
                entity.HasMany(e => e.Reservas).WithOne(r => r.EstacaoTrabalho).HasForeignKey(r => r.EstacaoTrabalhoId);
            });

            modelBuilder.Entity<TipoSensor>(entity =>
            {
                entity.ToTable("TIPO_SENSOR");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnName("ID_TIPO_SENSOR");
                entity.Property(t => t.NomeTipoSensor).HasColumnName("NOME_TIPO_SENSOR").HasMaxLength(50).IsRequired();
                entity.Property(t => t.UnidadeMedida).HasColumnName("UNIDADE_MEDIDA").HasMaxLength(20);
                entity.Property(t => t.Descricao).HasColumnName("DESCRICAO").HasMaxLength(255);
                entity.Property(t => t.Ativo).HasColumnName("STATUS_ATIVO").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(true);
                entity.HasIndex(t => t.NomeTipoSensor).IsUnique();
                entity.Ignore(t => t.DataCadastro);
            });

            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.ToTable("SENSOR");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("ID_SENSOR");
                entity.Property(s => s.EstacaoTrabalhoId).HasColumnName("ID_ESTACAO");
                entity.Property(s => s.TipoSensorId).HasColumnName("ID_TIPO_SENSOR");
                entity.Property(s => s.IdentificadorHardware).HasColumnName("IDENTIFICADOR_HARDWARE").HasMaxLength(100).IsRequired();
                entity.Property(s => s.DataInstalacao).HasColumnName("DATA_INSTALACAO");
                entity.Property(s => s.DataUltimaManutencao).HasColumnName("DATA_ULTIMA_MANUTENCAO");
                entity.Property(s => s.StatusSensor).HasColumnName("STATUS_SENSOR").HasMaxLength(20).HasDefaultValue("ATIVO");
                entity.Property(s => s.Observacoes).HasColumnName("OBSERVACOES").HasMaxLength(500);
                entity.HasIndex(s => s.IdentificadorHardware).IsUnique();
                entity.HasOne(s => s.EstacaoTrabalho).WithMany(e => e.Sensores).HasForeignKey(s => s.EstacaoTrabalhoId);
                entity.HasOne(s => s.TipoSensor).WithMany(t => t.Sensores).HasForeignKey(s => s.TipoSensorId);
                entity.Ignore(s => s.DataCadastro);
            });

            modelBuilder.Entity<LeituraSensor>(entity =>
            {
                entity.ToTable("LEITURA_SENSOR");
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id).HasColumnName("ID_LEITURA");
                entity.Property(l => l.SensorId).HasColumnName("ID_SENSOR");
                entity.Property(l => l.DataHoraLeitura).HasColumnName("DATA_HORA_LEITURA");
                entity.Property(l => l.ValorMedido).HasColumnName("VALOR_MEDIDO").HasColumnType("NUMBER(10,2)");
                entity.Property(l => l.QualidadeSinal).HasColumnName("QUALIDADE_SINAL");
                entity.Property(l => l.OrigemRegistro).HasColumnName("ORIGEM_REGISTRO").HasMaxLength(20);
                entity.Property(l => l.DataProcessamento).HasColumnName("DATA_PROCESSAMENTO").HasDefaultValueSql("SYSDATE");
                entity.HasOne(l => l.Sensor).WithMany(s => s.Leituras).HasForeignKey(l => l.SensorId);
                entity.Ignore(l => l.DataCadastro);
            });

            modelBuilder.Entity<StatusReserva>(entity =>
            {
                entity.ToTable("STATUS_RESERVA");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("ID_STATUS_RESERVA");
                entity.Property(s => s.CodigoStatus).HasColumnName("CODIGO_STATUS").HasMaxLength(30).IsRequired();
                entity.Property(s => s.Descricao).HasColumnName("DESCRICAO").HasMaxLength(255);
                entity.Property(s => s.Finalizador).HasColumnName("E_FINALIZADOR").HasConversion(boolToCharConverter).HasMaxLength(1).HasDefaultValue(false);
                entity.HasIndex(s => s.CodigoStatus).IsUnique();
                entity.Ignore(s => s.DataCadastro);
            });

            modelBuilder.Entity<ReservaEstacao>(entity =>
            {
                entity.ToTable("RESERVA_ESTACAO");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).HasColumnName("ID_RESERVA");
                entity.Property(r => r.UsuarioId).HasColumnName("ID_USUARIO");
                entity.Property(r => r.EstacaoTrabalhoId).HasColumnName("ID_ESTACAO");
                entity.Property(r => r.StatusReservaId).HasColumnName("ID_STATUS_RESERVA");
                entity.Property(r => r.DataHoraSolicitacao).HasColumnName("DATA_HORA_SOLICITACAO").HasDefaultValueSql("SYSDATE");
                entity.Property(r => r.DataHoraInicioPrevista).HasColumnName("DATA_HORA_INICIO_PREVISTA");
                entity.Property(r => r.DataHoraFimPrevista).HasColumnName("DATA_HORA_FIM_PREVISTA");
                entity.Property(r => r.DataHoraCheckin).HasColumnName("DATA_HORA_CHECKIN");
                entity.Property(r => r.DataHoraCheckout).HasColumnName("DATA_HORA_CHECKOUT");
                entity.Property(r => r.OrigemReserva).HasColumnName("ORIGEM_RESERVA").HasMaxLength(20);
                entity.Property(r => r.MotivoCancelamento).HasColumnName("MOTIVO_CANCELAMENTO").HasMaxLength(255);
                entity.Property(r => r.IndiceConfortoCalculado).HasColumnName("INDICE_CONFORTO_CALCULADO").HasColumnType("NUMBER(5,2)");
                entity.Property(r => r.Observacoes).HasColumnName("OBSERVACOES").HasMaxLength(500);
                entity.HasOne(r => r.Usuario).WithMany(u => u.Reservas).HasForeignKey(r => r.UsuarioId);
                entity.HasOne(r => r.EstacaoTrabalho).WithMany(e => e.Reservas).HasForeignKey(r => r.EstacaoTrabalhoId);
                entity.HasOne(r => r.Status).WithMany(s => s.Reservas).HasForeignKey(r => r.StatusReservaId);
                entity.Ignore(r => r.DataCadastro);
            });

            modelBuilder.Entity<AvaliacaoEstacao>(entity =>
            {
                entity.ToTable("AVALIACAO_ESTACAO");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).HasColumnName("ID_AVALIACAO");
                entity.Property(a => a.ReservaEstacaoId).HasColumnName("ID_RESERVA");
                entity.Property(a => a.NotaConfortoGeral).HasColumnName("NOTA_CONFORTO_GERAL");
                entity.Property(a => a.NotaRuido).HasColumnName("NOTA_RUIDO");
                entity.Property(a => a.NotaLuz).HasColumnName("NOTA_LUZ");
                entity.Property(a => a.NotaEstimuloVisual).HasColumnName("NOTA_ESTIMULO_VISUAL");
                entity.Property(a => a.Comentario).HasColumnName("COMENTARIO").HasMaxLength(500);
                entity.Property(a => a.DataAvaliacao).HasColumnName("DATA_AVALIACAO").HasDefaultValueSql("SYSDATE");
                entity.HasOne(a => a.Reserva).WithOne(r => r.Avaliacao).HasForeignKey<AvaliacaoEstacao>(a => a.ReservaEstacaoId);
                entity.Ignore(a => a.DataCadastro);
            });
        }
    }
}
