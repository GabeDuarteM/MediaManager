using System;
using System.Text.RegularExpressions;
using MediaManager.Helpers;
using MediaManager.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaManager.Tests
{
    [TestClass]
    public class TestesReconhecimentoDaSerie
    {
        private static TestContext context;

        [ClassInitialize()]
        public static void PrepararMassaDeDados(TestContext testContext)
        {
            context = testContext;
            TesteHelper.GerarMassaDeDados();
        }

        [TestMethod]
        public void TestarSeriesRegexS00E00()
        {
            #region Preparações

            Episodio episodio;
            string nomeRenomeado;
            string resultadoEsperado;
            string formatoRenomeio = "Titulo ({Titulo}) - TituloEpisodio ({TituloEpisodio}) - Temporada ({Temporada}) - Episodio ({Episodio}) - Absoluto ({Absoluto}) - SxEE ({SxEE}) - S00E00 ({S00E00})";

            #endregion Preparações

            #region Asserts

            /////

            episodio = new Episodio();
            episodio.sDsFilepath = "Arrow.S04E01.Green.Arrow.1080p.WEB-DL.6CH.x265.HEVC-PSA";
            if (episodio.IdentificarEpisodio())
            {
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Arrow) - TituloEpisodio (Green Arrow) - Temporada (04) - Episodio (01) - Absoluto (70) - SxEE (4x01) - S00E00 (S04E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Arrow) - TituloEpisodio (The Candidate) - Temporada (04) - Episodio (02) - Absoluto (71) - SxEE (4x02) - S00E00 (S04E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Arrow) - TituloEpisodio (Green Arrow) - Temporada (04) - Episodio (01) - Absoluto (70) - SxEE (4x01) - S00E00 (S04E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Better Call Saul) - TituloEpisodio (Marco) - Temporada (01) - Episodio (10) - Absoluto (10) - SxEE (1x10) - S00E00 (S01E10)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Falling Skies) - TituloEpisodio (Everybody Has Their Reasons) - Temporada (05) - Episodio (07) - Absoluto (49) - SxEE (5x07) - S00E00 (S05E07)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Game of Thrones) - TituloEpisodio (The Gift) - Temporada (05) - Episodio (07) - Absoluto (47) - SxEE (5x07) - S00E00 (S05E07)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Game of Thrones) - TituloEpisodio (Hardhome) - Temporada (05) - Episodio (08) - Absoluto (48) - SxEE (5x08) - S00E00 (S05E08)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Gotham) - TituloEpisodio (Under the Knife) - Temporada (01) - Episodio (20) - Absoluto (20) - SxEE (1x20) - S00E00 (S01E20)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Gotham) - TituloEpisodio (The Anvil or the Hammer) - Temporada (01) - Episodio (21) - Absoluto (21) - SxEE (1x21) - S00E00 (S01E21)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Gotham) - TituloEpisodio (All Happy Families Are Alike) - Temporada (01) - Episodio (22) - Absoluto (22) - SxEE (1x22) - S00E00 (S01E22)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Marvel's Agents of S.H.I.E.L.D.) - TituloEpisodio (S.O.S. (1)) - Temporada (02) - Episodio (21) - Absoluto (43) - SxEE (2x21) - S00E00 (S02E21)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Marvel's Agents of S.H.I.E.L.D.) - TituloEpisodio (Laws of Nature) - Temporada (03) - Episodio (01) - Absoluto (45) - SxEE (3x01) - S00E00 (S03E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Marvel's Daredevil) - TituloEpisodio (Stick) - Temporada (01) - Episodio (07) - Absoluto (07) - SxEE (1x07) - S00E00 (S01E07)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (The Big Bang Theory) - TituloEpisodio (The Matrimonial Momentum) - Temporada (09) - Episodio (01) - Absoluto (184) - SxEE (9x01) - S00E00 (S09E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (The Big Bang Theory) - TituloEpisodio (The Skywalker Incursion) - Temporada (08) - Episodio (19) - Absoluto (178) - SxEE (8x19) - S00E00 (S08E19)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (The Flash (2014)) - TituloEpisodio (The Trap) - Temporada (01) - Episodio (20) - Absoluto (20) - SxEE (1x20) - S00E00 (S01E20)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (The Flash (2014)) - TituloEpisodio (The Man Who Saved Central City) - Temporada (02) - Episodio (01) - Absoluto (24) - SxEE (2x01) - S00E00 (S02E01)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (The Following) - TituloEpisodio (Flesh & Blood) - Temporada (03) - Episodio (08) - Absoluto () - SxEE (3x08) - S00E00 (S03E08)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
                nomeRenomeado = Helper.RenomearConformePreferencias(episodio, formatoRenomeio);
                resultadoEsperado = "Titulo (Under the Dome) - TituloEpisodio (Move On & But I'm Not) - Temporada (03) - Episodio (01 & 02) - Absoluto (27 & 27) - SxEE (3x01x02) - S00E00 (S03E01E02)";
                Assert.AreEqual(resultadoEsperado, nomeRenomeado);
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
            //Helpers.Helper.RegexEpisodio regex = new Helpers.Helper.RegexEpisodio();

            //string filename = "Dragon Ball Super Episode 011".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //EpisodeToRename episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //Match match = regex.regex_Fansub0000.Match(episodio.Filename);
            //var actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Dragon Ball Super - 011", actual);

            /////

            //filename = "Fate Stay Night Ep 01 The Day of the Beginning [720p,BluRay,x264] - THORA v2".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Fate Stay Night - 01", actual);

            /////

            //filename = "Fate Stay Night Ep01 The Day of the Beginning [720p,BluRay,x264] - THORA v2".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Fate Stay Night - 01", actual);

            /////

            //filename = "[GJM-Mezashite] Charlotte - 01 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Charlotte - 01", actual);

            /////

            //filename = "[GJM-Mezashite] Charlotte - 01&02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Charlotte - 01&02", actual);

            /////

            //filename = "[GJM-Mezashite] Charlotte - 01 & 02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Charlotte - 01 & 02", actual);

            /////

            //filename = "[GJM-Mezashite] Charlotte - 01-02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Charlotte - 01-02", actual);

            /////

            //filename = "[GJM-Mezashite] Charlotte - 01 02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Charlotte - 01 02", actual);

            /////

            //filename = "[GJM-Mezashite] Charlotte - 01 - 02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Charlotte - 01 - 02", actual);

            /////

            //filename = "[DeadFish] Dragon Ball Super - 01 [720p][AAC]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Dragon Ball Super - 01", actual);

            /////

            //filename = "[DameDesuYo] DanMachi - 13 (1280x720 10bit AAC) [498496B0].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("DanMachi - 13", actual);

            /////

            //filename = "[Titania-Fansub]_Fairy_Tail_232_[VOSTFR]_[720p]_[9B6E1B3B].mp4".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Fairy Tail - 232", actual);

            /////

            //filename = "[Eclipse] Fullmetal Alchemist Brotherhood - 02 (1280x720 h264) [8452C4BF].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Fullmetal Alchemist Brotherhood - 02", actual);

            /////

            //filename = "[AnimeRG] One Piece - 702 [720p] [eng Subbed].mp4".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("One Piece - 702", actual);

            /////

            //filename = "[Hiryuu] Sword Art Online II - 01 [720p H264 AAC][FF6BE0B6].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Sword Art Online II - 01", actual);

            /////

            //filename = "[Hiryuu] Sword Art Online II - 05v2 [720p H264 AAC][9392CAF8].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Sword Art Online II - 05", actual);

            /////

            //filename = "[Hiryuu] Sword Art Online II - 14.5 [720p H264 AAC][226BC0EE].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Sword Art Online II - 14", actual);

            /////

            //filename = "[HorribleSubs] Sword Art Online - 01 [720p].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Sword Art Online - 01", actual);

            /////

            //filename = "[UTWoots] Sword Art Online - 02 [720p][120D9768].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //episodio = new EpisodeToRename() { Filename = filename };
            //Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            //match = regex.regex_Fansub0000.Match(episodio.Filename);
            //actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            //Assert.AreEqual("Sword Art Online - 02", actual);
        }

        [TestMethod]
        public void TestarSeriesRegex0x00()
        {
            //Helpers.Helper.RegexEpisodio regex = new Helpers.Helper.RegexEpisodio();

            //{
            //    string filename = "The Big Bang Theory 9x08 The Mystery Date Observation 720p".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //    EpisodeToRename episodio = new EpisodeToRename() { Filename = filename };
            //    Assert.IsTrue(regex.regex_0x00.IsMatch(episodio.Filename));
            //    Match match = regex.regex_0x00.Match(episodio.Filename);
            //    string sNmSerie = match.Groups["name"].Value.Trim();
            //    int nNrTemporada = int.MinValue;
            //    int nNrEpisodio = int.MinValue;
            //    int.TryParse(match.Groups["season"].Value.Trim(), out nNrTemporada);
            //    int.TryParse(match.Groups["episodes"].Value.Trim(), out nNrEpisodio);
            //    var actual = sNmSerie + " " + nNrTemporada + "x" + nNrEpisodio.ToString("00");
            //    Assert.AreEqual("The Big Bang Theory 9x08", actual);
            //}

            //{
            //    string filename = "Arrow - 4x01 - Green Arrow".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            //    EpisodeToRename episodio = new EpisodeToRename() { Filename = filename };
            //    Assert.IsTrue(regex.regex_0x00.IsMatch(episodio.Filename));
            //    Match match = regex.regex_0x00.Match(episodio.Filename);
            //    string sNmSerie = match.Groups["name"].Value.Trim();
            //    int nNrTemporada = int.MinValue;
            //    int nNrEpisodio = int.MinValue;
            //    int.TryParse(match.Groups["season"].Value.Trim(), out nNrTemporada);
            //    int.TryParse(match.Groups["episodes"].Value.Trim(), out nNrEpisodio);
            //    var actual = sNmSerie + " " + nNrTemporada + "x" + nNrEpisodio.ToString("00");
            //    Assert.AreEqual("Arrow 4x01", actual);
            //}
        }
    }
}