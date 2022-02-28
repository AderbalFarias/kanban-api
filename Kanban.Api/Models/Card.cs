using Kanban.Api.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kanban.Api.Models
{
    //[Validator(typeof(CardValidator))]
    public class Card
    {
        [Key]
        public Guid Id { get; set; }


        [Required]
        public string Titulo { get; set; }


        [Required]
        public string Conteudo { get; set; }


        [Required]
        public string Lista { get; set; }

        internal Domain.Entities.Card MapToEntityAdd() => new Domain.Entities.Card(this.Titulo, this.Conteudo, this.Lista);

        internal Domain.Entities.Card MapToEntity() => new Domain.Entities.Card(this.Id, this.Titulo, this.Conteudo, this.Lista);

        internal Card MapToCard(Domain.Entities.Card entity) =>
            new Card
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Conteudo = entity.Conteudo,
                Lista = entity.Lista
            };
    }
}
