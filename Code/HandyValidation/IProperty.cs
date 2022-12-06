namespace HandyValidation
{
    public interface IProperty<T>
    {
        T Value { get; set; }

        bool IsReadonly { get; set; }
    }
}
