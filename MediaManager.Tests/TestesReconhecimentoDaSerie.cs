using System;
using System.Text.RegularExpressions;
using MediaManager.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaManager.Tests
{
    [TestClass]
    public class TestesReconhecimentoDaSerie
    {
        [TestMethod]
        public void TestarSeriesRegexS00E00()
        {
            Helpers.Helper.RegexEpisodio regex = new Helpers.Helper.RegexEpisodio();

            string filename = "Arrow.S04E01.Green.Arrow.1080p.WEB-DL.6CH.x265.HEVC-PSA".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            EpisodeToRename episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            Match match = regex.regex_S00E00.Match(episodio.Filename);
            var actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Arrow S04E01", actual);

            ///

            filename = "Arrow.S04E02.1080p.HDTV.X264-DIMENSION".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Arrow S04E02", actual);

            ///

            filename = "Arrow.S04E01.HDTV.XviD-FUM[ettv]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Arrow S04E01", actual);

            ///

            filename = "Better.Call.Saul.S01E10.720p.HDTV.X264-DIMENSION".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Better Call Saul S01E10", actual);

            ///

            filename = "Falling Skies S05E07 720p WEB-DL x264-Belex - Dual Audio".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Falling Skies S05E07", actual);

            ///

            filename = "Game.of.Thrones.S05E07.720p.HDTV.x264-IMMERSE".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Game of Thrones S05E07", actual);

            ///

            filename = "game.of.thrones.s05e08.proper.720p.hdtv.x264-0sec".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("game of thrones S05E08", actual);

            ///

            filename = "Gotham.S01E20.Under.the.Knife.720p.WEB-DL.2CH.x265.HEVC-PSA".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Gotham S01E20", actual);

            ///

            filename = "Gotham.S01E21.720p.HDTV.X264-DIMENSION".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Gotham S01E21", actual);

            ///

            filename = "Gotham.S01E22.All.Happy.Families.Are.Alike.720p.WEB-DL.DD5.1.AAC2.0.H.264-YFN".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Gotham S01E22", actual);

            ///

            filename = "Marvel's.Agents.of.S.H.I.E.L.D.S02E21.S.O.S.Part.1.720p.WEB-DL.DD5.1.H.264-BS".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Marvels Agents of S H I E L D S02E21", actual);

            ///

            filename = "Marvel's.Agents.of.S.H.I.E.L.D.S03E01.Laws.of.Nature.720p.WEB-DL.DD5.1.H.264-CtrlHD".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Marvels Agents of S H I E L D S03E01", actual);

            ///

            filename = "Marvels.Daredevil.S01E07.Stick.1080p.NF.WEBRip.DD5.1.x264-NTb".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Marvels Daredevil S01E07", actual);

            ///

            filename = "the.big.bang.theory.s09e01.720p.hdtv.hevc.x265.rmteam".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("the big bang theory S09E01", actual);

            ///

            filename = "The.Big.Bang.Theory.S08E19.The.Skywalker.Incursion.720p.WEB-DL.DD5.1.AAC2.0.H.264-YFN".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("The Big Bang Theory S08E19", actual);

            ///

            filename = "The Flash S01E20 720p HDTV x264 AAC - Ozlem".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("The Flash S01E20", actual);

            ///

            filename = "The.Flash.2014.S02E01.The.Man.Who.Saved.Central.City.720p.WEB-DL.DD5.1.H.264-CtrlHD".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("The Flash 2014 S02E01", actual);

            ///

            filename = "The.Following.S03E08.720p.HDTV.X264-DIMENSION".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("The Following S03E08", actual);

            ///
            filename = "Under.the.Dome.S03E01E02.Move.On-But.Im.Not.720p.WEB-DL.DD5.1.H264-RARBG".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_S00E00.IsMatch(episodio.Filename));
            match = regex.regex_S00E00.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " S" + match.Groups["season"].Value.Trim() + "E" + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Under the Dome S03E01E02", actual);
        }

        [TestMethod]
        public void TestarSeriesRegexFansub0000()
        {
            Helpers.Helper.RegexEpisodio regex = new Helpers.Helper.RegexEpisodio();

            string filename = "Dragon Ball Super Episode 011".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            EpisodeToRename episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            Match match = regex.regex_Fansub0000.Match(episodio.Filename);
            var actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Dragon Ball Super - 011", actual);

            ///

            filename = "Fate Stay Night Ep 01 The Day of the Beginning [720p,BluRay,x264] - THORA v2".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Fate Stay Night - 01", actual);

            ///

            filename = "Fate Stay Night Ep01 The Day of the Beginning [720p,BluRay,x264] - THORA v2".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Fate Stay Night - 01", actual);

            ///

            filename = "[GJM-Mezashite] Charlotte - 01 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Charlotte - 01", actual);

            ///

            filename = "[GJM-Mezashite] Charlotte - 01&02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Charlotte - 01&02", actual);

            ///

            filename = "[GJM-Mezashite] Charlotte - 01 & 02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Charlotte - 01 & 02", actual);

            ///

            filename = "[GJM-Mezashite] Charlotte - 01-02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Charlotte - 01-02", actual);

            ///

            filename = "[GJM-Mezashite] Charlotte - 01 02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Charlotte - 01 02", actual);

            ///

            filename = "[GJM-Mezashite] Charlotte - 01 - 02 [40A3E193]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Charlotte - 01 - 02", actual);

            ///

            filename = "[DeadFish] Dragon Ball Super - 01 [720p][AAC]".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Dragon Ball Super - 01", actual);

            ///

            filename = "[DameDesuYo] DanMachi - 13 (1280x720 10bit AAC) [498496B0].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("DanMachi - 13", actual);

            ///

            filename = "[Titania-Fansub]_Fairy_Tail_232_[VOSTFR]_[720p]_[9B6E1B3B].mp4".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Fairy Tail - 232", actual);

            ///

            filename = "[Eclipse] Fullmetal Alchemist Brotherhood - 02 (1280x720 h264) [8452C4BF].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Fullmetal Alchemist Brotherhood - 02", actual);

            ///

            filename = "[AnimeRG] One Piece - 702 [720p] [eng Subbed].mp4".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("One Piece - 702", actual);

            ///

            filename = "[Hiryuu] Sword Art Online II - 01 [720p H264 AAC][FF6BE0B6].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Sword Art Online II - 01", actual);

            ///

            filename = "[Hiryuu] Sword Art Online II - 05v2 [720p H264 AAC][9392CAF8].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Sword Art Online II - 05", actual);

            ///

            filename = "[Hiryuu] Sword Art Online II - 14.5 [720p H264 AAC][226BC0EE].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Sword Art Online II - 14", actual);

            ///

            filename = "[HorribleSubs] Sword Art Online - 01 [720p].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Sword Art Online - 01", actual);

            ///

            filename = "[UTWoots] Sword Art Online - 02 [720p][120D9768].mkv".Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
            episodio = new EpisodeToRename() { Filename = filename };
            Assert.IsTrue(regex.regex_Fansub0000.IsMatch(episodio.Filename));
            match = regex.regex_Fansub0000.Match(episodio.Filename);
            actual = match.Groups["name"].Value.Trim() + " - " + match.Groups["episodes"].Value.Trim();
            Assert.AreEqual("Sword Art Online - 02", actual);
        }
    }
}