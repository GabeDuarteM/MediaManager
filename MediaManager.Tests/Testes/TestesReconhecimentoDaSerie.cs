using System;
using System.Text.RegularExpressions;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Tests.Preparacoes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MediaManager.Tests.Testes
{
    [TestClass]
    public class TestesReconhecimentoDaSerie
    {
        private static TestContext testContext;

        [AssemblyInitialize]
        public static void OnStartUp(TestContext testContext)
        {
            Startup.OnStartUp(testContext);
        }

        [ClassInitialize()]
        public static void PrepararMassaDeDados(TestContext testContext)
        {
            TestesReconhecimentoDaSerie.testContext = testContext;
        }

        [TestMethod]
        public void TestarSeriesRegexS00E00()
        {
            #region Preparações

            Episodio episodio;
            string nomeRenomeado;
            string resultadoEsperado;
            string formatoRenomeio = "Titulo ({Titulo}) - Temporada ({Temporada}) - Episodio ({Episodio}) - Absoluto ({Absoluto}) - SxEE ({SxEE}) - S00E00 ({S00E00})";

            #endregion Preparações

            #region Asserts

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Arrow.S04E01.Green.Arrow.1080p.WEB-DL.6CH.x265.HEVC-PSA";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Arrow) - Temporada (04) - Episodio (01) - Absoluto (70) - SxEE (4x01) - S00E00 (S04E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Arrow.S04E02.1080p.HDTV.X264-DIMENSION";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Arrow) - Temporada (04) - Episodio (02) - Absoluto (71) - SxEE (4x02) - S00E00 (S04E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Arrow.S04E01.HDTV.XviD-FUM[ettv]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Arrow) - Temporada (04) - Episodio (01) - Absoluto (70) - SxEE (4x01) - S00E00 (S04E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Better.Call.Saul.S01E10.720p.HDTV.X264-DIMENSION";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Better Call Saul) - Temporada (01) - Episodio (10) - Absoluto (10) - SxEE (1x10) - S00E00 (S01E10)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Falling Skies S05E07 720p WEB-DL x264-Belex - Dual Audio";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Falling Skies) - Temporada (05) - Episodio (07) - Absoluto (49) - SxEE (5x07) - S00E00 (S05E07)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Game.of.Thrones.S05E07.720p.HDTV.x264-IMMERSE";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Game of Thrones) - Temporada (05) - Episodio (07) - Absoluto (47) - SxEE (5x07) - S00E00 (S05E07)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "game.of.thrones.s05e08.proper.720p.hdtv.x264-0sec";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Game of Thrones) - Temporada (05) - Episodio (08) - Absoluto (48) - SxEE (5x08) - S00E00 (S05E08)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Gotham.S01E20.Under.the.Knife.720p.WEB-DL.2CH.x265.HEVC-PSA";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Gotham) - Temporada (01) - Episodio (20) - Absoluto (20) - SxEE (1x20) - S00E00 (S01E20)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Gotham.S01E21.720p.HDTV.X264-DIMENSION";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Gotham) - Temporada (01) - Episodio (21) - Absoluto (21) - SxEE (1x21) - S00E00 (S01E21)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Gotham.S01E22.All.Happy.Families.Are.Alike.720p.WEB-DL.DD5.1.AAC2.0.H.264-YFN";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Gotham) - Temporada (01) - Episodio (22) - Absoluto (22) - SxEE (1x22) - S00E00 (S01E22)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Marvel's.Agents.of.S.H.I.E.L.D.S02E21.S.O.S.Part.1.720p.WEB-DL.DD5.1.H.264-BS";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Marvel's Agents of S.H.I.E.L.D.) - Temporada (02) - Episodio (21) - Absoluto (43) - SxEE (2x21) - S00E00 (S02E21)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Marvel's.Agents.of.S.H.I.E.L.D.S03E01.Laws.of.Nature.720p.WEB-DL.DD5.1.H.264-CtrlHD";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Marvel's Agents of S.H.I.E.L.D.) - Temporada (03) - Episodio (01) - Absoluto (45) - SxEE (3x01) - S00E00 (S03E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Marvels.Daredevil.S01E07.Stick.1080p.NF.WEBRip.DD5.1.x264-NTb";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Marvel's Daredevil) - Temporada (01) - Episodio (07) - Absoluto (07) - SxEE (1x07) - S00E00 (S01E07)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "the.big.bang.theory.s09e01.720p.hdtv.hevc.x265.rmteam";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (The Big Bang Theory) - Temporada (09) - Episodio (01) - Absoluto (184) - SxEE (9x01) - S00E00 (S09E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "The.Big.Bang.Theory.S08E19.The.Skywalker.Incursion.720p.WEB-DL.DD5.1.AAC2.0.H.264-YFN";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (The Big Bang Theory) - Temporada (08) - Episodio (19) - Absoluto (178) - SxEE (8x19) - S00E00 (S08E19)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "The Flash S01E20 720p HDTV x264 AAC - Ozlem";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (The Flash (2014)) - Temporada (01) - Episodio (20) - Absoluto (20) - SxEE (1x20) - S00E00 (S01E20)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "The.Flash.2014.S02E01.The.Man.Who.Saved.Central.City.720p.WEB-DL.DD5.1.H.264-CtrlHD";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (The Flash (2014)) - Temporada (02) - Episodio (01) - Absoluto (24) - SxEE (2x01) - S00E00 (S02E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "The.Following.S03E08.720p.HDTV.X264-DIMENSION";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (The Following) - Temporada (03) - Episodio (08) - Absoluto (38) - SxEE (3x08) - S00E00 (S03E08)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Under.the.Dome.S03E01E02.Move.On-But.Im.Not.720p.WEB-DL.DD5.1.H264-RARBG";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Under the Dome) - Temporada (03) - Episodio (01 & 02) - Absoluto (27 & 27) - SxEE (3x01x02) - S00E00 (S03E01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            #endregion Asserts
        }

        [TestMethod]
        public void TestarSeriesRegexFansub0000()
        {
            #region Preparações

            Episodio episodio;
            string nomeRenomeado;
            string resultadoEsperado;
            string formatoRenomeio = "Titulo ({Titulo}) - Temporada ({Temporada}) - Episodio ({Episodio}) - Absoluto ({Absoluto}) - SxEE ({SxEE}) - S00E00 ({S00E00})";

            #endregion Preparações

            #region Asserts

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Dragon Ball Super Episode 011";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Dragon Ball Super) - Temporada (01) - Episodio (11) - Absoluto (11) - SxEE (1x11) - S00E00 (S01E11)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Fate Stay Night Ep 01 The Day of the Beginning [720p,BluRay,x264] - THORA v2";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (FateStay Night) - Temporada (01) - Episodio (01) - Absoluto (01) - SxEE (1x01) - S00E00 (S01E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[GJM-Mezashite] Charlotte - 01 [40A3E193]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Charlotte (2015)) - Temporada (01) - Episodio (01) - Absoluto (01) - SxEE (1x01) - S00E00 (S01E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[GJM-Mezashite] Charlotte - 01&02 [40A3E193]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Charlotte (2015)) - Temporada (01) - Episodio (01 & 02) - Absoluto (01 & 02) - SxEE (1x01x02) - S00E00 (S01E01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[GJM-Mezashite] Charlotte - 01 & 02 [40A3E193]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Charlotte (2015)) - Temporada (01) - Episodio (01 & 02) - Absoluto (01 & 02) - SxEE (1x01x02) - S00E00 (S01E01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[GJM-Mezashite] Charlotte - 01-02 [40A3E193]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Charlotte (2015)) - Temporada (01) - Episodio (01 & 02) - Absoluto (01 & 02) - SxEE (1x01x02) - S00E00 (S01E01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[GJM-Mezashite] Charlotte - 01 02 [40A3E193]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Charlotte (2015)) - Temporada (01) - Episodio (01 & 02) - Absoluto (01 & 02) - SxEE (1x01x02) - S00E00 (S01E01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[GJM-Mezashite] Charlotte - 01 - 02 [40A3E193]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Charlotte (2015)) - Temporada (01) - Episodio (01 & 02) - Absoluto (01 & 02) - SxEE (1x01x02) - S00E00 (S01E01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[DeadFish] Dragon Ball Super - 01 [720p][AAC]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Dragon Ball Super) - Temporada (01) - Episodio (01) - Absoluto (01) - SxEE (1x01) - S00E00 (S01E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[DameDesuYo] DanMachi - 13 (1280x720 10bit AAC) [498496B0]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Is It Wrong to Try to Pick Up Girls in a Dungeon) - Temporada (01) - Episodio (13) - Absoluto (13) - SxEE (1x13) - S00E00 (S01E13)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[Titania-Fansub]_Fairy_Tail_232_[VOSTFR]_[720p]_[9B6E1B3B]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Fairy Tail) - Temporada (06) - Episodio (06) - Absoluto (232) - SxEE (6x06) - S00E00 (S06E06)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[HorribleSubs] Fairy Tail S2 - 40 [720p]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Fairy Tail) - Temporada (05) - Episodio (40) - Absoluto (215) - SxEE (5x40) - S00E00 (S05E40)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[Eclipse] Fullmetal Alchemist Brotherhood - 02 (1280x720 h264) [8452C4BF]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Fullmetal Alchemist Brotherhood) - Temporada (01) - Episodio (02) - Absoluto (02) - SxEE (1x02) - S00E00 (S01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[Hiryuu] Sword Art Online II - 01 [720p H264 AAC][FF6BE0B6]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Sword Art Online) - Temporada (02) - Episodio (01) - Absoluto (26) - SxEE (2x01) - S00E00 (S02E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[Hiryuu] Sword Art Online II - 05v2 [720p H264 AAC][9392CAF8]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Sword Art Online) - Temporada (02) - Episodio (05) - Absoluto (30) - SxEE (2x05) - S00E00 (S02E05)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[Hiryuu] Sword Art Online II - 14.5 [720p H264 AAC][226BC0EE]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Sword Art Online) - Temporada (02) - Episodio (14) - Absoluto (39) - SxEE (2x14) - S00E00 (S02E14)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[HorribleSubs] Sword Art Online - 01 [720p]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Sword Art Online) - Temporada (01) - Episodio (01) - Absoluto (01) - SxEE (1x01) - S00E00 (S01E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "[UTWoots] Sword Art Online - 02 [720p][120D9768]";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Sword Art Online) - Temporada (01) - Episodio (02) - Absoluto (02) - SxEE (1x02) - S00E00 (S01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            #endregion Asserts
        }

        [TestMethod]
        public void TestarSeriesRegex0x00()
        {
            #region Preparações

            Episodio episodio;
            string nomeRenomeado;
            string resultadoEsperado;
            string formatoRenomeio = "Titulo ({Titulo}) - Temporada ({Temporada}) - Episodio ({Episodio}) - Absoluto ({Absoluto}) - SxEE ({SxEE}) - S00E00 ({S00E00})";

            #endregion Preparações

            #region Asserts

            episodio = new Episodio();
            episodio.sDsFilepath = "The Big Bang Theory 9x08 The Mystery Date Observation 720p";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (The Big Bang Theory) - Temporada (09) - Episodio (08) - Absoluto (191) - SxEE (9x08) - S00E00 (S09E08)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Arrow - 4x01 - Green Arrow";
            if (episodio.IdentificarEpisodio())
            {
                episodio.oSerie.sFormatoRenomeioPersonalizado = formatoRenomeio;
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio);
                resultadoEsperado = "Titulo (Arrow) - Temporada (04) - Episodio (01) - Absoluto (70) - SxEE (4x01) - S00E00 (S04E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
                (App.Container.Resolve<IContext>() as MockContext).ResetarTodosDados();
            }
            else
            {
                throw new AssertFailedException("Não conseguiu identificar o episódio " + episodio.sDsFilepath);
            }

            #endregion Asserts
        }
    }
}