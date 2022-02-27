using System;

namespace Kanban.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; }

        public string Conteudo { get; set; }

        public string Lista { get; set; }
    }
}
