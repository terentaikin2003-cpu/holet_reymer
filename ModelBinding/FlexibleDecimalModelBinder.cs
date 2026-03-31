using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HotelReymer.ModelBinding;

/// <summary>
/// Принимает десятичные числа и с запятой (ru-RU), и с точкой (как в invariant), чтобы не было ошибок при смешанном вводе.
/// </summary>
public sealed class FlexibleDecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            if (bindingContext.ModelType == typeof(decimal?))
                bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
        var raw = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(raw))
        {
            if (bindingContext.ModelType == typeof(decimal?))
                bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var trimmed = raw.Trim().Replace('\u00A0', ' ');

        if (TryParseDecimal(trimmed, out var parsed))
        {
            bindingContext.Result = ModelBindingResult.Success(parsed);
            return Task.CompletedTask;
        }

        bindingContext.ModelState.TryAddModelError(
            modelName,
            string.Format(
                CultureInfo.CurrentCulture,
                "Значение «{0}» недопустимо для поля {1}.",
                raw,
                bindingContext.ModelMetadata.GetDisplayName()));

        return Task.CompletedTask;
    }

    private static bool TryParseDecimal(string s, out decimal result)
    {
        if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
            return true;
        if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
            return true;

        var compact = s.Replace(" ", "", StringComparison.Ordinal).Replace("\u00A0", "", StringComparison.Ordinal);
        if (compact != s)
        {
            if (decimal.TryParse(compact, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
                return true;
            if (decimal.TryParse(compact, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                return true;
        }

        result = default;
        return false;
    }
}

public sealed class FlexibleDecimalModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var t = context.Metadata.ModelType;
        if (t == typeof(decimal) || t == typeof(decimal?))
            return new FlexibleDecimalModelBinder();
        return null;
    }
}
