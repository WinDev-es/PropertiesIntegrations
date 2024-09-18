internal static class GenericValidation
{
    private const int NoRecords = 0;
    private const string valueId = "IdProperty";
    private const string valueName = "Name";
    public static bool HasRecords<T>(IEnumerable<T> listItemsValidation)
    {
        int? quantityRecords = listItemsValidation?.Count();
        return (listItemsValidation != null && quantityRecords > NoRecords);
    }
    public static bool IsNotNull<T>(T validationItem) => (validationItem != null);
    public static bool IsGreaterThanZero<T>(T value) where T : struct, IComparable => value.CompareTo(default(T)) > 0;
    public static bool ValidateNullField<T>(T data, params Predicate<T>[] Validations) => Validations.ToList().Any(x => x(data));
    public static bool ValidateDuplicateNameField<T>(IEnumerable<T> data, Guid id, string name) => id == Guid.Empty ? data.Any(b => b.GetType().GetProperty(valueName).GetValue(b).ToString().ToLower().Trim() == name.ToLower().Trim()) :
                                                                    data.Any(b => b.GetType().GetProperty(valueName).GetValue(b).ToString().ToLower().Trim() == name.ToLower().Trim() &&
                                                                        Guid.Parse(b.GetType().GetProperty(valueId).GetValue(b).ToString()) != id);
}