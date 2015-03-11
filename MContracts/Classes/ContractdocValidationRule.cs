using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using MCDomain.Model;
using MContracts.Properties;
using MContracts.ViewModel;

namespace MContracts.Classes
{
    class ContractdocValidationRule: ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            BindingGroup bindingGroup = value as BindingGroup;
            if (bindingGroup == null) return new ValidationResult(false, "ContractdocValidationRule should only be used with a BindingGroup");
            if (bindingGroup.Items.Count == 1)
            {
                object item = bindingGroup.Items[0];
                ContractdocCardViewModel viewModel = item as ContractdocCardViewModel;

                if (viewModel == null) return ValidationResult.ValidResult;
                Contractdoc contract = viewModel.ContractObject;
                if (contract == null) return ValidationResult.ValidResult;

                if (contract.Contractstate != null)
                {
                    Contractstate state = contract.Contractstate;

                    if (state.IsSigned && !contract.Approvedat.HasValue)
                        return new ValidationResult(false,
                                                    "Договор должен находится в состоянии подписан, если задана дата подписания");
                    if (!state.IsSigned && contract.Approvedat.HasValue)
                        return new ValidationResult(false,
                                                    "Договор должен находится в состоянии подписан, если задана дата подписания");
                }
                else
                {
                    if (contract.Approvedat.HasValue)
                        return new ValidationResult(false, "Состояние подписания договора должно быть задано");
                }

                if ((contract.Currency != null) && (contract.Currency.IsForeign))
                {
                    if (!contract.Currencyrate.HasValue) return new ValidationResult(false, "Курс валюты должен быть задан.");   
                }

            }
            return ValidationResult.ValidResult;
        }
    }
}
