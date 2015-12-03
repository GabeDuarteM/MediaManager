namespace MediaManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Episodios",
                c => new
                    {
                        nCdEpisodio = c.Int(nullable: false, identity: true),
                        sDsEpisodio = c.String(),
                        nNrEpisodio = c.Int(nullable: false),
                        nNrTemporada = c.Int(nullable: false),
                        nNrAbsoluto = c.Int(),
                        nNrEstreiaDepoisTemporada = c.Int(),
                        nNrEstreiaAntesEpisodio = c.Int(),
                        nNrEstreiaAntesTemporada = c.Int(),
                        sLkArtwork = c.String(),
                        nIdEstadoEpisodio = c.Int(nullable: false),
                        tDtEstreia = c.DateTime(),
                        nCdEpisodioAPI = c.Int(nullable: false),
                        nCdTemporadaAPI = c.Int(nullable: false),
                        nCdVideo = c.Int(nullable: false),
                        nCdVideoAPI = c.Int(nullable: false),
                        bFlRenomeado = c.Boolean(nullable: false),
                        sDsIdioma = c.String(),
                        sNrUltimaAtualizacao = c.String(),
                        sDsFilepath = c.String(),
                        sDsFilepathOriginal = c.String(),
                        sDsSinopse = c.String(),
                        dNrAvaliacao = c.Double(),
                        nQtAvaliacao = c.Int(),
                    })
                .PrimaryKey(t => t.nCdEpisodio)
                .ForeignKey("dbo.Series", t => t.nCdVideo, cascadeDelete: true)
                .Index(t => t.nCdVideo);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        nCdVideo = c.Int(nullable: false, identity: true),
                        sDsTitulo = c.String(),
                        nCdApi = c.Int(nullable: false),
                        sNmAtores = c.String(),
                        sDsEstreiaDiaSemana = c.String(),
                        sDsEstreiaHorario = c.String(),
                        sDsClassificacao = c.String(),
                        tDtEstreia = c.DateTime(),
                        sDsGeneros = c.String(),
                        sDsImgFanart = c.String(),
                        sDsImgPoster = c.String(),
                        bFlAnime = c.Boolean(nullable: false),
                        sDsIdioma = c.String(),
                        sNrUltimaAtualizacao = c.String(),
                        sDsEmissora = c.String(),
                        sDsSinopse = c.String(),
                        dNrAvaliacao = c.Double(),
                        nQtAvaliacao = c.Int(),
                        nQtDuracaoEpisodio = c.Int(),
                        sDsStatus = c.String(),
                        sDsPasta = c.String(),
                    })
                .PrimaryKey(t => t.nCdVideo);
            
            CreateTable(
                "dbo.SerieAlias",
                c => new
                    {
                        nCdAlias = c.Int(nullable: false, identity: true),
                        sDsAlias = c.String(),
                        nNrEpisodio = c.Int(nullable: false),
                        nCdVideo = c.Int(nullable: false),
                        nNrTemporada = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.nCdAlias)
                .ForeignKey("dbo.Series", t => t.nCdVideo, cascadeDelete: true)
                .Index(t => t.nCdVideo);
            
            CreateTable(
                "dbo.Feeds",
                c => new
                    {
                        nCdFeed = c.Int(nullable: false, identity: true),
                        sDsFeed = c.String(nullable: false),
                        sLkFeed = c.String(nullable: false),
                        nIdTipoConteudo = c.Int(nullable: false),
                        nNrPrioridade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.nCdFeed);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SerieAlias", "nCdVideo", "dbo.Series");
            DropForeignKey("dbo.Episodios", "nCdVideo", "dbo.Series");
            DropIndex("dbo.SerieAlias", new[] { "nCdVideo" });
            DropIndex("dbo.Episodios", new[] { "nCdVideo" });
            DropTable("dbo.Feeds");
            DropTable("dbo.SerieAlias");
            DropTable("dbo.Series");
            DropTable("dbo.Episodios");
        }
    }
}
