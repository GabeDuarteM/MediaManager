using System;

namespace MediaManager.Helpers
{
    /// <summary>
    /// Classe contendo todos os enums utilizados.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// NOVO = Recém criado (new Video()), Simples = Video com as informações basicas vindas da API, Completo = Video com as informações completas vindas da API.
        /// </summary>
        public enum Estado
        {
            Novo,
            Simples,
            Completo,
            CompletoSemForeignKeys
        }

        /// <summary>
        /// Define o tipo de conteúdo a ser usado.
        /// </summary>
        public enum ContentType
        {
            Selecione = 0,
            Filme = 1,
            Série = 2,
            Anime = 3,
            Episódio = 4,
            AnimeFilmeSérie = 7
        }

        public enum TipoImagem
        {
            Todos = 0,
            Fanart = 1,
            Poster = 2
        }

        [Serializable]
        public enum MetodoDeProcessamento
        {
            HardLink = 0,
            Copiar = 1
        }

        public enum EstadoEpisodio
        {
            Selecione = 0,
            Arquivado = 1,
            Baixado = 2,
            Baixando = 3,
            Desejado = 4,
            Ignorado = 5
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
                        return ContentType.Selecione;

                    case "Filme":
                        return ContentType.Filme;

                    case "Série":
                        return ContentType.Série;

                    case "Anime":
                        return ContentType.Anime;

                    case "Episódio":
                        return ContentType.Episódio;

                    case "Filme, Serie e Anime":
                        return ContentType.AnimeFilmeSérie;

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
                    case ContentType.Selecione:
                        return "";

                    case ContentType.Filme:
                        return "Filme";

                    case ContentType.Série:
                        return "Série";

                    case ContentType.Anime:
                        return "Anime";

                    case ContentType.Episódio:
                        return "Episódio";

                    case ContentType.AnimeFilmeSérie:
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