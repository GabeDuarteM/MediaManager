using System;

namespace MediaManager.Helpers
{
    /// <summary>
    /// Classe contendo todos os enums utilizados.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Define o tipo de conteúdo a ser usado.
        /// </summary>
        public enum ContentType
        {
            unknown = 0,
            movie = 1,
            show = 2,
            anime = 3,
            season = 4,
            episode = 5,
            person = 6,
            movieShowAnime = 7
        }

        public enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        public enum TipoImagem
        {
            Todos = 0,
            Fanart = 1,
            Poster = 2
        }

        /// <summary>
        /// Transforma a string em um enum
        /// </summary>
        /// <param name="str">String a ser transformada</param>
        /// <param name="enumType">Tipo do enum destino</param>
        /// <returns>Enum do tipo destino escolhido</returns>
        public static object ToEnum(string str, Type enumType)
        {
            if (enumType == typeof(ContentType))
            {
                switch (str)
                {
                    case "":
                        return ContentType.unknown;

                    case "Filme":
                        return ContentType.movie;

                    case "Série":
                        return ContentType.show;

                    case "Anime":
                        return ContentType.anime;

                    case "Temporada":
                        return ContentType.season;

                    case "Episódio":
                        return ContentType.episode;

                    case "Pessoa":
                        return ContentType.person;

                    case "Filme, Serie e Anime":
                        return ContentType.movieShowAnime;

                    default:
                        return null;
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido", "enumType");
            }
        }

        /// <summary>
        /// Transforma o enum numa string "enfeitada".
        /// </summary>
        public static string ToString(object enumItem)
        {
            if (enumItem.GetType() == typeof(ContentType))
            {
                switch ((ContentType)enumItem)
                {
                    case ContentType.unknown:
                        return "";

                    case ContentType.movie:
                        return "Filme";

                    case ContentType.show:
                        return "Série";

                    case ContentType.anime:
                        return "Anime";

                    case ContentType.season:
                        return "Temporada";

                    case ContentType.episode:
                        return "Episódio";

                    case ContentType.person:
                        return "Pessoa";

                    case ContentType.movieShowAnime:
                        return "Filme, Serie e Anime";

                    default:
                        return null;
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido", "enumItem");
            }
        }
    }
}