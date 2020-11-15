using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerAPI.ModelBinder
{
    public class TransactionListModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                Exception exception = new Exception($"{nameof(TransactionListModelBinder)} must be a IEnumerable type");
                return Task.FromException(exception);
            }

            string valuesInput = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            if (String.IsNullOrEmpty(valuesInput))
                return Task.FromException(new Exception($"{nameof(TransactionListModelBinder)} no value input"));

            string[] valuesCollection = valuesInput.Split(',',StringSplitOptions.RemoveEmptyEntries);
            List<int> parsedValues = new List<int>();

            foreach(string valueAux in valuesCollection)
            {
                int? valueParsed =  Utilities.TypeConverter.ToInteger(valueAux);

                if (valueParsed.HasValue)
                    parsedValues.Add(valueParsed.Value);
            }

            bindingContext.Result = ModelBindingResult.Success(parsedValues);
            return Task.CompletedTask;
        }
    }
}
