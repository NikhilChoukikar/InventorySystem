using FluentValidation;
using InventorySystem.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Application.Validators
{
        public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
        {
            public CreateProductDtoValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(200).WithMessage("Name must be 200 characters or fewer.");

                RuleFor(x => x.CategoryId)
                    .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");

                RuleFor(x => x.SubCategoryId)
                    .GreaterThanOrEqualTo(0).WithMessage("SubCategoryId cannot be negative.");

                RuleFor(x => x.Quantity)
                    .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

                RuleFor(x => x.Price)
                    .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");
            }
        }
    }

