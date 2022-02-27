using System;

namespace Kanban.Api.Models
{
    public class CardModel
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; }

        public string Conteudo { get; set; }

        public string Lista { get; set; }
    }
}
