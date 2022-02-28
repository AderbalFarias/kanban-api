using FluentValidation;
using Kanban.Api.Models;

namespace Kanban.Api.Validators
{
    public class CardValidator : AbstractValidator<Card>
    {
        public CardValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O titulo não pode estar vazio")
                .Length(0, 100)
                .WithMessage("O titulo não poder ser maior que 100 characters.");

            RuleFor(x => x.Conteudo)
                .NotEmpty()
                .WithMessage("O conteudo não pode estar vazio")
                .Length(0, 2000)
                .WithMessage("O titulo não poder ser maior que 2000 characters.");

            RuleFor(x => x.Lista)
                .NotEmpty()
                .WithMessage("A lista não pode estar vazia")
                .Length(0, 50)
                .WithMessage("O titulo não poder ser maior que 50 characters.");

        }
    }
}
