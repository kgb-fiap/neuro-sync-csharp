using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NeuroSync.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMPRESA",
                columns: table => new
                {
                    ID_EMPRESA = table.Column<int>(nullable: false),
                    RAZAO_SOCIAL = table.Column<string>(maxLength: 150, nullable: false),
                    NOME_FANTASIA = table.Column<string>(maxLength: 150, nullable: true),
                    CNPJ = table.Column<string>(maxLength: 20, nullable: false),
                    EMAIL_CORPORATIVO = table.Column<string>(maxLength: 150, nullable: true),
                    TELEFONE_CORPORATIVO = table.Column<string>(maxLength: 20, nullable: true),
                    DATA_CADASTRO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE"),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPRESA", x => x.ID_EMPRESA);
                });

            migrationBuilder.CreateTable(
                name: "PERFIL",
                columns: table => new
                {
                    ID_PERFIL = table.Column<int>(nullable: false),
                    NOME_PERFIL = table.Column<string>(maxLength: 50, nullable: false),
                    DESCRICAO = table.Column<string>(maxLength: 255, nullable: true),
                    NIVEL_ACESSO = table.Column<int>(nullable: true),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFIL", x => x.ID_PERFIL);
                });

            migrationBuilder.CreateTable(
                name: "STATUS_RESERVA",
                columns: table => new
                {
                    ID_STATUS_RESERVA = table.Column<int>(nullable: false),
                    CODIGO_STATUS = table.Column<string>(maxLength: 30, nullable: false),
                    DESCRICAO = table.Column<string>(maxLength: 255, nullable: true),
                    E_FINALIZADOR = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "N")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATUS_RESERVA", x => x.ID_STATUS_RESERVA);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_SENSOR",
                columns: table => new
                {
                    ID_TIPO_SENSOR = table.Column<int>(nullable: false),
                    NOME_TIPO_SENSOR = table.Column<string>(maxLength: 50, nullable: false),
                    UNIDADE_MEDIDA = table.Column<string>(maxLength: 20, nullable: true),
                    DESCRICAO = table.Column<string>(maxLength: 255, nullable: true),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_SENSOR", x => x.ID_TIPO_SENSOR);
                });

            migrationBuilder.CreateTable(
                name: "FILIAL",
                columns: table => new
                {
                    ID_FILIAL = table.Column<int>(nullable: false),
                    ID_EMPRESA = table.Column<int>(nullable: false),
                    NOME_FILIAL = table.Column<string>(maxLength: 150, nullable: false),
                    CODIGO_FILIAL = table.Column<string>(maxLength: 30, nullable: true),
                    CIDADE = table.Column<string>(maxLength: 100, nullable: true),
                    UF = table.Column<string>(maxLength: 2, nullable: true),
                    PAIS = table.Column<string>(maxLength: 50, nullable: true),
                    ENDERECO = table.Column<string>(maxLength: 255, nullable: true),
                    DATA_CADASTRO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE"),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILIAL", x => x.ID_FILIAL);
                    table.ForeignKey(
                        name: "FK_FILIAL_EMPRESA",
                        column: x => x.ID_EMPRESA,
                        principalTable: "EMPRESA",
                        principalColumn: "ID_EMPRESA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SETOR",
                columns: table => new
                {
                    ID_SETOR = table.Column<int>(nullable: false),
                    ID_FILIAL = table.Column<int>(nullable: false),
                    NOME_SETOR = table.Column<string>(maxLength: 150, nullable: false),
                    CODIGO_SETOR = table.Column<string>(maxLength: 30, nullable: true),
                    ANDAR = table.Column<string>(maxLength: 20, nullable: true),
                    DESCRICAO = table.Column<string>(maxLength: 255, nullable: true),
                    DATA_CADASTRO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE"),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SETOR", x => x.ID_SETOR);
                    table.ForeignKey(
                        name: "FK_SETOR_FILIAL",
                        column: x => x.ID_FILIAL,
                        principalTable: "FILIAL",
                        principalColumn: "ID_FILIAL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZONA_SENSORIAL",
                columns: table => new
                {
                    ID_ZONA = table.Column<int>(nullable: false),
                    ID_FILIAL = table.Column<int>(nullable: false),
                    NOME_ZONA = table.Column<string>(maxLength: 150, nullable: false),
                    TIPO_ZONA = table.Column<string>(maxLength: 50, nullable: true),
                    DESCRICAO = table.Column<string>(maxLength: 255, nullable: true),
                    RUIDO_MEDIO_ESTIMADO_DB = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                    LUZ_MEDIA_ESTIMADO_LUX = table.Column<decimal>(type: "NUMBER(7,2)", nullable: true),
                    CARACTERISTICA_VISUAL = table.Column<string>(maxLength: 50, nullable: true),
                    CAPACIDADE_ESTIMADA = table.Column<int>(nullable: true),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZONA_SENSORIAL", x => x.ID_ZONA);
                    table.ForeignKey(
                        name: "FK_ZONA_FILIAL",
                        column: x => x.ID_FILIAL,
                        principalTable: "FILIAL",
                        principalColumn: "ID_FILIAL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(nullable: false),
                    ID_SETOR = table.Column<int>(nullable: false),
                    NOME_COMPLETO = table.Column<string>(maxLength: 150, nullable: false),
                    EMAIL_CORPORATIVO = table.Column<string>(maxLength: 150, nullable: false),
                    MATRICULA_INTERNA = table.Column<string>(maxLength: 50, nullable: true),
                    TELEFONE = table.Column<string>(maxLength: 20, nullable: true),
                    DATA_ADMISSAO = table.Column<DateTime>(nullable: true),
                    DATA_CADASTRO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE"),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S"),
                    FLAG_NEURODIVERGENTE = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "N"),
                    OBSERVACOES_SUPORTE = table.Column<string>(maxLength: 500, nullable: true),
                    SENHA_HASH = table.Column<string>(maxLength: 255, nullable: false),
                    DATA_ULTIMO_LOGIN = table.Column<DateTime>(nullable: true),
                    QTDE_TENTATIVAS_LOGIN = table.Column<int>(nullable: false, defaultValue: 0),
                    MUDAR_SENHA_PROX_LOGIN = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "N")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_USUARIO_SETOR",
                        column: x => x.ID_SETOR,
                        principalTable: "SETOR",
                        principalColumn: "ID_SETOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ESTACAO_TRABALHO",
                columns: table => new
                {
                    ID_ESTACAO = table.Column<int>(nullable: false),
                    ID_ZONA = table.Column<int>(nullable: false),
                    CODIGO_ESTACAO = table.Column<string>(maxLength: 50, nullable: false),
                    DESCRICAO = table.Column<string>(maxLength: 255, nullable: true),
                    PERMITE_RESERVA = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S"),
                    PERMITE_USO_ESPONTANEO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S"),
                    STATUS_ESTACAO = table.Column<string>(maxLength: 20, nullable: false, defaultValue: "ATIVA"),
                    OBSERVACOES = table.Column<string>(maxLength: 500, nullable: true),
                    DATA_CADASTRO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTACAO_TRABALHO", x => x.ID_ESTACAO);
                    table.ForeignKey(
                        name: "FK_ESTACAO_ZONA",
                        column: x => x.ID_ZONA,
                        principalTable: "ZONA_SENSORIAL",
                        principalColumn: "ID_ZONA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO_PERFIL",
                columns: table => new
                {
                    ID_USUARIO_PERFIL = table.Column<int>(nullable: false),
                    ID_USUARIO = table.Column<int>(nullable: false),
                    ID_PERFIL = table.Column<int>(nullable: false),
                    DATA_ATRIBUICAO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE"),
                    USUARIO_RESPONSAVEL = table.Column<string>(maxLength: 150, nullable: true),
                    STATUS_ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO_PERFIL", x => x.ID_USUARIO_PERFIL);
                    table.ForeignKey(
                        name: "FK_USUARIO_PERFIL_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USUARIO_PERFIL_PERFIL",
                        column: x => x.ID_PERFIL,
                        principalTable: "PERFIL",
                        principalColumn: "ID_PERFIL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PREFERENCIA_SENSORIAL",
                columns: table => new
                {
                    ID_PREFERENCIA = table.Column<int>(nullable: false),
                    ID_USUARIO = table.Column<int>(nullable: false),
                    RUIDO_MAX_DB = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                    LUZ_MIN_LUX = table.Column<decimal>(type: "NUMBER(7,2)", nullable: true),
                    LUZ_MAX_LUX = table.Column<decimal>(type: "NUMBER(7,2)", nullable: true),
                    TOLERANCIA_VISUAL = table.Column<int>(nullable: true),
                    PREFERE_ZONA = table.Column<string>(maxLength: 30, nullable: true),
                    OBSERVACOES_PREFERENCIA = table.Column<string>(maxLength: 500, nullable: true),
                    DATA_INICIO_VIGENCIA = table.Column<DateTime>(nullable: false),
                    DATA_FIM_VIGENCIA = table.Column<DateTime>(nullable: true),
                    ATIVO = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "S")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PREFERENCIA_SENSORIAL", x => x.ID_PREFERENCIA);
                    table.ForeignKey(
                        name: "FK_PREF_SENSORIAL_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SENSOR",
                columns: table => new
                {
                    ID_SENSOR = table.Column<int>(nullable: false),
                    ID_ESTACAO = table.Column<int>(nullable: false),
                    ID_TIPO_SENSOR = table.Column<int>(nullable: false),
                    IDENTIFICADOR_HARDWARE = table.Column<string>(maxLength: 100, nullable: false),
                    DATA_INSTALACAO = table.Column<DateTime>(nullable: true),
                    DATA_ULTIMA_MANUTENCAO = table.Column<DateTime>(nullable: true),
                    STATUS_SENSOR = table.Column<string>(maxLength: 20, nullable: false, defaultValue: "ATIVO"),
                    OBSERVACOES = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SENSOR", x => x.ID_SENSOR);
                    table.ForeignKey(
                        name: "FK_SENSOR_ESTACAO",
                        column: x => x.ID_ESTACAO,
                        principalTable: "ESTACAO_TRABALHO",
                        principalColumn: "ID_ESTACAO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SENSOR_TIPO_SENSOR",
                        column: x => x.ID_TIPO_SENSOR,
                        principalTable: "TIPO_SENSOR",
                        principalColumn: "ID_TIPO_SENSOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RESERVA_ESTACAO",
                columns: table => new
                {
                    ID_RESERVA = table.Column<int>(nullable: false),
                    ID_USUARIO = table.Column<int>(nullable: false),
                    ID_ESTACAO = table.Column<int>(nullable: false),
                    ID_STATUS_RESERVA = table.Column<int>(nullable: false),
                    DATA_HORA_SOLICITACAO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE"),
                    DATA_HORA_INICIO_PREVISTA = table.Column<DateTime>(nullable: false),
                    DATA_HORA_FIM_PREVISTA = table.Column<DateTime>(nullable: false),
                    DATA_HORA_CHECKIN = table.Column<DateTime>(nullable: true),
                    DATA_HORA_CHECKOUT = table.Column<DateTime>(nullable: true),
                    ORIGEM_RESERVA = table.Column<string>(maxLength: 20, nullable: true),
                    MOTIVO_CANCELAMENTO = table.Column<string>(maxLength: 255, nullable: true),
                    INDICE_CONFORTO_CALCULADO = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                    OBSERVACOES = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVA_ESTACAO", x => x.ID_RESERVA);
                    table.ForeignKey(
                        name: "FK_RESERVA_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESERVA_ESTACAO",
                        column: x => x.ID_ESTACAO,
                        principalTable: "ESTACAO_TRABALHO",
                        principalColumn: "ID_ESTACAO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESERVA_STATUS",
                        column: x => x.ID_STATUS_RESERVA,
                        principalTable: "STATUS_RESERVA",
                        principalColumn: "ID_STATUS_RESERVA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LEITURA_SENSOR",
                columns: table => new
                {
                    ID_LEITURA = table.Column<int>(nullable: false),
                    ID_SENSOR = table.Column<int>(nullable: false),
                    DATA_HORA_LEITURA = table.Column<DateTime>(nullable: false),
                    VALOR_MEDIDO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    QUALIDADE_SINAL = table.Column<int>(nullable: true),
                    ORIGEM_REGISTRO = table.Column<string>(maxLength: 20, nullable: true),
                    DATA_PROCESSAMENTO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEITURA_SENSOR", x => x.ID_LEITURA);
                    table.ForeignKey(
                        name: "FK_LEITURA_SENSOR_SENSOR",
                        column: x => x.ID_SENSOR,
                        principalTable: "SENSOR",
                        principalColumn: "ID_SENSOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AVALIACAO_ESTACAO",
                columns: table => new
                {
                    ID_AVALIACAO = table.Column<int>(nullable: false),
                    ID_RESERVA = table.Column<int>(nullable: false),
                    NOTA_CONFORTO_GERAL = table.Column<int>(nullable: false),
                    NOTA_RUIDO = table.Column<int>(nullable: true),
                    NOTA_LUZ = table.Column<int>(nullable: true),
                    NOTA_ESTIMULO_VISUAL = table.Column<int>(nullable: true),
                    COMENTARIO = table.Column<string>(maxLength: 500, nullable: true),
                    DATA_AVALIACAO = table.Column<DateTime>(nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AVALIACAO_ESTACAO", x => x.ID_AVALIACAO);
                    table.ForeignKey(
                        name: "FK_AVALIACAO_RESERVA",
                        column: x => x.ID_RESERVA,
                        principalTable: "RESERVA_ESTACAO",
                        principalColumn: "ID_RESERVA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FILIAL_ID_EMPRESA",
                table: "FILIAL",
                column: "ID_EMPRESA");

            migrationBuilder.CreateIndex(
                name: "UQ_EMPRESA_CNPJ",
                table: "EMPRESA",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_PERFIL_NOME",
                table: "PERFIL",
                column: "NOME_PERFIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SETOR_ID_FILIAL",
                table: "SETOR",
                column: "ID_FILIAL");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ID_SETOR",
                table: "USUARIO",
                column: "ID_SETOR");

            migrationBuilder.CreateIndex(
                name: "UQ_USUARIO_EMAIL",
                table: "USUARIO",
                column: "EMAIL_CORPORATIVO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_PERFIL_ID_PERFIL",
                table: "USUARIO_PERFIL",
                column: "ID_PERFIL");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_PERFIL_ID_USUARIO",
                table: "USUARIO_PERFIL",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_PREFERENCIA_SENSORIAL_ID_USUARIO",
                table: "PREFERENCIA_SENSORIAL",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_ZONA_SENSORIAL_ID_FILIAL",
                table: "ZONA_SENSORIAL",
                column: "ID_FILIAL");

            migrationBuilder.CreateIndex(
                name: "UQ_ESTACAO_CODIGO",
                table: "ESTACAO_TRABALHO",
                column: "CODIGO_ESTACAO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ESTACAO_TRABALHO_ID_ZONA",
                table: "ESTACAO_TRABALHO",
                column: "ID_ZONA");

            migrationBuilder.CreateIndex(
                name: "UQ_SENSOR_IDENTIFICADOR",
                table: "SENSOR",
                column: "IDENTIFICADOR_HARDWARE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SENSOR_ID_ESTACAO",
                table: "SENSOR",
                column: "ID_ESTACAO");

            migrationBuilder.CreateIndex(
                name: "IX_SENSOR_ID_TIPO_SENSOR",
                table: "SENSOR",
                column: "ID_TIPO_SENSOR");

            migrationBuilder.CreateIndex(
                name: "IX_LEITURA_SENSOR_ID_SENSOR",
                table: "LEITURA_SENSOR",
                column: "ID_SENSOR");

            migrationBuilder.CreateIndex(
                name: "UQ_STATUS_RESERVA_CODIGO",
                table: "STATUS_RESERVA",
                column: "CODIGO_STATUS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RESERVA_ESTACAO_ID_USUARIO",
                table: "RESERVA_ESTACAO",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVA_ESTACAO_ID_ESTACAO",
                table: "RESERVA_ESTACAO",
                column: "ID_ESTACAO");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVA_ESTACAO_ID_STATUS_RESERVA",
                table: "RESERVA_ESTACAO",
                column: "ID_STATUS_RESERVA");

            migrationBuilder.CreateIndex(
                name: "IX_AVALIACAO_ESTACAO_ID_RESERVA",
                table: "AVALIACAO_ESTACAO",
                column: "ID_RESERVA",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AVALIACAO_ESTACAO");
            migrationBuilder.DropTable(name: "LEITURA_SENSOR");
            migrationBuilder.DropTable(name: "PREFERENCIA_SENSORIAL");
            migrationBuilder.DropTable(name: "USUARIO_PERFIL");
            migrationBuilder.DropTable(name: "RESERVA_ESTACAO");
            migrationBuilder.DropTable(name: "SENSOR");
            migrationBuilder.DropTable(name: "USUARIO");
            migrationBuilder.DropTable(name: "STATUS_RESERVA");
            migrationBuilder.DropTable(name: "ESTACAO_TRABALHO");
            migrationBuilder.DropTable(name: "TIPO_SENSOR");
            migrationBuilder.DropTable(name: "SETOR");
            migrationBuilder.DropTable(name: "ZONA_SENSORIAL");
            migrationBuilder.DropTable(name: "PERFIL");
            migrationBuilder.DropTable(name: "FILIAL");
            migrationBuilder.DropTable(name: "EMPRESA");
        }
    }
}
